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
using ReusableFiniteStateMachine.Sources.StateMachine;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Tests.Fixture.CacheFixture;
using ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;
using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;
using TUnit.Mocks.Assertions;

namespace ReusableFiniteStateMachine.Tests.Unit;

/// <summary>
/// Test class that encapsulate all <see cref="IStateMachine"/> unit tests.
/// </summary>
public class StateMachineUnitTests
{
  #region GetNextState
  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.GetNextState(object?)"/> method
  /// called on a non-initialized <see cref="IStateMachine"/> state machine 
  /// should return <see cref="IStateMachine.EndState"/>.
  /// </summary>
  ///// <param name="stateMachine">The non-initialized <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.GetNextState(object?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_GetNextState_NotInitialized_Should_Return_EndState()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act
    var nextState = stateMachine.GetNextState();

    ///Asset
    await Assert.That(nextState).IsEqualTo(stateMachine.EndState);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.GetNextState(object?)"/> method
  /// called on a initialized <see cref="InitializedStateMachineFixture"/> state machine 
  /// should return <see cref="StateMachineFixture.FirstState"/>.
  /// </summary>
  /// <param name="stateMachine">The initialized <see cref="InitializedStateMachineFixture"/> state machine fixture 
  /// to test upon its <see cref="IStateMachine.GetNextState(object?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [ClassDataSource<InitializedStateMachineFixture>]
  public async Task IStateMachine_GetNextState_Initialized_Should_Return_Its_FirstState(InitializedStateMachineFixture stateMachine)
  {
    ///Arrange
    var expectedNextState = stateMachine.FirstState;

    ///Act
    var nextState = stateMachine.GetNextState();

    ///Asset
    await Assert.That(nextState).IsEqualTo(expectedNextState);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.GetNextState(object?)"/> method
  /// called on <see cref="PlayerStateMachineFixture"/> player state machine 
  /// should fall back to call its <see cref="IStateMachine.GetNextStateCentralized"/> delegate.
  /// </summary>
  /// <param name="playerStateMachine">The <see cref="PlayerStateMachineFixture"/> player state machine fixture to test upon its <see cref="IStateMachine.GetNextState(object?)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [ClassDataSource<PlayerStateMachineFixture>]
  public async Task IStateMachine_GetNextState_With_Null_NextState_Of_CurrentState_Should_FallBack_To_GetNextStateCentralized(PlayerStateMachineFixture playerStateMachine)
  {
    ///Arrange
    var mockedGetNextStateCentralized = Mock.OfDelegate<Func<object?, IState?>>();
    mockedGetNextStateCentralized!.Invoke(Any()).Returns(playerStateMachine.EndState);

    playerStateMachine.GetNextStateCentralized = mockedGetNextStateCentralized;

    ///Simulate exiting StartState that itself have a decentralized GetNextState()
    playerStateMachine.CurrentState = playerStateMachine.GetNextState();

    ///Act
    var nextState = playerStateMachine.GetNextState();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(mockedGetNextStateCentralized!.Invoke(Any())).WasCalled();
      await Assert.That(nextState).IsEqualTo(playerStateMachine.EndState);
    }
  }
  #endregion

  #region InitializeStateMachine
  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.InitializeStateMachine(IState, bool, object?)"/> method
  /// called on a non-initialized <see cref="IStateMachine"/> state machine with a NULL first state argument 
  /// should keep having its <see cref="IStateMachine.StartState"/> being NULL.
  /// </summary>
  ///// <param name="stateMachine">The non-initialized <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.InitializeStateMachine(IState, bool, object?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_InitializeStateMachine_With_Null_FirstState_Should_Keep_StartState_Null()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act
    stateMachine.InitializeStateMachine(null!);

