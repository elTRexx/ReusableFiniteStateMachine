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

using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;

namespace ReusableFiniteStateMachine.Tests.Helpers;

/// <summary>
/// Static helper class for <see cref="PlayerStateMachineFixture"/> player sate machine handling.
/// Mainly used to simulate an 'internal game state' for player 
/// that is driven by a <see cref="PlayerStateMachineFixture"/> player sate machine.
/// </summary>
public static class PlayerStateMachineHelper
{
  private static bool _isDirty = true;

  private static (double, PlayerInputs)[]? _orderedInputProbabilities;
  private static (double, PlayerInputs)[] _OrderedInputProbabilities => _orderedInputProbabilities ??= new (double, PlayerInputs)[4];

  public static int InputCount 
  {
    get; 
    set
    {
      if(field / InputCeil != value / InputCeil)
        _isDirty = true;
      field = value;
    }
  } = 0;

  public static int RepeatCount { get; set; } = 0;
  public static int InputCeil 
  {
    get => field;
    set => field = Math.Max(1, Math.Abs(value));///avoid divide by 0
  } = DEFAULT_INPUT_CEIL;

  public static int InputStep => InputCount/InputCeil;


  private static Dictionary<PlayerInputs, double>? _inputsProbabilities;
  public static Dictionary<PlayerInputs, double> InputsProbabilities
  { 
    get 
    {
      if (_inputsProbabilities?.Count != 4)
        ResetInputsProbabilitiesToDefaults();

      return _inputsProbabilities!;
    } 
  }

  /// <summary>
  /// Asynchronously waiting given <paramref name="playerStateMachine"/> player state machine to end.
  /// Used for end to end testing.
  /// </summary>
  /// <param name="playerStateMachine">The <see cref="PlayerStateMachineFixture"/> player state machine to wait for its end.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to cancel this asynchronous methods and throwing back a <see cref="OperationCanceledException"/> exception.</param>
  /// <returns>The <see cref="Task"/> to be awaited while the <paramref name="playerStateMachine"/> player state machine is running.</returns>
  public static async Task AwaitStateMachineEndingAsync(this PlayerStateMachineFixture playerStateMachine, CancellationToken cancellationToken)
  {
    while (!playerStateMachine.IsEnded)
    {
      IsJumping = false; ///Force ending jummping

      cancellationToken.ThrowIfCancellationRequested();

      Task.Yield();
    }

    TestsLog.AppendLineShared($"[{RepeatCount}/{PLAYER_STATE_MACHINE_REPEAT}] Ending StateMachine in [{playerStateMachine.CurrentState?.GetType().Name}] State - {(IsAlive ? "ALIVE" : "DEAD")} {CurrentCredit}/{DEFAULT_MAX_CREDIT} | {(playerStateMachine.IsEnded ? "ENDED" : "RUNNING")}\n");
  }

  /// <summary>
  /// Starting a random <see cref="PlayerInputs"/> production loop until given <paramref name="playerStateMachine"/> player state machine end.
  /// Used for end to end testing.
  /// </summary>
  /// <param name="playerStateMachine">The <see cref="PlayerStateMachineFixture"/> player state machine that its end stop this <see cref="PlayerInputs"/> production loop.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to cancel this <see cref="PlayerInputs"/> production loop and throwing back a <see cref="OperationCanceledException"/> exception.</param>
  public static void StartRandomInputsLoop(this PlayerStateMachineFixture playerStateMachine, CancellationToken cancellationToken = default)
  {
    Task.Run(async () =>
    {
      while (!playerStateMachine.IsEnded)
      {
        CurrentInput = await GetNextInputAsync(DEFAULT_DELAY_MS, cancellationToken: playerStateMachine.CancellationTokenSource.Token);
        cancellationToken.ThrowIfCancellationRequested();
      }
    }, cancellationToken);
  }

  /// <summary>
  /// Asynchronously waiting given <paramref name="msDelay"/> in millisecond before returning a random <see cref="PlayerInputs"/>.
  /// Used for end to end testing.
  /// </summary>
  /// <param name="msDelay">The delay to wait (in millisecond) before requesting a new <see cref="PlayerInputs"/> to be returned.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to cancel this asynchronous methods.</param>
  /// <returns>A <see cref="Task{PlayerInputs}"/> that await given <paramref name="msDelay"/> 
  /// before returning a random <see cref="PlayerInputs"/>.
  /// Return <see cref="DEFAULT_INPUT"/> if given <paramref name="cancellationToken"/> is cancelled.
  /// Keep returning <see cref="PlayerInputs.Exit"/> once it has been already returned (simulated game exit, so no other inputs needs to be produced).
  /// </returns>
  public static async Task<PlayerInputs> GetNextInputAsync(int msDelay = DEFAULT_DELAY_MS, CancellationToken cancellationToken = default)
  {
    InputCount++;

    await Task.Delay(msDelay, cancellationToken);

    if (cancellationToken.IsCancellationRequested)
      return DEFAULT_INPUT;

    if (CurrentInput == PlayerInputs.Exit)
      return PlayerInputs.Exit;

    var input = _GetInput();

    return input;
  }

