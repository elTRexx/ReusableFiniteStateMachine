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

using ReusableFiniteStateMachine.Sources.Factories.Interfaces;
using ReusableFiniteStateMachine.Sources.Services;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Tests.Fixture.CacheFixture;
using ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;
using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;
using ReusableFiniteStateMachine.Tests.Helpers;

namespace ReusableFiniteStateMachine.Tests.Unit;

/// <summary>
/// Test class that encapsulate all <see cref="IState"/> unit tests.
/// </summary>
public class StateUnitTests
{
  #region Initialize
  /// <summary>
  /// Unit test asserting that <see cref="IState.Initialize(IStateMachine, object?)"/> method
  /// with NULL argument should pass on all <see cref="IState"/> state fixtures returned by <see cref="GetStateFixtures"/>.
  /// </summary>
  /// <param name="state">The collection of <see cref="IState"/> to test upon its <see cref="IState.Initialize(IStateMachine, object?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [MethodDataSource(typeof(FixtureHelper), nameof(GetStateFixtures))]
  public async Task IState_Initialize_With_NullArgument_ShouldPass(IState state)
  {
    ///Arrange

    ///Act & Assert       
    await Assert.That(() => state.Initialize(null!)).ThrowsNothing();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IState.Initialize(IStateMachine, object?)"/> method
  /// called on given <see cref="FirstStateFixture"/> state fixture with a new <see cref="IStateMachine"/> instance
  /// should set it to its <see cref="IState.StateMachineOwner"/> property.
  /// </summary>
  /// <param name="stateFixture">the <see cref="FirstStateFixture"/> state fixture
  /// to test upon its <see cref="IState.Initialize(IStateMachine, object?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [ClassDataSource<FirstStateFixture>()]
  public async Task FirstStateFixture_Initialize_With_NewStateMachine_Should_SetIt_To_StateMachineOwner(FirstStateFixture stateFixture)
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act
    stateFixture.Initialize(stateMachine);

    ///Assert
    await Assert.That(stateFixture.StateMachineOwner).IsEqualTo(stateMachine);
  }
  #endregion

  #region OnEnter
  /// <summary>
  /// Unit test asserting that <see cref="IState.OnEnter(IState?)"/> method
  /// with NULL argument should pass on all <see cref="IState"/> state fixtures returned by <see cref="GetStateFixtures"/>.
  /// </summary>
  /// <param name="state">The collection of <see cref="IState"/> to test upon its <see cref="IState.OnEnter(IState?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [MethodDataSource(typeof(FixtureHelper), nameof(GetStateFixtures))]
  public async Task IState_OnEnter_With_NullArgument_ShouldPass(IState state)
  {
    ///Arrange

    ///Act & Assert      
    await Assert.That(() => state.OnEnter(null)).ThrowsNothing();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IState.OnEnter(IState?)"/> method
  /// called on given <see cref="FirstStateFixture"/> state fixture 
  /// with an arranged <see cref="IState"/> previous state object
  /// should set it to its <see cref="BaseMonitoringStateFixture.PreviousState"/> property.
  /// </summary>
  /// <param name="stateFixture">the <see cref="FirstStateFixture"/> state fixture
  /// to test upon its <see cref="IState.OnEnter(IState?)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [ClassDataSource<FirstStateFixture>()]
  public async Task FirstStateFixture_OnEnter_With_GivenPreviousState_Should_SetIt_To_PreviousState(FirstStateFixture stateFixture)
  {
    ///Arrange
    IState? expectedPreviousState = StateMachineInstance.EndState;

    ///Act
    stateFixture.OnEnter(expectedPreviousState);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(expectedPreviousState).IsNotNull();
      await Assert.That(stateFixture.PreviousState).IsEqualTo(expectedPreviousState);
    }
  }
  #endregion

  #region OnExit
  /// <summary>
  /// Unit test asserting that <see cref="IState.OnExit(IState?)"/> method
  /// with NULL argument should pass on all <see cref="IState"/> state fixtures returned by <see cref="GetStateFixtures"/>.
  /// </summary>
  /// <param name="state">The collection of <see cref="IState"/> to test upon its <see cref="IState.OnExit(IState?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [MethodDataSource(typeof(FixtureHelper), nameof(GetStateFixtures))]
  public async Task IState_OnExit_With_NullArgument_ShouldPass(IState state)
  {
    ///Arrange

    ///Act & Assert      
    await Assert.That(() => state.OnExit(null)).ThrowsNothing();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IState.OnExit(IState?)"/> method
  /// called on given <see cref="FirstStateFixture"/> state fixture 
  /// with an arranged <see cref="IState"/> next state object
  /// should set it to its <see cref="BaseMonitoringStateFixture.NextState"/> property.
  /// </summary>
  /// <param name="stateFixture">the <see cref="FirstStateFixture"/> state fixture
  /// to test upon its <see cref="IState.OnExit(IState?)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [ClassDataSource<FirstStateFixture>()]
  public async Task FirstStateFixture_OnExit_With_GivenNextState_Should_SetIt_To_NextState(FirstStateFixture stateFixture)
  {
    ///Arrange
    IState? expectedNextState = StateMachineInstance.EndState;

    ///Act
    stateFixture.OnExit(expectedNextState);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(expectedNextState).IsNotNull();
      await Assert.That(stateFixture.NextState).IsEqualTo(expectedNextState);
    }
  }
  #endregion