    ///Assert
    await Assert.That(stateMachine.StartState).IsNull();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.InitializeStateMachine(IState, bool, object?)"/> method
  /// called on a non-initialized <see cref="StateMachineFixture"/> state machine with 
  /// <see cref="StateMachineFixture.FirstState"/> as its first state argument
  /// should have its <see cref="IStateMachine.StartState"/> NOT being NULL.
  /// </summary>
  ///// <param name="stateMachine">The non-initialized <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.InitializeStateMachine(IState, bool, object?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_InitializeStateMachine_With_FirstState_Should_Have_NonNull_StartState()
  {
    ///Arrange
    var stateMachine = StateMachineInstance as StateMachineFixture;

    ///Act
    stateMachine?.InitializeStateMachine(stateMachine?.FirstState);

    ///Assert
    await Assert.That(stateMachine?.StartState).IsNotNull();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.InitializeStateMachine(IState, bool, object?)"/> method
  /// called on a mocked <see cref="StateMachine"/> state machine with 
  /// <see cref="StateMachineFixture.FirstState"/> as its first state argument
  /// and having its autorun flagged to <see langword="true"/>
  /// should call <see cref="StateMachine.AutorunStateMachine(object?)"/> method.
  /// </summary>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_InitializeStateMachine_With_FirstState_And_Autorun_Should_Call_AutorunStateMachine()
  {
    ///Arrange
    var mockedStateMachine = StateMachineFixture.Mock();

    mockedStateMachine.AutorunStateMachine(Any());

    var stateMachine = mockedStateMachine.Object;

    var firstState = stateMachine.GetOrAddState<FirstStateFixture>();

    ///Act
    stateMachine.InitializeStateMachine(firstState, true);

    ///Assert
    await Assert.That(mockedStateMachine.AutorunStateMachine(Any())).WasCalled(Times.Once);
  }
  #endregion

  #region AutorunStateMachine
  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.AutorunStateMachine(object?)"/> method
  /// called on a mocked non-initialized <see cref="StateMachine"/> state machine   
  /// should not call <see cref="StateMachine.Update(object?)"/> method.
  /// </summary>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_AutorunStateMachine_NotInitialized_ShouldNot_Call_Update()
  {
    ///Arrange
    var mockedStateMachine = StateMachineFixture.Mock();

    mockedStateMachine.Update(Any());

    var stateMachine = mockedStateMachine.Object;

    ///Act
    stateMachine.AutorunStateMachine();

    ///Assert
    await Assert.That(mockedStateMachine.Update(Any())).WasNeverCalled();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.AutorunStateMachine(object?)"/> method
  /// called on a mocked initialized <see cref="StateMachine"/> state machine   
  /// should call <see cref="StateMachine.Update(object?)"/> method.
  /// </summary>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_AutorunStateMachine_Initialized_ShouldCall_Update()
  {
    ///Arrange
    var mockedStateMachine = StateMachineFixture.Mock();

    var stateMachine = mockedStateMachine.Object;

    mockedStateMachine.Update(Any()).Callback(_ => stateMachine.CurrentState = stateMachine.EndState);

    var firstState = stateMachine.GetOrAddState<FirstStateFixture>();

    stateMachine.InitializeStateMachine(firstState);

    ///Act
    stateMachine.AutorunStateMachine();

    ///let time for Update() to be called on in its own thread (force thread commutation).
    /// ! Maybe not 100% reproducible !
    await Task.Delay(10);

    ///Assert
    await Assert.That(mockedStateMachine.Update(Any())).WasCalled();
  }
  #endregion

  #region Update
  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.Update(object?)"/> method
  /// called on a mocked non-initialized <see cref="StateMachine"/> state machine   
  /// should not call <see cref="StateMachine.GetNextState(object?)"/> method.
  /// </summary>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_Update_NotInitialized_ShouldNot_Call_GetNextState()
  {
    ///Arrange
    var mockedStateMachine = StateMachineFixture.Mock();

    var stateMachine = mockedStateMachine.Object;

    ///Act
    stateMachine.Update();

    ///Assert
    await Assert.That(mockedStateMachine.GetNextState(Any())).WasNeverCalled();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.Update(object?)"/> method
  /// called on a mocked initialized <see cref="StateMachine"/> state machine   
  /// should call <see cref="StateMachine.GetNextState(object?)"/> method.
  /// </summary>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_Update_Initialized_Should_Call_GetNextState()
  {
    ///Arrange
    var mockedStateMachine = StateMachineFixture.Mock();

    var stateMachine = mockedStateMachine.Object;

    var firstState = stateMachine.GetOrAddState<FirstStateFixture>();

    stateMachine.InitializeStateMachine(firstState);

    ///Act
    stateMachine.Update();

    ///Assert
    await Assert.That(mockedStateMachine.GetNextState(Any())).WasCalled();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.Update(object?)"/> method
  /// called on a mocked initialized <see cref="StateMachine"/> state machine 
  /// that return a NULL next state should call <see cref="StateMachine.OnTransitionFailed(EventArgs)"/> method.
  /// </summary>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_Update_Initialized_With_Null_NextState_Should_Call_OnTransitionFailed()
  {
    ///Arrange
    var mockedStateMachine = StateMachineFixture.Mock();

    mockedStateMachine.GetNextState(Any()).Returns(default(IState));

    var stateMachine = mockedStateMachine.Object;

    var firstState = stateMachine.GetOrAddState<FirstStateFixture>();

    stateMachine.InitializeStateMachine(firstState);

    ///Act
    stateMachine.Update();

    ///Assert
    await Assert.That(mockedStateMachine.OnTransitionFailed(Any())).WasCalled();
  }
  #endregion

  #region GetOrAddState  
  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.GetOrAddState{T_State}(object?)"/> method
  /// on a not already owned <see cref="{T_State}"/> state
  /// should return a new <see cref="{T_STate}"/> instance state.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// to test upon its <see cref="IStateMachine.GetOrAddState{T_State}(object?)"/> method.</param>
  /// <returns>The awaitable test assertions.</returns>
  [Test]
  public async Task IStateMachine_GetOrAddState_On_A_Not_Cached_StateType_Should_Return_AnInstance_Of_ExpectedType()
  {
    ///Arrange

    ///Act
    var newState = StateMachineInstance.GetOrAddState<StateAFixture>();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(newState).IsNotNull();
      await Assert.That(newState).IsTypeOf<StateAFixture>();
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.GetOrAddState{T_State}(object?)"/> method
  /// on an already owned <see cref="{T_State}"/> state
  /// should return a <see cref="{T_STate}"/> instance state.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// to test upon its <see cref="IStateMachine.GetOrAddState{T_State}(object?)"/> method.</param>
  /// <returns>The awaitable test assertions.</returns>
  [Test]
  public async Task IStateMachine_GetOrAddState_On_A_Cached_StateType_Should_Return_AnInstance_Of_ExpectedType()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    /// Add and cache a new StateAFixture to given state machine first.
    stateMachine.GetOrAddState<StateAFixture>();

    ///Act
    var cachedState = stateMachine.GetOrAddState<StateAFixture>();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(cachedState).IsNotNull();
      await Assert.That(cachedState).IsTypeOf<StateAFixture>();
    }
  }
  #endregion

  #region Dispose
  /// <summary>
  /// Unit test asserting that <see cref="StateMachine.Dispose"/> method
  /// called on a mocked <see cref="StateMachine"/> state machine
  /// should call <see cref="StateMachine.ResetStateMachine(bool)"/> method.
  /// </summary>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_Dispose_Should_Call_ResetStateMachine()
  {
    ///Arrange
    var mockedStateMachine = StateMachineFixture.Mock();

    var stateMachine = mockedStateMachine.Object;

    ///Act
    stateMachine.Dispose();

    ///Assert
    await Assert.That(mockedStateMachine.ResetStateMachine(false)).WasCalled();
  }

  /// <summary>
  /// Unit test asserting that <see cref="StateMachine.Dispose"/> method  
  /// should call <see cref="IStateFactory.CleanCache(IStateMachine)"/> method.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateFactory.CleanCache(IStateMachine)"/> method.</param>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_Dispose_Should_Call_CleanCache()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    var stateCache = Builder.Services.Cache as StateCacheFixture;

    uint expectedCleanCacheCallCount = stateCache?.CleanCacheOfStateMachineCallCount + 1 ?? uint.MaxValue;

    ///Act
    stateMachine.Dispose();

    ///Assert
    await Assert.That(stateCache?.CleanCacheOfStateMachineCallCount ?? uint.MinValue).IsEqualTo(expectedCleanCacheCallCount);
  }
  #endregion

  #region Events
  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.RaiseStateMachineStarted(EventArgs?)"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should raise its <see cref="IStateMachine.MachineStarted"/> event.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.RaiseStateMachineStarted(EventArgs?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_RaiseStateMachineStarted_Should_Raise_MachineStartedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;
    bool isStarted = false;

    stateMachine.MachineStarted += (_, _) => isStarted = true;

    ///Act
    stateMachine.RaiseStateMachineStarted();

    ///Assert
    await Assert.That(isStarted).IsTrue();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.RaiseStateMachineEnded(EventArgs?)"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should raise its <see cref="IStateMachine.MachineEnded"/> event.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.RaiseStateMachineEnded(EventArgs?)"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_RaiseStateMachineEnded_Should_Raise_MachineEndedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;
    bool isEnded = false;

    stateMachine.MachineEnded += (_, _) => isEnded = true;

    ///Act
    stateMachine.RaiseStateMachineEnded();

    ///Assert
    await Assert.That(isEnded).IsTrue();
  }

  /// <summary>
  /// Private utility method used by other unit test methods that provide shared reusable code
  /// for testing <see cref="IStateMachine"/> event flushing methods.
  /// </summary>
  /// <param name="flushDelegate">The actual event flushing method, as an <see cref="Action"/> delegate, to be tested.</param>
  /// <param name="raiseDelegate">The event raising method, as an <see cref="Action{EventArgs}"/> delegate, used for the test.</param>
  /// <param name="eventHandlerDelegate">The <see cref="Action{EventHandler{EventArgs}}"/> delegate that wraps the event handler that should be raised or not for the test.</param>
  /// <param name="expectedRaisingEvent"><see cref="bool"/> flag indicating wether the test expect the event to be raised or not.</param>
  /// <returns>The awaitable (by the real test method) test assertion.</returns>
  private static Task<bool> IStateMachine_FlushEvents_Tests(
    Action flushDelegate, 
    Action<EventArgs?> raiseDelegate, 
    Action<EventHandler<EventArgs?>> eventHandlerDelegate, 
    bool expectedRaisingEvent)
  {
    ///Arrange
    bool isRaised = false;
    eventHandlerDelegate((_, _) => isRaised = true);

    ///Act
    flushDelegate();
    raiseDelegate(default);

    ///Assert
#pragma warning disable TUnitAssertions0002 // Assert statement not awaited
    return Assert.That(isRaised).IsEqualTo(expectedRaisingEvent).AssertAsync();
#pragma warning restore TUnitAssertions0002 // Assert statement not awaited
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushMachineStarted"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should prevent any <see cref="IStateMachine.MachineStarted"/> associated event handler to be raised.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushMachineStarted"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushMachineStarted_Then_Calling_RaiseStateMachineStarted_WithSettedMachineStartedEvent_Should_NotRaise_MachineStartedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_Tests(stateMachine.FlushMachineStarted, stateMachine.RaiseStateMachineStarted, _ => stateMachine.MachineStarted += _, false); 
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushMachineEnded"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should still raise any <see cref="IStateMachine.MachineStarted"/> associated event handler.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushMachineEnded"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushMachineEnded_Then_Calling_RaiseStateMachineStarted_WithSettedMachineStartedEvent_Should_Raise_MachineStartedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_Tests(stateMachine.FlushMachineEnded, stateMachine.RaiseStateMachineStarted, _ => stateMachine.MachineStarted += _, true);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushTransitionFailed"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should still raise any <see cref="IStateMachine.MachineStarted"/> associated event handler.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushTransitionFailed"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushTransitionFailed_Then_Calling_RaiseStateMachineStarted_WithSettedMachineStartedEvent_Should_Raise_MachineStartedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_Tests(stateMachine.FlushTransitionFailed, stateMachine.RaiseStateMachineStarted, _ => stateMachine.MachineStarted += _, true);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushCurrentStateChanged"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should still raise any <see cref="IStateMachine.MachineStarted"/> associated event handler.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushCurrentStateChanged"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushCurrentStateChanged_Then_Calling_RaiseStateMachineStarted_WithSettedMachineStartedEvent_Should_Raise_MachineStartedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_Tests(stateMachine.FlushCurrentStateChanged, stateMachine.RaiseStateMachineStarted, _ => stateMachine.MachineStarted += _, true);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushMachineEnded"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should prevent any <see cref="IStateMachine.MachineEnded"/> associated event handler to be raised.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushMachineEnded"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushMachineEnded_Then_Calling_RaiseStateMachineEnded_WithSettedMachineEndedEvent_Should_NotRaise_MachineEndedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_Tests(stateMachine.FlushMachineEnded, stateMachine.RaiseStateMachineEnded, _ => stateMachine.MachineEnded += _, false);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushMachineStarted"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should still raise any <see cref="IStateMachine.MachineEnded"/> associated event handler.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushMachineStarted"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushMachineStarted_Then_Calling_RaiseStateMachineEnded_WithSettedMachineEndedEvent_Should_NotRaise_MachineEndedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_Tests(stateMachine.FlushMachineStarted, stateMachine.RaiseStateMachineEnded, _ => stateMachine.MachineEnded += _, true);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushTransitionFailed"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should still raise any <see cref="IStateMachine.MachineEnded"/> associated event handler.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushTransitionFailed"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushTransitionFailed_Then_Calling_RaiseStateMachineEnded_WithSettedMachineEndedEvent_Should_NotRaise_MachineEndedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_Tests(stateMachine.FlushTransitionFailed, stateMachine.RaiseStateMachineEnded, _ => stateMachine.MachineEnded += _, true);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushCurrentStateChanged"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture
  /// should still raise any <see cref="IStateMachine.MachineEnded"/> associated event handler.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushCurrentStateChanged"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushCurrentStateChanged_Then_Calling_RaiseStateMachineEnded_WithSettedMachineEndedEvent_Should_NotRaise_MachineEndedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_Tests(stateMachine.FlushCurrentStateChanged, stateMachine.RaiseStateMachineEnded, _ => stateMachine.MachineEnded += _, true);
  }

  /// <summary>
  /// Private utility method used by other unit test methods that provide shared reusable code
  /// for testing <see cref="IStateMachine"/> event flushing methods 
  /// on a mocked <see cref="StateMachine"/> state machine fixture
  /// where its <see cref="IStateMachine.TransitionFailed"/> event ending being is raised.
  /// </summary>
  /// <param name="callingFlushDelegate">The <see cref="Action{StateManager}"/> delegate that wraps the call to the desired flushing method.</param>
  /// <param name="expectedRaisingEvent"><see cref="bool"/> flag indicating wether the test expect 
  /// the <see cref="IStateMachine.TransitionFailed"/> event to be raised or not.</param>
  /// <returns>The awaitable (by the real test method) test assertion.</returns>
  private static Task<bool> IStateMachine_FlushEvents_TransitionFailedEvent_Tests(
    Action<StateMachine> callingFlushDelegate, 
    bool expectedRaisingEvent)
  {
    ///Arrange    
    var mockedStateMachine = StateMachineFixture.Mock();

    ///Assure that TransitionFailed event is raised on Update()
    mockedStateMachine.GetNextState(Any()).Returns(default(IState));

    var stateMachine = mockedStateMachine.Object;

    var firstState = stateMachine.GetOrAddState<FirstStateFixture>();

    stateMachine.InitializeStateMachine(firstState);

    bool transitionFailedRaised = false;

    stateMachine.TransitionFailed += (_, _) => transitionFailedRaised = true;

    ///Act
    callingFlushDelegate(stateMachine);
    stateMachine.Update();

    ///Assert
#pragma warning disable TUnitAssertions0002 // Assert statement not awaited
    return Assert.That(transitionFailedRaised).IsEqualTo(expectedRaisingEvent).AssertAsync();
#pragma warning restore TUnitAssertions0002 // Assert statement not awaited
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushTransitionFailed"/> method
  /// called on a mocked initialized <see cref="StateMachine"/> state machine that <see cref="IStateMachine.GetNextState(object?)"/>
  /// return a NULL next state should NOT raise its <see cref="IStateMachine.TransitionFailed"/> event.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushTransitionFailed_Initialized_Calling_Update_WithNullNextState_Should_NotRaise_TransitionFailedEvent()
    => await IStateMachine_FlushEvents_TransitionFailedEvent_Tests(_ => _.FlushTransitionFailed(), false);

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushMachineStarted"/> method
  /// called on a mocked initialized <see cref="StateMachine"/> state machine that <see cref="IStateMachine.GetNextState(object?)"/> 
  /// return a NULL next state should raise its <see cref="IStateMachine.TransitionFailed"/> event.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushMachineStarted_Initialized_Calling_Update_WithNullNextState_Should_Raise_TransitionFailedEvent()
    => await IStateMachine_FlushEvents_TransitionFailedEvent_Tests(_ => _.FlushMachineStarted(), true);

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushMachineEnded"/> method
  /// called on a mocked initialized <see cref="StateMachine"/> state machine that <see cref="IStateMachine.GetNextState(object?)"/> 
  /// return a NULL next state should raise its <see cref="IStateMachine.TransitionFailed"/> event.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushMachineEnded_Initialized_Calling_Update_WithNullNextState_Should_Raise_TransitionFailedEvent()
    => await IStateMachine_FlushEvents_TransitionFailedEvent_Tests(_ => _.FlushMachineEnded(), true);

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushCurrentStateChanged"/> method
  /// called on a mocked initialized <see cref="StateMachine"/> state machine that <see cref="IStateMachine.GetNextState(object?)"/> 
  /// return a NULL next state should raise its <see cref="IStateMachine.TransitionFailed"/> event.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushCurrentStateChanged_Initialized_Calling_Update_WithNullNextState_Should_Raise_TransitionFailedEvent()
    => await IStateMachine_FlushEvents_TransitionFailedEvent_Tests(_ => _.FlushCurrentStateChanged(), true);

  /// <summary>
  /// Private utility method used by other unit test methods that provide shared reusable code
  /// for testing <see cref="IStateMachine"/> event flushing methods 
  /// on a given <see cref="StateMachineFixture"/> state machine where 
  /// its <see cref="IStateMachine.CurrentStateChanged"/> event ending being is raised.
  /// </summary>
  /// <param name="callingFlushDelegate">The <see cref="Action{StateManager}"/> delegate that wraps the call to the desired flushing method.</param>
  /// <param name="expectedRaisingEvent"><see cref="bool"/> flag indicating wether the test expect 
  /// the <see cref="IStateMachine.CurrentStateChanged"/> event to be raised or not.</param>
  /// <returns>The awaitable (by the real test method) test assertion.</returns>
  private static Task<bool> IStateMachine_FlushEvents_CurrentStateEvent_Tests(
    IStateMachine stateMachine,
    Action<IStateMachine> callingFlushDelegate,
    bool expectedRaisingEvent)
  {
    ///Arrange
    bool currentStateChanged = false;

    stateMachine.CurrentStateChanged += (_, _) => currentStateChanged = true;
    
    ///Act
    callingFlushDelegate(stateMachine);

    ///Assure that CurrentStateChanged is raised
    stateMachine.CurrentState = default;
    stateMachine.CurrentState = stateMachine.StartState;
                                                        
    ///Assert
#pragma warning disable TUnitAssertions0002 // Assert statement not awaited
    return Assert.That(currentStateChanged).IsEqualTo(expectedRaisingEvent).AssertAsync();
#pragma warning restore TUnitAssertions0002 // Assert statement not awaited
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushCurrentStateChanged"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture 
  /// and changing its <see cref="IStateMachine.CurrentState"/>
  /// should NOT raise its <see cref="IStateMachine.CurrentStateChanged"/> event.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushCurrentStateChanged"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushCurrentStateChanged_Changing_CurrentState_Should_NotRaise_CurrentStateChangedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_CurrentStateEvent_Tests(stateMachine, _ => _.FlushCurrentStateChanged(), false);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushMachineStarted"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture 
  /// and changing its <see cref="IStateMachine.CurrentState"/>
  /// should raise its <see cref="IStateMachine.CurrentStateChanged"/> event.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushMachineStarted"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushMachineStarted_Changing_CurrentState_Should_Raise_CurrentStateChangedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_CurrentStateEvent_Tests(stateMachine, _ => _.FlushMachineStarted(), true);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushMachineEnded"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture 
  /// and changing its <see cref="IStateMachine.CurrentState"/>
  /// should raise its <see cref="IStateMachine.CurrentStateChanged"/> event.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushMachineEnded"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushMachineEnded_Changing_CurrentState_Should_Raise_CurrentStateChangedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_CurrentStateEvent_Tests(stateMachine, _ => _.FlushMachineEnded(), true);
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateMachine.FlushTransitionFailed"/> method
  /// called on a <see cref="StateMachineFixture"/> state machine fixture 
  /// and changing its <see cref="IStateMachine.CurrentState"/>
  /// should raise its <see cref="IStateMachine.CurrentStateChanged"/> event.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.FlushTransitionFailed"/> method.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_FlushTransitionFailed_Changing_CurrentState_Should_Raise_CurrentStateChangedEvent()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act & Assert
    await IStateMachine_FlushEvents_CurrentStateEvent_Tests(stateMachine, _ => _.FlushTransitionFailed(), true);
  }
  #endregion
}