  /// <summary>
  /// Getting a random <see cref="PlayerInputs"/> weighted from 
  /// <see cref="_OrderedInputProbabilities"/> evoluting probabilities table property.
  /// </summary>
  /// <returns>A random <see cref="PlayerInputs"/> driven by 
  /// <see cref="_OrderedInputProbabilities"/> evoluting probabilities table property (or <see cref="DEFAULT_INPUT"/> if it fails)</returns>
  private static PlayerInputs _GetInput()
  {
    if (_isDirty || _orderedInputProbabilities == null)
      _UpdateProbabilities();

    var randomNumber = Random.Shared.NextDouble();

    for(int i = 0; i < _OrderedInputProbabilities.Length; i++)
    {
      if(randomNumber <= _OrderedInputProbabilities[i].Item1)
        return _OrderedInputProbabilities[i].Item2;
    }

    return DEFAULT_INPUT;
  }

  /// <summary>
  /// Update <see cref="_OrderedInputProbabilities"/> evoluting probabilities table property
  /// along <see cref="InputStep"/> and <see cref="DEFAULT_EVOLUTION_PROBABILITY"/> value.
  /// </summary>
  private static void _UpdateProbabilities()
  {
    double cumulativeProbabilities = 0d;

    if (InputStep > 0)
    {
      double probabilityEvolution = InputStep * DEFAULT_EVOLUTION_PROBABILITY;

      foreach (var input in InputsProbabilities.Keys)
      {
        InputsProbabilities[input] = Math.Clamp(input == PlayerInputs.Exit ?
          probabilityEvolution :
          InputsProbabilities[input] - InputsProbabilities[input]*DEFAULT_EVOLUTION_PROBABILITY, 0d, 1d);
      }
    }

    _orderedInputProbabilities = [.. InputsProbabilities.OrderByDescending(kvp => kvp.Value).Select(kvp => { cumulativeProbabilities += kvp.Value; return (cumulativeProbabilities, kvp.Key); })];

    _isDirty = false;
  }

  /// <summary>
  /// Utility method to return the probability the player stay alive when attempting to jump in the simulated game loop
  /// along <see cref="DEFAULT_JUMP_ALIVE_PROBABILITY"/> and given <paramref name="canDie"/> values.
  /// </summary>
  /// <param name="canDie"><see langword="bool"/> flag enforcing to return 1 (always staying alive) when <see langword="false"/></param>
  /// <returns>The probability to stay alive when the player attempt to jump in the game simulation 
  /// as <see langword="double"/> [0d - 1d] range value (0d meaning it always die, 1d never).</returns>
  public static double JumpAliveProbability(bool canDie = true)
      => canDie ? DEFAULT_JUMP_ALIVE_PROBABILITY : 1d;

  /// <summary>
  /// Utility method to return the probability the player stay alive when attempting to move in the simulated game loop
  /// along <see cref="DEFAULT_MOVE_ALIVE_PROBABILITY"/> and given <paramref name="canDie"/> values.
  /// </summary>
  /// <param name="canDie"><see langword="bool"/> flag enforcing to return 1 (always staying alive) when <see langword="false"/></param>
  /// <returns>The probability to stay alive when the player attempt to move in the game simulation 
  /// as <see langword="double"/> [0d - 1d] range value (0d meaning it always die, 1d never).</returns>
  public static double MoveAliveProbability(bool canDie = true)
    => canDie ? DEFAULT_MOVE_ALIVE_PROBABILITY : 1d;

  /// <summary>
  /// Reset to default <see cref="InputsProbabilities"/> <see cref="PlayerInputs"/> probabilities table property.
  /// </summary>
  public static void ResetInputsProbabilitiesToDefaults()
  {
    _inputsProbabilities ??= [];
    if (_inputsProbabilities.Count != 4)
    {
      _inputsProbabilities.Clear();
      _inputsProbabilities.Add(PlayerInputs.Idle, DEFAULT_IDLE_PROBABILITY);
      _inputsProbabilities.Add(PlayerInputs.Move, DEFAULT_MOVE_PROBABILITY);
      _inputsProbabilities.Add(PlayerInputs.Jump, DEFAULT_JUMP_PROBABILITY);
      _inputsProbabilities.Add(PlayerInputs.Exit, DEFAULT_EXIT_PROBABILITY);
      return;
    }

    _inputsProbabilities[PlayerInputs.Idle] = DEFAULT_IDLE_PROBABILITY;
    _inputsProbabilities[PlayerInputs.Move] = DEFAULT_MOVE_PROBABILITY;
    _inputsProbabilities[PlayerInputs.Jump] = DEFAULT_JUMP_PROBABILITY;
    _inputsProbabilities[PlayerInputs.Exit] = DEFAULT_EXIT_PROBABILITY;
  }

  /// <summary>
  /// Reset both this <see cref="PlayerStateMachineHelper"/> and <see cref="PlayerManager"/> 
  /// static classes's 'inner states' fields and properties to their default values.
  /// </summary>
  public static void ResetToDefaults()
  {
    InputCeil = DEFAULT_INPUT_CEIL;
    InputCount = 0;
    _isDirty = true;
    _inputsProbabilities?.Clear();
    _orderedInputProbabilities = null;
    ///------PlayerManager-------
    CurrentInput = DEFAULT_INPUT;
    CurrentCredit = DEFAULT_MAX_CREDIT;
    CanDie = true;
    IsAlive = true;
    IsJumping = false;
    InputChanged = false;
    WasExitAsked = false;
  }
}
  
