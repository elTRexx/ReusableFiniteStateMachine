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
using ReusableFiniteStateMachine.Sources.Services;
using ReusableFiniteStateMachine.Sources.StateMachine;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;
using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;

namespace ReusableFiniteStateMachine.Tests.Unit;

/// <summary>
/// Test class that encapsulate all <see cref="IStateCache"/> unit tests.
/// </summary>
public class StateCacheUnitTests
{
  #region CleanCache
  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.CleanCache(IState)"/> method
  /// called with a not already cached <see cref="IState"/> state argument
  /// should not call its <see cref="BaseState.ClearOwner"/> method.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateFactory_CleanCache_On_NotCached_IState_Should_Not_Call_ClearOwner()
  {
    ///Arrange
    var mockedState = BaseState.Mock();
    var state = mockedState.Object;
    state.Initialize(StateMachineInstance);

    ///Act
    Builder.Services.Cache.CleanCache(state);

    ///Assert
    await Assert.That(mockedState.ClearOwner).WasNeverCalled();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.CleanCache(IState)"/> method
  /// called with an already cached <see cref="IState"/> state argument
  /// should call its <see cref="BaseState.ClearOwner"/> method.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_CleanCache_On_Cached_IState_Should_Call_ClearOwner()
  {
    ///Arrange
    var mockedState = BaseState.Mock();
    var state = mockedState.Object;
    state.Initialize(StateMachineInstance);
    bool isCached = Builder.Services.Cache.TryCacheState(state);

    ///Act
    Builder.Services.Cache.CleanCache(state);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(isCached).IsTrue();
      await Assert.That(mockedState.ClearOwner).WasCalled();
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.CleanCache(IStateMachine)"/> method called
  /// with a given <see cref="StateMachineFixture"/> state machine fixture
  /// should call <see cref="BaseState.Dispose(bool)"/> method on each of its owned cached <see cref="IState"/> states.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// used to test upon <see cref="IStateFactory.CleanCache(IStateMachine)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_CleanCache_With_Given_IStateMachine_Should_Call_Dispose_On_CachedStates()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    /// Simulate state factory creation and caching
    var mockedStateA = StateAFixture.Mock();
    var mockedStateB = StateBFixture.Mock();
    var mockedStateC = StateCFixture.Mock();

    var stateA = mockedStateA.Object;
    var stateB = mockedStateB.Object;
    var stateC = mockedStateC.Object;

    stateA.Initialize(stateMachine);
    stateB.Initialize(stateMachine);
    stateC.Initialize(stateMachine);

    Builder.Services.Cache .TryCacheState(stateA);
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
    Builder.Services.Cache.CleanCache(stateMachine);

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
  #endregion

  #region CleanAllCache
  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.CleanAllCache"/> method
  /// should clear the <see cref="IStateCache.CachedStateInstances"/> cache.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [NotInParallel]/// Test on whole shared <see cref="IStateCache.CachedStateInstances"/> cache.
  public async Task IStateFactory_CleanAllCache_Should_Clear_Cache()
  {
    ///Arrange

    /// Caching a <see cref="IStateMachine"/> state machine first
    StateMachineInstance.GetOrAddState<StateAFixture>();
    bool wasCacheEmpty = Builder.Services.Cache.CachedStateInstances.Count == 0;

    ///Act
    Builder.Services.Cache.CleanAllCache();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(wasCacheEmpty).IsFalse();
      await Assert.That(Builder.Services.Cache.CachedStateInstances.Count).IsEqualTo(0);
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.CleanAllCache"/> method
  /// should call <see cref="StateMachine.Dispose"/> on each cached <see cref="IStateMachine"/>.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [NotInParallel]/// Test on whole shared <see cref="IStateCache.CachedStateInstances"/> cache.
  public async Task IStateFactory_CleanAllCache_Should_Call_Dispose_On_Each_Cached_IStateMachine_Owned_IState()
  {
    ///Arrange
    var mockedStateMachine1 = StateMachineFixture.Mock();
    var mockedStateMachine2 = StateMachineFixture.Mock();
    var mockedStateMachine3 = StateMachineFixture.Mock();

    var stateMachine1 = mockedStateMachine1.Object;
    var stateMachine2 = mockedStateMachine2.Object;
    var stateMachine3 = mockedStateMachine3.Object;

    ///Trigger caching.
    stateMachine1.GetOrAddState<StateAFixture>();
    stateMachine2.GetOrAddState<StateBFixture>();
    stateMachine3.GetOrAddState<StateCFixture>();

    bool isStateMachine1Cached = Builder.Services.Cache.CachedStateInstances.ContainsKey(stateMachine1);
    bool isStateMachine2Cached = Builder.Services.Cache.CachedStateInstances.ContainsKey(stateMachine2);
    bool isStateMachine3Cached = Builder.Services.Cache.CachedStateInstances.ContainsKey(stateMachine3);

    ///Act
    Builder.Services.Cache.CleanAllCache();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(isStateMachine1Cached).IsTrue();
      await Assert.That(isStateMachine2Cached).IsTrue();
      await Assert.That(isStateMachine3Cached).IsTrue();

      await Assert.That(mockedStateMachine1.Dispose(Any())).WasCalled();
      await Assert.That(mockedStateMachine2.Dispose(Any())).WasCalled();
      await Assert.That(mockedStateMachine3.Dispose(Any())).WasCalled();
    }
  }
  #endregion

  #region TryGetCachedState
  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryGetCachedState{T_State}(IStateMachine, out T_State)"/> method
  /// called with a <see langword="null"/> <see cref="IStateMachine"/> argument
  /// should return <see langword="false"/>.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateFactory_TryGetCachedState_With_Null_IStateMachine_Should_Return_False()
  {
    ///Arrange
    ///Act
    bool wasCached = Builder.Services.Cache.TryGetCachedState<StateAFixture>(default!, out _);

    ///Assert
    await Assert.That(wasCached).IsFalse();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryGetCachedState{T_State}(IStateMachine, out T_State)"/> method
  /// called with a <see langword="null"/> <see cref="IStateMachine"/> state machine argument
  /// should have its out <see cref="{T_State}"/> parameter being a new, not null, object instance.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_TryGetCachedState_With_Null_IStateMachine_Should_Out_NonNullInstance_Of_Given_StateType()
  {
    ///Arrange

    ///Act
    Builder.Services.Cache.TryGetCachedState<StateAFixture>(default!, out var state);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(state).IsNotNull();
      await Assert.That(state).IsTypeOf<StateAFixture>();
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryGetCachedState{T_State}(IStateMachine, out T_State)"/> method
  /// called with an <see cref="IStateMachine"/> state machine argument on 
  /// a not already cached concrete <see cref="{T_State}"/> <see cref="IState"/> state type
  /// should return <see langword="false"/>.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateFactory_TryGetCachedState_With_IStateMachine_On_NotAlreadyCached_StateType_Should_Return_False()
  {
    ///Arrange

    ///Act
    bool wasCached = Builder.Services.Cache.TryGetCachedState<StateAFixture>(StateMachineInstance, out _);

    ///Assert
    await Assert.That(wasCached).IsFalse();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryGetCachedState{T_State}(IStateMachine, out T_State)"/> method
  /// called with an <see cref="IStateMachine"/> state machine argument on 
  /// a not already cached concrete <see cref="{T_State}"/> <see cref="IState"/> state type
  /// should have its out <see cref="{T_State}"/> parameter being a new, not null, object instance.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_TryGetCachedState_With_IStateMachine_On_NotAlreadyCached_StateType_Should_Out_NonNullInstance_Of_Given_StateType()
  {
    ///Arrange

    ///Act
    Builder.Services.Cache.TryGetCachedState<StateAFixture>(StateMachineInstance, out var state);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(state).IsNotNull();
      await Assert.That(state).IsTypeOf<StateAFixture>();
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryGetCachedState{T_State}(IStateMachine, out T_State)"/> method
  /// called with an <see cref="IStateMachine"/> state machine argument on 
  /// an already cached concrete <see cref="{T_State}"/> <see cref="IState"/> state type
  /// should return <see langword="true"/>.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateFactory_TryGetCachedState_With_IStateMachine_On_AlreadyCached_StateType_Should_Return_True()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;
    /// Trigger caching of a StateAFixture instance in the cache.
    stateMachine.GetOrAddState<StateAFixture>();

    ///Act
    bool wasCached = Builder.Services.Cache.TryGetCachedState<StateAFixture>(stateMachine, out _);

    ///Assert
    await Assert.That(wasCached).IsTrue();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryGetCachedState{T_State}(IStateMachine, out T_State)"/> method
  /// called with an <see cref="IStateMachine"/> state machine argument on 
  /// an already cached concrete <see cref="{T_State}"/> <see cref="IState"/> state type
  /// should have its out <see cref="{T_State}"/> parameter being that already cached object instance.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_TryGetCachedState_With_IStateMachine_On_AlreadyCached_StateType_Should_Out_TheCachedInstance_Of_Given_StateType()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;
    /// Trigger caching of a StateAFixture instance in the cache.
    var expectedCachedState = stateMachine.GetOrAddState<StateAFixture>();

    ///Act
    Builder.Services.Cache.TryGetCachedState<StateAFixture>(stateMachine, out var cachedState);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(expectedCachedState).IsNotNull();
      await Assert.That(cachedState).IsNotNull();
      await Assert.That(cachedState).IsEqualTo(expectedCachedState);
    }
  }
  #endregion

  #region TryCacheState
  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryCacheState(IState)"/> method
  /// called with a <see langword="null"/> <see cref="IState"/> argument
  /// should return <see langword="false"/>.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateFactory_TryCacheState_With_Null_IState_Should_Return_False()
  {
    ///Arrange

    ///Act
    bool isCached = Builder.Services.Cache.TryCacheState(null!);

    ///Assert
    await Assert.That(isCached).IsFalse();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryCacheState(IState)"/> method
  /// called with an <see cref="IState"/> argument that is not owned by a <see cref="IStateMachine"/> state machine
  /// should return <see langword="false"/>.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateFactory_TryCacheState_With_A_NotOwned_IState_Should_Return_False()
  {
    ///Arrange
    /// Can not use Mock() as its .Object GetType() is not the expected IState implementing class one.
    /// Can not use state factory as it automatically register new state instance with its state machine owner.
    var stateWithoutOwner = new StateAFixture();

    ///Act
    bool isCached = Builder.Services.Cache.TryCacheState(stateWithoutOwner);

    ///Assert
    await Assert.That(isCached).IsFalse();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryCacheState(IState)"/> method
  /// called with an <see cref="IState"/> argument that is owned by a <see cref="IStateMachine"/> state machine
  /// should return <see langword="true"/>.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateFactory_TryCacheState_With_An_Owned_IState_Not_Already_Cached_Should_Return_True()
  {
    ///Arrange
    /// Can not use Mock() as its .Object GetType() is not the expected IState implementing class one.
    /// Can not use state factory as it automatically register new state instance with its state machine owner.
    var stateWithOwner = new StateAFixture();
    stateWithOwner.Initialize(StateMachineInstance);

    ///Act
    bool isCached = Builder.Services.Cache.TryCacheState(stateWithOwner);

    ///Assert
    await Assert.That(isCached).IsTrue();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryCacheState(IState)"/> method
  /// called with an <see cref="IState"/> argument that is already cached
  /// should return <see langword="false"/>.
  /// </summary>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  public async Task IStateFactory_TryCacheState_With_An_Owned_IState_Already_Cached_Should_Return_False()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act
    bool isCached = Builder.Services.Cache.TryCacheState(stateMachine.EndState);

    ///Assert
    await Assert.That(isCached).IsFalse();
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryCacheState(IState)"/> method
  /// called with a <see langword="null"/> <see cref="IState"/> argument
  /// should not alter/change <see cref="IStateCache.CachedStateInstances"/> cache.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [NotInParallel]/// Test on whole shared <see cref="IStateCache.CachedStateInstances"/> cache.
  public async Task IStateFactory_TryCacheState_With_Null_IState_Should_Not_Alter_Cache()
  {
    ///Arrange
    /// Make a copy
    var previousCache =
      Builder.Services.Cache.CachedStateInstances.ToDictionary();

    ///Act
    Builder.Services.Cache.TryCacheState(null!);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(previousCache).IsNotNull();
      await Assert.That(Builder.Services.Cache.CachedStateInstances).IsEquivalentTo(previousCache);
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryCacheState(IState)"/> method
  /// called with an <see cref="IState"/> argument that is not owned by a <see cref="IStateMachine"/> state machine
  /// should not alter/change <see cref="IStateCache.CachedStateInstances"/> cache.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  [NotInParallel]/// Test on whole shared <see cref="IStateCache.CachedStateInstances"/> cache.
  public async Task IStateFactory_TryCacheState_With_A_NotOwned_IState_Should_Not_Alter_Cache()
  {
    ///Arrange
    /// Make a copy
    var previousCache =
      Builder.Services.Cache.CachedStateInstances.ToDictionary();
    /// Can not use Mock() as its .Object Type is not the expected IState implementing class one.
    /// Can not use state factory as it automatically register new state instance with its state machine owner.
    var stateWithoutOwner = new StateAFixture();

    ///Act
    Builder.Services.Cache.TryCacheState(stateWithoutOwner);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(previousCache).IsNotNull();
      await Assert.That(Builder.Services.Cache.CachedStateInstances).IsEquivalentTo(previousCache);
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryCacheState(IState)"/> method
  /// called with an <see cref="IState"/> argument that is owned by a <see cref="IStateMachine"/> state machine
  /// and not already cached
  /// should add it to <see cref="IStateCache.CachedStateInstances"/> cache afterward.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_TryCacheState_With_An_Owned_IState_Not_Already_Cached_Should_CacheIt_Afterward()
  {
    ///Arrange
    /// Can not use Mock() as its .Object Type is not the expected IState implementing class one.
    /// Can not use state factory as it automatically register new state instance with its state machine owner.
    var stateWithOwner = new StateAFixture();
    var stateMachine = StateMachineInstance;

    stateWithOwner.Initialize(stateMachine);

    bool wasCached = Builder.Services.Cache.TryGetCachedState<StateAFixture>(stateMachine, out _);
    bool isCached;

    ///Act
    Builder.Services.Cache.TryCacheState(stateWithOwner);

    ///Assert
    isCached = Builder.Services.Cache.TryGetCachedState<StateAFixture>(stateMachine, out var cachedState);

    using (Assert.Multiple())
    {
      await Assert.That(wasCached).IsFalse();
      await Assert.That(isCached).IsTrue();
      await Assert.That(cachedState).IsNotNull();
      await Assert.That(cachedState).IsEqualTo(stateWithOwner);
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateCache.TryCacheState(IState)"/> method
  /// called with an <see cref="IState"/> argument that is owned by a <see cref="IStateMachine"/> state machine
  /// and already cached
  /// should not alter/change <see cref="IStateCache.CachedStateInstances"/> cache.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_TryCacheState_With_An_Already_Cached_IState_Should_Not_Alter_Cache()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    bool wasStateMachineCached =
      Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var previousStateMachineCache) &&
      previousStateMachineCache != default;

    ///Act
    Builder.Services.Cache.TryCacheState(stateMachine.EndState);
    Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var newStateMachineCache);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(wasStateMachineCached).IsTrue();
      await Assert.That(previousStateMachineCache).IsNotNull();
      await Assert.That(newStateMachineCache).IsEquivalentTo(previousStateMachineCache);
    }
  }
  #endregion
}
