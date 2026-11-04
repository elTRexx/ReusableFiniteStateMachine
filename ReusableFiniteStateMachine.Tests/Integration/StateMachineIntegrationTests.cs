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

using ReusableFiniteStateMachine.Sources.Cache.Interface;
using ReusableFiniteStateMachine.Sources.Factories.Interfaces;
using ReusableFiniteStateMachine.Sources.Services;
using ReusableFiniteStateMachine.Sources.StateMachine;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;
using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;

namespace ReusableFiniteStateMachine.Tests.Integration;

/// <summary>
/// Test class that encapsulate all <see cref="IStateMachine"/> integration tests.
/// </summary>
public class StateMachineIntegrationTests
{
  #region Events
  /// <summary>
  /// Integration test asserting that given initialized <see cref="InitializedStateMachineFixture"/> state machine fixture
  /// that its <see cref="IStateMachine.CurrentState"/> is set to <see cref="IStateMachine.StartState"/>
  /// should NOT raise its <see cref="IStateMachine.MachineStarted"/> event.
  /// (An initialized state machine is already in its <see cref="IStateMachine.StartState"/>)
  /// </summary>
  /// <param name="stateMachine">The initialized <see cref="InitializedStateMachineFixture"/> state machine fixture 
  /// to test upon its <see cref="IStateMachine.MachineStarted"/> event.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [ClassDataSource<InitializedStateMachineFixture>]
  public async Task IStateMachine_Initialized_And_Setting_CurrentState_To_StartState_Should_NotRaise_MachineStartedEvent(InitializedStateMachineFixture stateMachine)
  {
    ///Arrange
    bool isStarted = false;

    stateMachine.MachineStarted += (_, _) => isStarted = true;

    ///Act
    ///Initialized StateMachine is already in Start State, so reset it should not trigger state transition.
    stateMachine.CurrentState = stateMachine.StartState;

    ///Assert
    await Assert.That(isStarted).IsFalse();
  }

  /// <summary>
  /// Integration test asserting that given initialized <see cref="InitializedStateMachineFixture"/> state machine fixture
  /// that its <see cref="IStateMachine.MachineStarted"/> and <see cref="IStateMachine.MachineEnded"/> events are set and running it  
  /// should raise ONLY its <see cref="IStateMachine.MachineEnded"/> event.
  /// (An initialized state machine is already in its <see cref="IStateMachine.StartState"/>)
  /// </summary>
  /// <param name="stateMachine">The initialized <see cref="InitializedStateMachineFixture"/> state machine fixture 
  /// to test upon its <see cref="IStateMachine.MachineStarted"/> and <see cref="IStateMachine.MachineEnded"/> events.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [ClassDataSource<InitializedStateMachineFixture>]
  public async Task IStateMachine_Initialized_And_AutoRunningIt_With_Setted_MachineStarted_And_MachineEnded_Event_Should_Raise_Only_MachnineEnded(InitializedStateMachineFixture stateMachine)
  {
    ///Arrange    
    bool machineStartedWasCalled = false;
    bool machineEndedWasCalled = false;

    stateMachine.MachineStarted += (_, _) => machineStartedWasCalled = true;
    stateMachine.MachineEnded += (_, _) => machineEndedWasCalled = true;

    ///Act
    stateMachine.CurrentState = stateMachine.StartState;

    stateMachine.AutorunStateMachine();

    ///Assert
    await Task.Delay(10);
    using (Assert.Multiple())
    {
      await Assert.That(machineStartedWasCalled).IsFalse();///initialized state machine should already be in start state, so not calling MachineStarted again.
      await Assert.That(machineEndedWasCalled).IsTrue();
    }
  }

