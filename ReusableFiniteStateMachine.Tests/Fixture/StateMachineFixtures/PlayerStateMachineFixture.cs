#region CeCill-B license
#region English version
//Copyright Aurélien Pascal Maignan, (25 October 2026) 

//[aurelien.maignan@protonmail.com]

//This software is a computer program whose purpose is
//to test the reusable state machine software library named "ReusableFiniteStateMachine"
//using TUnit testing framework by Tom Longhurst (https://github.com/thomhurst/TUnit)

//This software is governed by the CeCILL-B license under French law and
//abiding by the rules of distribution of free software.  You can  use,
//modify and/ or redistribute the software under the terms of the CeCILL-B
//license as circulated by CEA, CNRS and INRIA at the following URL
//"http://www.cecill.info". 

//As a counterpart to the access to the source code and  rights to copy,
//modify and redistribute granted by the license, users are provided only
//with a limited warranty  and the software's author,  the holder of the
//economic rights, and the successive licensors  have only  limited
//liability. 

//In this respect, the user's attention is drawn to the risks associated
//with loading,  using,  modifying and/or developing or reproducing the
//software by the user in light of its specific status of free software,
//that may mean  that it is complicated to manipulate, and  that  also
//therefore means  that it is reserved for developers  and  experienced
//professionals having in-depth computer knowledge. Users are therefore
//encouraged to load and test the software's suitability as regards their
//requirements in conditions enabling the security of their systems and/or 
//data to be ensured and, more generally, to use and operate it in the 
//same conditions as regards security. 

//The fact that you are presently reading this means that you have had
//knowledge of the CeCILL-B license and that you accept its terms.
#endregion

#region French version
//Copyright Aurélien Pascal Maignan, (25 Octobre 2026) 

//aurelien.maignan@protonmail.com

//Ce logiciel est un programme informatique servant à tester
//la bibliothèque de machine à états réutilisable dénomé "ReusableFiniteStateMachine"
//en utilisant le cadriciel TUnit développé par Tom Longhurst (https://github.com/thomhurst/TUnit)

//Ce logiciel est régi par la licence CeCILL-B soumise au droit français et
//respectant les principes de diffusion des logiciels libres.Vous pouvez
//utiliser, modifier et/ou redistribuer ce programme sous les conditions
//de la licence CeCILL-B telle que diffusée par le CEA, le CNRS et l'INRIA 
//sur le site "http://www.cecill.info".

//En contrepartie de l'accessibilité au code source et des droits de copie,
//de modification et de redistribution accordés par cette licence, il n'est
//offert aux utilisateurs qu'une garantie limitée.  Pour les mêmes raisons,
//seule une responsabilité restreinte pèse sur l'auteur du programme,  le
//titulaire des droits patrimoniaux et les concédants successifs.

//A cet égard  l'attention de l'utilisateur est attirée sur les risques
//associés au chargement, à l'utilisation,  à la modification et/ou au
//développement et à la reproduction du logiciel par l'utilisateur étant 
//donné sa spécificité de logiciel libre, qui peut le rendre complexe à
//manipuler et qui le réserve donc à des développeurs et des professionnels
//avertis possédant  des  connaissances  informatiques approfondies.Les
//utilisateurs sont donc invités à charger  et  tester  l'adéquation  du
//logiciel à leurs besoins dans des conditions permettant d'assurer la
//sécurité de leurs systèmes et ou de leurs données et, plus généralement,
//à l'utiliser et l'exploiter dans les mêmes conditions de sécurité.

//Le fait que vous puissiez accéder à cet en-tête signifie que vous avez
//pris connaissance de la licence CeCILL-B, et que vous en avez accepté les
//termes. 
#endregion 
#endregion

using ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;
using ReusableFiniteStateMachine.Tests.Helpers;
using ReusableFiniteStateMachine.Sources.StateMachine;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using TUnit.Core.Interfaces;

namespace ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;

/// <summary>
/// The derived <see cref="StateMachine"/> class as a specialized state machine that drive 
/// a simple simulated game's player state flow.
/// Used to test a centralized version of a <see cref="IStateMachine"/> state machine.
/// Implements both <see cref="IAsyncDisposable"/> and <see cref="IAsyncInitializer"/> as it is used 
/// as a <see cref="ClassDataSources"/> fixture object argument in test methods.
/// </summary>
public class PlayerStateMachineFixture : StateMachine, IAsyncDisposable, IAsyncInitializer
{
  private readonly Lock _lock = new();

