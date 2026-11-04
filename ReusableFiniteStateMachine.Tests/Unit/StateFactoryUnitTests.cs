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

using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.Factories.Interfaces;
using ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;
using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;
using ReusableFiniteStateMachine.Sources.Services;

namespace ReusableFiniteStateMachine.Tests.Unit;

/// <summary>
/// Test class that encapsulate all <see cref="IStateFactory"/> unit tests.
/// </summary>
public class StateFactoryUnitTests
{
  #region GetOrCreateState
  /// <summary>
  /// Unit test asserting that <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method
  /// called with a given <see cref="StateMachineFixture"/> state machine fixture 
  /// on any concrete <see cref="{T_State}"/> <see cref="IState"/> state type (<see cref="StateAFixture"/> here) and without extra optional argument 
  /// should return a new <see cref="{T_State}"/> instance object.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// used to test upon <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_GetOrCreate_With_Given_IStateMachine_And_Null_Argument_Should_Return_State_Instance_Of_ExpectedType()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    ///Act
    var newState = Builder.Services.State_Factory?.GetOrCreateState<StateAFixture>(stateMachine);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(newState).IsNotNull();
      await Assert.That(newState).IsTypeOf<StateAFixture>();
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method
  /// called with a given <see cref="StateMachineFixture"/> state machine fixture 
  /// on a not already cached concrete <see cref="{T_State}"/> <see cref="IState"/> state type and without extra optional argument 
  /// should return a new <see cref="{T_State}"/> instance object that wasn't previously cached.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// used to test upon <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_GetOrCreate_With_Given_IStateMachine_And_Null_Argument_On_NotAlreadyCached_StateType_Should_Return_NonAlreadyCached_New_State_Instance()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    /// Assure that given IState type (StateAFixture here) was not already owned.
    bool wasCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var cachedStates) &&
      (cachedStates?.ContainsKey(typeof(StateAFixture)) ?? false);

    ///Act
    var newState = Builder.Services.State_Factory.GetOrCreateState<StateAFixture>(stateMachine);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(wasCached).IsFalse();
      await Assert.That(newState).IsNotNull();
      await Assert.That(newState).IsTypeOf<StateAFixture>();
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method
  /// called with a given <see cref="StateMachineFixture"/> state machine fixture 
  /// on a not already cached concrete <see cref="{T_State}"/> <see cref="IState"/> state type and without extra optional argument 
  /// should have its returned <see cref="{T_State}"/> instance object to be cached afterward.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// used to test upon <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_GetOrCreate_With_Given_IStateMachine_And_Null_Argument_On_NotAlreadyCached_StateType_Should_Cache_Returned_State_Instance()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;
    IState? cachedAInstance = default;

    /// Assure that given IState type (StateAFixture here) was not already owned.
    bool wasCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var previousCachedStates) &&
      (previousCachedStates?.ContainsKey(typeof(StateAFixture)) ?? false);

    ///Act
    var newState = Builder.Services.State_Factory.GetOrCreateState<StateAFixture>(stateMachine);
    bool isCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var newCachedStates) &&
      (newCachedStates?.TryGetValue(typeof(StateAFixture), out cachedAInstance) ?? false);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(previousCachedStates).IsNotNull();///? keep or not ?
      await Assert.That(newState).IsNotNull();
      await Assert.That(wasCached).IsFalse();
      await Assert.That(isCached).IsTrue();
      await Assert.That(cachedAInstance).IsEqualTo(newState);
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method
  /// called with a given <see cref="StateMachineFixture"/> state machine fixture 
  /// on an already cached concrete <see cref="{T_State}"/> <see cref="IState"/> state type and without extra optional argument 
  /// should return that exact already cached <see cref="{T_State}"/> instance object.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// used to test upon <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_GetOrCreate_With_Given_IStateMachine_And_Null_Argument_On_AlreadyCached_StateType_Should_Return_Cached_State_Instance()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;
    IState? cachedState = default;

    /// Pre call GetOrCreate once to cache it first.
    Builder.Services.State_Factory.GetOrCreateState<StateAFixture>(stateMachine);

    /// Assure that given IState type (StateAFixture here) is already owned.
    bool wasCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var cachedStates) &&
      (cachedStates?.TryGetValue(typeof(StateAFixture), out cachedState) ?? false);

    var expectedState = cachedState as StateAFixture;

    ///Act
    var newState = Builder.Services.State_Factory.GetOrCreateState<StateAFixture>(stateMachine);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(wasCached).IsTrue();
      await Assert.That(newState).IsNotNull();
      await Assert.That(expectedState).IsNotNull();
      await Assert.That(newState).IsEqualTo(expectedState);
    }
  }

  /// <summary>
  /// Unit test asserting that <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method
  /// called with a given <see cref="StateMachineFixture"/> state machine fixture 
  /// on a not already cached concrete <see cref="{T_State}"/> <see cref="IState"/> state type and without extra optional argument 
  /// should call <see cref="IState.Initialize(IStateMachine, object?)"/> method on the returned <see cref="{T_State}"/> instance object.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="StateMachineFixture"/> state machine fixture
  ///// used to test upon <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method.</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IStateFactory_GetOrCreate_With_Given_IStateMachine_And_Null_Argument_On_NotAlreadyCached_StateType_Should_Call_Initialize_On_Returned_State_Instance()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    /// Assure that given IState type (StateInitializeMonitoringFixture here) was not already owned.
    bool wasCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var cachedStates) &&
      (cachedStates?.ContainsKey(typeof(StateInitializeMonitoringFixture)) ?? false);

    ///Act
    var newState = Builder.Services.State_Factory.GetOrCreateState<StateInitializeMonitoringFixture>(stateMachine);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(newState).IsNotNull();
      await Assert.That(cachedStates).IsNotNull(); /// ? Keep ?
      await Assert.That(wasCached).IsFalse();
      await Assert.That(newState?.InitializeCallCount).IsPositive();
    }
  }
  #endregion
}
