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

using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;
using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;
using TUnit.Assertions.Enums;

namespace ReusableFiniteStateMachine.Tests.Fonctional;

/// <summary>
/// Test class that encapsulate all fonctionnal tests.
/// </summary>
public class StateMachineFonctionalTests
{
  /// <summary>
  /// Fonctional test to assert that given <see cref="InitializedStateMachineFixture"/> state machine,
  /// that is initialized and running, should follow its expected <see cref="IState"/> flow
  /// [<see cref="IStateMachine.StartState"/>] -> [<see cref="StateMachineFixture.FirstState"/>] -> [<see cref="IStateMachine.EndState"/>]
  /// </summary>
  /// <param name="stateMachine">The <see cref="InitializedStateMachineFixture"/> state machine fixture to test upon.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [ClassDataSource<InitializedStateMachineFixture>]
  public async Task IStateMachine_Initialized_And_RunningIt_ShouldHave_CurrentState_Go_From_StartState_To_FirstState_To_EndState(InitializedStateMachineFixture stateMachine)
  {
    ///Arrange
    Queue<IState> currentStates = [];

    IState[] expectedStates =
      [stateMachine.StartState,
      stateMachine.FirstState,
      stateMachine.EndState!///???
      ];

    ///Act
    int i = 0;

    while (true
      && i < 10 ///While loop safety escape mechanism
      )
    {
      i++;

      currentStates.Enqueue(stateMachine.CurrentState);

      if (stateMachine.IsEnded)
        break;

      stateMachine.Update();
    }

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(i).IsLessThan(10);

      await Assert.That(currentStates.ElementAt(0)).IsEqualTo(expectedStates[0]);
      await Assert.That(currentStates.ElementAt(1)).IsEqualTo(expectedStates[1]);
      await Assert.That(currentStates.ElementAt(2)).IsEqualTo(expectedStates[2]);

      await Assert.That(currentStates).IsEquivalentTo(expectedStates, CollectionOrdering.Matching);
    }
  }

  /// <summary>
  /// Fonctional test to assert that given <see cref="PlayerStateMachineFixture"/> state machine should
  /// follow its expected <see cref="IState"/> flow when updating with a predetermined set of <see cref="PlayerInputs"/>.
  /// [<see cref="IdleStateFixture"/>] -> [<see cref="MoveStateFixture"/>] -> [<see cref="JumpStateFixture"/>] -> [<see cref="IdleStateFixture"/>] -> [<see cref="ExitStateFixture"/>]
  /// </summary>
  /// <param name="playerStateMachine">The <see cref="PlayerStateMachineFixture"/> state machine fixture to test upon.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [ClassDataSource<PlayerStateMachineFixture>]
  public async Task PlayerStateMachine_UpdatingIt_With_A_Set_Of_Determined_OrderedInputs_Should_Follow_Its_StatesFlow(PlayerStateMachineFixture playerStateMachine)
  {
    ///Arrange
    PlayerInputs[] orderedInputs = [PlayerInputs.Move, PlayerInputs.Jump, PlayerInputs.Idle, PlayerInputs.Exit];
    PlayerStates[] expectedStatesFlow = [PlayerStates.Idle, PlayerStates.Move, PlayerStates.Jump, PlayerStates.Idle, PlayerStates.Exit];
    Queue<PlayerStates> resultingStatesFlow = [];
    CanDie = false;

    ///Act
    ///Getting out of StartState first
    playerStateMachine.Update();

    resultingStatesFlow.Enqueue(playerStateMachine.CurrentState?.GetPlayerStateEnum() ?? PlayerStates.None);

    for (int i = 0; i < orderedInputs.Length; i++)
    {
      CurrentInput = orderedInputs[i];
      IsJumping = false;///Artificially end jumping
      playerStateMachine.Update();
      IsJumping = false;///Artificially end jumping
      resultingStatesFlow.Enqueue(playerStateMachine.CurrentState?.GetPlayerStateEnum() ?? PlayerStates.None);
    }

    ///Assert
    await Assert.That(resultingStatesFlow.Count).IsEqualTo(expectedStatesFlow.Length);

    using (Assert.Multiple())
    {
      await Assert.That(resultingStatesFlow.ElementAt(0)).IsEqualTo(expectedStatesFlow[0]);
      await Assert.That(resultingStatesFlow.ElementAt(1)).IsEqualTo(expectedStatesFlow[1]);
      await Assert.That(resultingStatesFlow.ElementAt(2)).IsEqualTo(expectedStatesFlow[2]);
      await Assert.That(resultingStatesFlow.ElementAt(3)).IsEqualTo(expectedStatesFlow[3]);
      await Assert.That(resultingStatesFlow.ElementAt(4)).IsEqualTo(expectedStatesFlow[4]);

      await Assert.That(resultingStatesFlow).IsEquivalentTo(expectedStatesFlow, CollectionOrdering.Matching);
    }
  }

  /// <summary>
  /// Fonctional test to assert that given <see cref="InitializedOrderedStateMachineFixture"/> state machine fixture should
  /// have its <see cref="State1Fixture"/>, <see cref="State2Fixture"/> and <see cref="State3Fixture"/> to call
  /// their respective <see cref="IState.OnEnter(IState?)"/> and <see cref="IState.OnExit(IState?)"/> methods
  /// in the expected state flow order while updating its <see cref="IStateMachine.CurrentState"/> property value.
  /// </summary>
  /// <param name="stateMachineFixture">The <see cref="InitializedOrderedStateMachineFixture"/> state machine fixture to test upon.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [ClassDataSource<InitializedOrderedStateMachineFixture>()]
  public async Task IStateMachine_Initialized_And_RunningIt_Should_Have_Its_CurrentState_Changing_Call_OnEnter_And_OnExit_With_CorectPreviousAndNextState(InitializedOrderedStateMachineFixture stateMachineFixture)
  {
    ///Arrange
    IState[] expectedOrderedPreviousStates = [stateMachineFixture.StartState, stateMachineFixture.FirstState, stateMachineFixture.MiddleState];
    IState[] expectedOrderedNextStates = [stateMachineFixture.MiddleState, stateMachineFixture.LastState, stateMachineFixture.EndState];
    Queue<IState> previousStates = [];
    Queue<IState> nextStates = [];
    BaseMonitoringStateFixture? previousBaseMonitoringStateFixture = default;

    ///Act
    int i = 0;

    while (true
      && i < 10 ///While loop safety escape mechanism
      )
    {
      if (stateMachineFixture.IsEnded)
      {
        if (previousBaseMonitoringStateFixture?.NextState != default)
          nextStates.Enqueue(previousBaseMonitoringStateFixture.NextState);
        break;
      }

      if (stateMachineFixture.CurrentState is BaseMonitoringStateFixture baseMonitoringStateFixture)
      {
        if (baseMonitoringStateFixture.PreviousState != default)
          previousStates.Enqueue(baseMonitoringStateFixture.PreviousState);
        if (previousBaseMonitoringStateFixture?.NextState != default)
          nextStates.Enqueue(previousBaseMonitoringStateFixture.NextState);

        previousBaseMonitoringStateFixture = baseMonitoringStateFixture;
      }

      stateMachineFixture.Update();
      
      i++;
    }

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(i).IsLessThan(10);

      await Assert.That(previousStates.Count > 2).IsTrue();

      await Assert.That(previousStates.ElementAt(0)).IsEqualTo(expectedOrderedPreviousStates[0]);
      await Assert.That(previousStates.ElementAt(1)).IsEqualTo(expectedOrderedPreviousStates[1]);
      await Assert.That(previousStates.ElementAt(2)).IsEqualTo(expectedOrderedPreviousStates[2]);

      await Assert.That(nextStates.Count > 2).IsTrue();

      await Assert.That(nextStates.ElementAt(0)).IsEqualTo(expectedOrderedNextStates[0]);
      await Assert.That(nextStates.ElementAt(1)).IsEqualTo(expectedOrderedNextStates[1]);
      await Assert.That(nextStates.ElementAt(2)).IsEqualTo(expectedOrderedNextStates[2]);

      await Assert.That(previousStates).IsEquivalentTo(expectedOrderedPreviousStates, CollectionOrdering.Matching);
      
      await Assert.That(nextStates).IsEquivalentTo(expectedOrderedNextStates, CollectionOrdering.Matching);
    }
  }
}