  /// <summary>
  /// Unit test asserting that <see cref="IState.Update(object?)"/> method
  /// with NULL argument should pass on all <see cref="IState"/> state fixtures returned by <see cref="GetStateFixtures"/>.
  /// </summary>
  /// <param name="state">The collection of <see cref="IState"/> to test upon its <see cref="IState.Update(object?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [MethodDataSource(typeof(FixtureHelper) , nameof(GetStateFixtures))]
  public async Task IState_Update_WithNullArgument_ShouldPass(IState state)
  {
    ///Arrange

    ///Act & Assert           
    await Assert.That(() => state.Update(null)).ThrowsNothing();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IState.GetNextState(object?)"/> method
  /// with NULL argument on every <see cref="IState"/> state fixtures returned by <see cref="GetStateFixtures"/>
  /// and owned by an <see cref="InitializedStateMachineFixture"/> initialized state machine fixture
  /// should return its expected next <see cref="IState"/>.
  /// [<see cref="InitializedStateMachineFixture.StartState"/>] -> [<see cref="StateMachineFixture.FirstState"/>] -> [<see cref="IStateMachine.EndState"/>]
  /// </summary>
  /// <param name="state">The collection of <see cref="IState"/> to test upon its <see cref="IState.GetNextState(object?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [MethodDataSource(typeof(FixtureHelper), nameof(GetStateFixtures))]
  public async Task IState_GetNextState_With_NullArgument_Should_Return_ExpectedNextState(IState state)
  {
    ///Arrange
    var stateMachine = state?.StateMachineOwner as StateMachineFixture;

    var expectedNextState = true switch
    {
      _ when stateMachine == default => null,
      _ when state == stateMachine.StartState => stateMachine.FirstState,
      _ when state == stateMachine.FirstState => stateMachine.EndState,
      _ when state == stateMachine.EndState => stateMachine.EndState,
      _ => null
    };

    ///Act
    var nextState = state?.GetNextState();

    ///Assert      
    await Assert.That(nextState).IsEqualTo(expectedNextState);
  }

  #region Dispose
  /// <summary>
  /// Unit test asserting that calling <see cref="BaseState.Dispose()"/> method
  /// should clear (set its value to <see langword="null"/>) its <see cref="BaseState.StateMachineOwner"/> property.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [NotInParallel]/// Test on shared <see cref="StateCacheFixture.CleanCacheOfStateCallCount"/> property.
  public async Task IState_Dispose_Should_Call_CleanCache()
  {
    ///Arrange
    var firstState = StateMachineInstance.GetOrAddState<FirstStateFixture>();
    var stateCache = Builder.Services.Cache as StateCacheFixture;
    uint expectedCleanCacheCallCount = stateCache?.CleanCacheOfStateCallCount + 1 ?? uint.MaxValue;

    ///Act
    firstState?.Dispose();

    ///Assert
    await Assert.That(stateCache?.CleanCacheOfStateCallCount ?? uint.MinValue).IsEqualTo(expectedCleanCacheCallCount);
  }

  /// <summary>
  /// Unit test asserting that <see cref="BaseState.Dispose()"/> method
  /// should call its <see cref="BaseState.Dispose(bool)"/> overload method
  /// with <see langword="true"/> for its argument.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IState_Dispose_Should_CallDisposeOverload_With_True_Argument()
  {
    ///Arrange
    var firstState = StateMachineInstance.GetOrAddState<FirstStateFixture>();

    ///Act
    firstState?.Dispose();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(firstState?.DisposeCallCount).IsEqualTo(1u);
      await Assert.That(firstState?.Disposing).IsTrue();
    }
  }

  /// <summary>
  /// Unit test asserting that calling <see cref="BaseState.Dispose()"/> method
  /// multiple times in a row should call the <see cref="IStateFactory.CleanCache(IState)"/> method only once.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [NotInParallel]/// Test on shared <see cref="StateCacheFixture.CleanCacheOfStateCallCount"/> property.
  public async Task IState_Dispose_MultipleCall_Should_Call_CleanCache_OnlyOnce()
  {
    ///Arrange
    var firstState = StateMachineInstance.GetOrAddState<FirstStateFixture>();
    int randomDisposeCallAmount = Random.Shared.Next(2, 10);
    int currentCallCount = 0;
    var stateFactory = Builder.Services.Cache as StateCacheFixture;
    uint expectedCleanCacheCallCount = stateFactory?.CleanCacheOfStateCallCount + 1 ?? uint.MaxValue;

    ///Act
    while (currentCallCount++ < randomDisposeCallAmount)
      firstState?.Dispose();

    ///Assert
    await Assert.That(stateFactory?.CleanCacheOfStateCallCount ?? uint.MinValue).IsEqualTo(expectedCleanCacheCallCount);
  }
  #endregion
} 