  /// <summary>
  /// Integration test asserting that given non-initialized <see cref="StateMachineFixture"/> state machine fixture
  /// that its <see cref="IStateMachine.MachineStarted"/> and <see cref="IStateMachine.MachineEnded"/> events are set and running it  
  /// should raise BOTH its <see cref="IStateMachine.MachineStarted"/> and <see cref="IStateMachine.MachineEnded"/> events.
  /// (An non-initialized state machine is waiting in its <see cref="IStateMachine.EndState"/> before running)
  /// </summary>
  ///// <param name="stateMachine">The non-initialized <see cref="StateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="IStateMachine.MachineStarted"/> and <see cref="IStateMachine.MachineEnded"/> events.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateMachine_Not_Initialized_InitializingIt_And_AutoRunningIt_With_Setted_MachineStarted_And_MachineEnded_Event_Should_RaiseThem()
  {
    ///Arrange
    bool machineStartedWasCalled = false;
    bool machineEndedWasCalled = false;
    var stateMachine = StateMachineInstance as StateMachineFixture;

    stateMachine?.MachineStarted += (_, _) => machineStartedWasCalled = true;
    stateMachine?.MachineEnded += (_, _) => machineEndedWasCalled = true;

    ///Act
    stateMachine?.InitializeStateMachine(stateMachine.FirstState, true);

    ///Assert
    await Task.Delay(10);
    using (Assert.Multiple())
    {
      await Assert.That(stateMachine).IsNotNull();
      await Assert.That(machineStartedWasCalled).IsTrue();
      await Assert.That(machineEndedWasCalled).IsTrue();
    }
  }

  /// <summary>
  /// Integration test asserting that given cycling <see cref="TraficLightLoopingStateMachineFixture"/> state machine fixture
  /// that is running and then calling its <see cref="IStateMachine.ForceStopStateMachine"/> method
  /// should raise its <see cref="IStateMachine.MachineEnded"/> event.
  /// </summary>
  /// <param name="traficLightLoopingStateManchine">The cycling <see cref="TraficLightLoopingStateMachineFixture"/> state machine fixture
  /// to test upon its <see cref="IStateMachine.MachineEnded"/> event.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [ClassDataSource<TraficLightLoopingStateMachineFixture>]
  public async Task TraficLightLoopingStateMachine_RunningIt_And_Calling_ForceExitUpdateLoop_Should_Raise_MachineEndedEvent(TraficLightLoopingStateMachineFixture traficLightLoopingStateManchine)
  {
    ///Arrange
    bool traficLightLoopingStateMachineEnded = false;
    int iterationCount = 0;

    traficLightLoopingStateManchine.MachineEnded += (_, _) => traficLightLoopingStateMachineEnded = true;

    ///Act
    traficLightLoopingStateManchine.AutorunStateMachine();
    await Assert.That(traficLightLoopingStateMachineEnded).IsFalse();

    await Task.Delay(10);
    traficLightLoopingStateManchine.ForceStopStateMachine();

    while(!traficLightLoopingStateManchine.IsEnded && iterationCount < 50)
    {
      await Task.Delay(10);
      iterationCount++;
    }

    ///Assert
    await Assert.That(traficLightLoopingStateMachineEnded).IsTrue();
  } 
  #endregion

  /// <summary>
  /// Integration test asserting that given <see cref="PlayerStateMachineFixture"/> player state machine fixture
  /// that call its <see cref="IStateMachine.Update(object?)"/> method
  /// should call its <see cref="IStateMachine.GetNextStateCentralized"/> delegate.
  /// </summary>
  /// <param name="playerStateMachine">The <see cref="PlayerStateMachineFixture"/> player state machine fixture
  /// to test upon its <see cref="IStateMachine.GetNextStateCentralized"/> delegate.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [ClassDataSource<PlayerStateMachineFixture>]
  public async Task PlayerStateManager_Calling_Update_Should_Call_GetNextStateCentralizedDelegate(PlayerStateMachineFixture playerStateMachine)
  {
    ///Arrange
    var mockedGetNextStateCentralized = Mock.OfDelegate<Func<object?, IState?>>();
    mockedGetNextStateCentralized!.Invoke(Any()).Returns(default(IState));

    playerStateMachine.GetNextStateCentralized = mockedGetNextStateCentralized;

    ///Simulate exiting StartState that itself have a decentralized GetNextState()
    playerStateMachine.CurrentState = playerStateMachine.GetNextState();

    ///Act
    playerStateMachine.Update();

    ///Assert
    await Assert.That(mockedGetNextStateCentralized!.Invoke(Any())).WasCalled();
  }

