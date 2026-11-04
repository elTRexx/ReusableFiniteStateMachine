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
using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;
using ReusableFiniteStateMachine.Sources.States.Interfaces;

namespace ReusableFiniteStateMachine.Tests.Helpers;

/// <summary>
/// Static class encapsulating the simulated 'inner states' of a game's player
/// driven by a <see cref="PlayerStateMachineFixture"/> player state machine.
/// </summary>
public static class PlayerManager
{
  public enum PlayerStates
  {
    None,
    Idle,
    Move,
    Jump,
    Death,
    Exit
  }

  public enum PlayerInputs
  {
    Idle,
    Move,
    Jump,
    Exit
  }

  public static int CurrentCredit
  {
    get;
    set
    {
      field = Math.Max(0, value);///>=0
    }
  } = DEFAULT_MAX_CREDIT;


  public static bool CanDie { get; set; } = true;
  public static bool IsAlive { get; set; } = true;
  public static bool IsJumping { get; set; } = false;
  public static bool WasExitAsked { get; set; } = false;
  public static bool InputChanged { get; set; } = false;

  public static PlayerInputs CurrentInput
  {
    get;
    set
    {
      if (field == value)
        return;

      field = value;
      InputChanged = true;
      if (field == PlayerInputs.Exit)
        WasExitAsked = true;
    }
  } = DEFAULT_INPUT;

  /// <summary>
  /// Utility method that return the corresponding <see cref="PlayerStates"/> enum value
  /// of given <see cref="IState"/> <paramref name="state"/>.
  /// </summary>
  /// <param name="state">The <see cref="IState"/> object we try to get its corresponding <see cref="PlayerStates"/> enum value.</param>
  /// <returns>The corresponding <see cref="PlayerStates"/> enum value of given <paramref name="state"/> object 
  /// if it correctly derive from <see cref="BasePlayerState"/> or <see cref="PlayerStates.None"/> instead.</returns>
  public static PlayerStates GetPlayerStateEnum(this IState state)
    => state is BasePlayerState playerState ? playerState.GetPlayerStateEnum() : PlayerStates.None;

  /// <summary>
  /// Utility method that return the corresponding <see cref="PlayerStates"/> enum value
  /// of given <see cref="BasePlayerState"/> <paramref name="playerState"/>.
  /// </summary>
  /// <param name="playerState">The <see cref="BasePlayerState"/> object we try to get its corresponding <see cref="PlayerStates"/> enum value.</param>
  /// <returns>The corresponding <see cref="PlayerStates"/> enum value of given <paramref name="playerState"/> object 
  /// if it is one of the concrete known sub-class of <see cref="BasePlayerState"/> or <see cref="PlayerStates.None"/> instead.</returns>
  public static PlayerStates GetPlayerStateEnum(this BasePlayerState playerState)
  {
    if (playerState?.GetType().IsAbstract ?? true)
      return PlayerStates.None;

    return playerState switch
    {
      IdleStateFixture => PlayerStates.Idle,
      MoveStateFixture => PlayerStates.Move,
      JumpStateFixture => PlayerStates.Jump,
      DeathStateFixture => PlayerStates.Death,
      ExitStateFixture => PlayerStates.Exit,
      _ => PlayerStates.None
    };
  }
}