  public IState IdleState
    => field ??= GetOrAddState<IdleStateFixture>();

  public IState MoveState
    => field ??= GetOrAddState<MoveStateFixture>();

  public IState JumpState
    => field ??= GetOrAddState<JumpStateFixture>();

  public IState DeathState
    => field ??= GetOrAddState<DeathStateFixture>();

  public IState ExitState
    => field ??= GetOrAddState<ExitStateFixture>();

  /// <summary>
  /// Dispose this <see cref="PlayerStateMachineFixture"/> state machine object
  /// and reset the shared 'inner state' of any <see cref="PlayerStateMachineFixture"/> back their default values.
  /// </summary>
  /// <returns>The <see cref="ValueTask"/> to await this disposal.</returns>
  public ValueTask DisposeAsync()
  {
    Dispose();

    ResetToDefaults();

    return ValueTask.CompletedTask;
  }

  /// <summary>
  /// Initialize this <see cref="PlayerStateMachineFixture"/> state machine object,
  /// set its <see cref="IStateMachine.GetNextStateCentralized"/> delegate property
  /// and reset the shared 'inner state' of any <see cref="PlayerStateMachineFixture"/> back their default values.
  /// </summary>
  /// <returns>The <see cref="Task"/> to await this initialization.</returns>
  public Task InitializeAsync()
  {
    InitializeStateMachine(IdleState);

    GetNextStateCentralized = GetNextPlayerState;

    ResetToDefaults();

    return Task.CompletedTask;
  }

  /// <summary>
  /// As this <see cref="PlayerStateMachineFixture"/> is used to test a centralized version of a <see cref="IStateMachine"/> state machine,
  /// all logics of <see cref="IState"/> transitions are centralized in this method.
  /// </summary>
  /// <param name="argument">Not used here.</param>
  /// <returns>The next <see cref="IState"/> this <see cref="PlayerStateMachineFixture"/> state machine will transition into from its <see cref="IStateMachine.CurrentState"/> step.</returns>
  public IState? GetNextPlayerState(object? argument)
  {
    lock (_lock)
    {
      #region Early return checkings
      switch (true)
      {
        case true when CurrentState == default:
          return default;
        case true when CurrentState == StartState:
          return IdleState;
        case true when CurrentState == ExitState || CurrentState == EndState:
          return EndState;
        default:
          break;
      }
      #endregion

      if (!InputChanged && IsAlive)
        return CurrentState;

      InputChanged = false;

      var currentEnumState = (CurrentState as BasePlayerState)!.GetPlayerStateEnum();

      bool isInDeathState = CurrentState == DeathState;
      
      if (currentEnumState != PlayerStates.Exit)
      {
        if (CurrentInput == PlayerInputs.Exit)
          return ExitState;

        if (!IsAlive && !isInDeathState)
          return DeathState;
      }

      return currentEnumState switch
      {
        PlayerStates.None => default,
        PlayerStates.Idle when CurrentInput == PlayerInputs.Idle => IdleState,
        PlayerStates.Idle when CurrentInput == PlayerInputs.Move => MoveState,
        PlayerStates.Idle when CurrentInput == PlayerInputs.Jump => JumpState,
        PlayerStates.Move when CurrentInput == PlayerInputs.Idle => IdleState,
        PlayerStates.Move when CurrentInput == PlayerInputs.Move => MoveState,
        PlayerStates.Move when CurrentInput == PlayerInputs.Jump => JumpState,
        PlayerStates.Jump when CurrentInput == PlayerInputs.Idle && IsJumping => JumpState,
        PlayerStates.Jump when CurrentInput == PlayerInputs.Idle && !IsJumping => IdleState,
        PlayerStates.Jump when CurrentInput == PlayerInputs.Move && IsJumping => JumpState,
        PlayerStates.Jump when CurrentInput == PlayerInputs.Move && !IsJumping => MoveState,
        PlayerStates.Jump when CurrentInput == PlayerInputs.Jump => JumpState,
        PlayerStates.Death when CurrentCredit <= 0 => ExitState,
        PlayerStates.Death when CurrentInput == PlayerInputs.Idle => DeathState,
        PlayerStates.Death when CurrentInput != PlayerInputs.Idle => IdleState,/// An 'non-idle' input means wake-up from death
        PlayerStates.Exit => EndState,
        _ => default
      };
    }
  }
}