  #region Dispose
  /// <summary>
  /// Integration test asserting that <see cref="StateMachine.Dispose"/> method
  /// called on a initialized <see cref="InitializedOrderedStateMachineFixture"/> state machine
  /// should also call <see cref="BaseState.Dispose(bool)"/> on each of its owned states
  /// </summary>
  ///// <param name="stateMachine">The initialized <see cref="InitializedOrderedStateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="StateMachine.Dispose"/> method.</param>
  /// <returns>The awaitable test assertions.</returns>
  [Test]
  public async Task IStateMachine_Dispose_Should_Call_Dispose_On_OwnedStates()
  {
    ///Arrange
    /// Avoid ClassDataSource as it is constructor at discovery time, not execution time. So constructed state machine 
    /// could have been disposed prior in another test like a CleanAllCache() method testing.
    var stateMachine = Builder.Services.StateMachine_Factory.CreateStateMachine<InitializedOrderedStateMachineFixture>();

    ///Trigger states caching
    BaseMonitoringStateFixture[] stateFixtureArray = [stateMachine.FirstState, stateMachine.MiddleState, stateMachine.LastState];

    bool wasStateMachineCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var instances) &&
      instances?.Count > 0;

    ///Act
    stateMachine.Dispose();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(wasStateMachineCached).IsTrue();
      foreach (var stateFixture in stateFixtureArray)
        ///Note : we only test known arranged states, but it should also call 
        ///<see cref="BaseState.Dispose(bool)"/>> on its built-in owned <see cref="StartState"/> and <see cref="EndState"/>.
        await Assert.That(stateFixture.DisposeCallCount).IsEqualTo(1u);
    }
  }

  /// <summary>
  /// Integration test asserting that <see cref="StateMachine.Dispose"/> method
  /// called on a initialized <see cref="StateMachineFixture"/> state machine
  /// should also call <see cref="BaseState.Dispose(bool)"/> on each of its owned states
  /// </summary>
  ///// <param name="stateMachine">The initialized <see cref="InitializedStateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="StateMachine.Dispose"/> method.</param>
  /// <returns>The awaitable test assertions.</returns>
  [Test]
  public async Task IStateMachine_Dispose_Should_Call_Dispose_On_OwnedStates2()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Simulate state factory creation and caching
    var mockedStateA = StateAFixture.Mock();
    var mockedStateB = StateBFixture.Mock();
    var mockedStateC = StateCFixture.Mock();

    var stateA = mockedStateA.Object;
    var stateB = mockedStateB.Object;
    var stateC = mockedStateC.Object;

    stateA.Initialize(stateMachine);
    stateB.Initialize(stateMachine);
    stateC.Initialize(stateMachine);

    Builder.Services.Cache.TryCacheState(stateA);
    Builder.Services.Cache.TryCacheState(stateB);
    Builder.Services.Cache.TryCacheState(stateC);

    bool wasStateACached = false;
    bool wasStateBCached = false;
    bool wasStateCCached = false;

    bool wasStateMachineCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var instances);

    if (wasStateMachineCached)
    {
      wasStateACached = instances?.ContainsValue(stateA) ?? false;
      wasStateBCached = instances?.ContainsValue(stateB) ?? false;
      wasStateCCached = instances?.ContainsValue(stateC) ?? false;
    }

    ///Act
    stateMachine.Dispose();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(wasStateMachineCached).IsTrue();
      await Assert.That(wasStateACached).IsTrue();
      await Assert.That(wasStateBCached).IsTrue();
      await Assert.That(wasStateCCached).IsTrue();

      ///Note : we only test known arranged states, but it should also call 
      ///<see cref="BaseState.Dispose(bool)"/>> on its built-in owned <see cref="StartState"/> and <see cref="EndState"/>.
      await Assert.That(mockedStateA.Dispose(Any())).WasCalled();
      await Assert.That(mockedStateB.Dispose(Any())).WasCalled();
      await Assert.That(mockedStateC.Dispose(Any())).WasCalled();
    }
  }

  /// <summary>
  /// Integration test asserting that <see cref="StateMachine.Dispose"/> method
  /// called on a initialized <see cref="StateMachineFixture"/> state machine
  /// should clean its own entry in the <see cref="IStateCache.CachedStateInstances"/> cache
  /// </summary>
  ///// <param name="stateMachine">The initialized <see cref="InitializedStateMachineFixture"/> state machine fixture 
  ///// to test upon its <see cref="StateMachine.Dispose"/> method.</param>
  /// <returns>The awaitable test assertion.</returns>
  [Test]
  public async Task IStateMachine_Dispose_Should_CleanItSelf_From_StateCache()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    var wasCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var _);

    ///Act
    stateMachine.Dispose();

    var isCleaned = !Builder.Services.Cache.CachedStateInstances.ContainsKey(stateMachine);

    ///Assert 
    using (Assert.Multiple())
    {
      await Assert.That(wasCached).IsTrue();
      await Assert.That(isCleaned).IsTrue();
    }
  } 
  #endregion

  #region Cache
  /// <summary>
  /// Integration test asserting that <see cref="IStateMachine.GetOrAddState{T_State}(object?)"/> method
  /// on a not already owned <see cref="{T_State}"/> state
  /// should have its returned <see cref="{T_STate}"/> instance state 
  /// cached in <see cref="IStateCache.CachedStateInstances"/> cache.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// to test upon its <see cref="IStateMachine.GetOrAddState{T_State}(object?)"/> method.</param>
  /// <returns>The awaitable test assertions.</returns>
  [Test]
  public async Task IStateMachine_GetOrAddState_Not_AlreadyCached_StateType_Should_HaveIt_Cached_Afterward()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    var wasNewStateCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var instances) &&
      (instances?.TryGetValue(typeof(StateAFixture), out _) ?? false);

    ///Act
    var newState = stateMachine.GetOrAddState<StateAFixture>();

    bool isCached = instances.TryGetValue(typeof(StateAFixture), out var cachedAInstance) && 
      cachedAInstance == newState;

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(newState).IsNotNull();
      await Assert.That(wasNewStateCached).IsFalse();
      await Assert.That(instances).IsNotNull();
      await Assert.That(isCached).IsTrue();
    }
  }

  /// <summary>
  /// Integration test asserting that <see cref="IStateMachine.GetOrAddState{T_State}(object?)"/> method
  /// on an already owned <see cref="{T_State}"/> state
  /// should return the <see cref="{T_STate}"/> instance state
  /// that is already cached  in <see cref="IStateCache.CachedStateInstances"/> cache.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// to test upon its <see cref="IStateMachine.GetOrAddState{T_State}(object?)"/> method.</param>
  /// <returns>The awaitable test assertions.</returns>
  [Test]
  public async Task IStateMachine_GetOrAddState_AlreadyCached_StateType_Should_Return_CachedInstance()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    /// Add and cache a new StateAFixture to given state machine first.
    stateMachine.GetOrAddState<StateAFixture>();

    IState? cachedState = default;

    var isStateMachineCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var instances);
    var wasStateCached = instances?.TryGetValue(typeof(StateAFixture), out cachedState) ?? false;

    ///Act
    var ownedState = stateMachine.GetOrAddState<StateAFixture>();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(cachedState).IsNotNull();
      await Assert.That(ownedState).IsNotNull();
      await Assert.That(isStateMachineCached).IsTrue();
      await Assert.That(wasStateCached).IsTrue();
      await Assert.That(cachedState).IsEqualTo(ownedState);
    }
  }
  #endregion
}

