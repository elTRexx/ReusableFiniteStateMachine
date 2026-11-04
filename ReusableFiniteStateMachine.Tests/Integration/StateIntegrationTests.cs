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
using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;

namespace ReusableFiniteStateMachine.Tests.Integration;

/// <summary>
/// Test class that encapsulate all <see cref="IState"/> integration tests.
/// </summary>
public class StateIntegrationTests
{
  /// <summary>
  /// Integration test asserting that calling <see cref="BaseState.Dispose()"/> method
  /// should clear (set its value to <see langword="null"/>) its <see cref="BaseState.StateMachineOwner"/> property.
  /// </summary>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IState_Dispose_Should_ClearOwner()
  {
    ///Arrange
    var firstState = StateMachineInstance.GetOrAddState<FirstStateFixture>();
    bool wasOwnerNull = firstState?.StateMachineOwner == default;

    ///Act
    firstState?.Dispose();

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(wasOwnerNull).IsFalse();
      await Assert.That(firstState?.StateMachineOwner).IsNull();
    }
  }

  /// <summary>
  /// Integration test asserting that calling <see cref="BaseState.Dispose()"/> method
  /// should clean the <see cref="IStateCache.CachedStateInstances"/> cache 
  /// by removing itself as a value from this cache.
  /// </summary>
  ///// <param name="stateMachine">The <see cref="InitializedStateMachineFixture"/> state machine fixture
  ///// that own the <see cref="IState"/> state to test upon its <see cref="BaseState.Dispose()"/> method
  ///// (as its instance is cached under its <see cref="IStateMachine"/> state machine owner as a key entry).</param>
  /// <returns>The awaited test assertions.</returns>
  [Test]
  public async Task IState_Dispose_Should_Clean_StateFactory_Cache()
  {
    ///Arrange
    var stateMachine = StateMachineInstance;

    var state = stateMachine.EndState;

    ///TODO? Shall be locked / NotInParallel ???
    bool wasCached = Builder.Services.Cache.CachedStateInstances.TryGetValue(stateMachine, out var instances) &&
      (instances?.ContainsValue(state) ?? false);
    
    bool isCleaned;

    ///Act
    state?.Dispose();
    isCleaned = instances != null && !instances.ContainsValue(state);

    ///Assert
    using (Assert.Multiple())
    {
      await Assert.That(state).IsNotNull();
      await Assert.That(wasCached).IsTrue();
      await Assert.That(isCleaned).IsTrue();
    }
  }
}
