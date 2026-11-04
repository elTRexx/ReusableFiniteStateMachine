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
using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;

namespace ReusableFiniteStateMachine.Tests.Fixture.StateFixtures;

/// <summary>
/// Derived class of <see cref="BaseState"/> especially specialized for tests
/// that monitor different methods calls via some backed properties.
/// </summary>
public class BaseMonitoringStateFixture : BaseState
{
  public uint ClearOwnerCallCount { get; private set; } = 0;

  public uint DisposeCallCount { get; private set; } = 0;

  //public ulong UpdateCallCount { get; private set; } = 0;

  public IState? PreviousState { get; private set; }
  public IState? NextState { get; private set; }

  public bool Disposing { get; private set; }

  /// <summary>
  /// <inheritdoc cref="BaseState.GetNextState(object?)"/>
  /// This <see cref="BaseMonitoringStateFixture"/> directly transition to its <see cref="IStateMachine"/> state machine owner's <see cref="IStateMachine.EndState"/>.
  /// </summary>
  /// <param name="argument">Not used here.</param>
  /// <returns>the <see cref="IStateMachine.EndState"/> of its <see cref="IStateMachine"/> state machine owner.</returns>
  public override IState? GetNextState(object? argument = null)
    => StateMachineOwner?.EndState;

  /// <summary>
  /// <inheritdoc cref="BaseState.Init(object?)"/>
  /// Setting this <see cref="BaseMonitoringStateFixture"/> monitored properties to their default value.
  /// </summary>
  /// <param name="argument">Not used here.</param>
  internal protected override void Init(object? argument = null)
  {
    ClearOwnerCallCount = 0;
    //UpdateCallCount = 0;
    Disposing = false;
    PreviousState = default;
    NextState = default;
  }

  /// <summary>
  /// <inheritdoc cref="BaseState.OnExit(IState?)"/>
  /// Keeping track of <paramref name="previousState"/> value in <see cref="PreviousState"/> property,
  /// used for monitoring in tests methods.
  /// </summary>
  /// <param name="previousState">The <see cref="IState"/> to be monitored.</param>
  public override void OnEnter(IState? previousState)
    => PreviousState = previousState;

  /// <summary>
  /// <inheritdoc cref="BaseState.OnExit(IState?)"/>
  /// Keeping track of <paramref name="nextState"/> value in <see cref="NextState"/> property,
  /// used for monitoring in tests methods.
  /// </summary>
  /// <param name="nextState">The <see cref="IState"/> to be monitored.</param>
  public override void OnExit(IState? nextState)
    => NextState = nextState;

  /// <summary>
  /// <inheritdoc cref="BaseState.Update(object?)"/>
  /// No update logic performed here.
  /// </summary>
  /// <param name="argumentObject">Not used here.</param>
  public override void Update(object? argumentObject = null)
  {
    /////Update can be called a lot.
    //UpdateCallCount = UpdateCallCount == ulong.MaxValue ? UpdateCallCount : UpdateCallCount+1;
  }

  /// <summary>
  /// <inheritdoc cref="BaseState.Dispose(bool)"/>
  /// Keeping track of how many time this method is called in <see cref="DisposeCallCount"/> property
  /// as well as <paramref name="disposing"/> value in <see cref="Disposing"/> property,
  /// used for monitoring in tests methods.
  /// </summary>
  /// <param name="disposing">The <see langword="bool"/> value to be monitored.</param>
  internal protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    ++DisposeCallCount;
    Disposing = disposing;
  }

  /// <summary>
  /// <inheritdoc cref="BaseState.ClearOwner()"/>
  /// Keeping track of how many time this method is called in <see cref="ClearOwnerCallCount"/> property,
  /// used for monitoring in tests methods.
  /// </summary>
  internal override void ClearOwner()
  {
    base.ClearOwner();
    ++ClearOwnerCallCount;
  }
}
