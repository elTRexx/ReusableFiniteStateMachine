#region CeCill-C license
#region English version
//Copyright Aurélien Pascal Maignan, (25 October 2026) 

//[aurelien.maignan@protonmail.com]

//This software is a computer program whose purpose is to provide a reusable state machine
//and provided as a library.

//This software is governed by the CeCILL-C license under French law and
//abiding by the rules of distribution of free software.  You can  use,
//modify and/ or redistribute the software under the terms of the CeCILL-C
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
//knowledge of the CeCILL-C license and that you accept its terms.
#endregion

#region French version
//Copyright Aurélien Pascal Maignan, (25 Octobre 2026) 

//aurelien.maignan@protonmail.com

//Ce logiciel est un programme informatique implémentant une machine à états réutilisable
//et fourni en tant que bibliothèque.

//Ce logiciel est régi par la licence CeCILL-C soumise au droit français et
//respectant les principes de diffusion des logiciels libres.Vous pouvez
//utiliser, modifier et/ou redistribuer ce programme sous les conditions
//de la licence CeCILL-C telle que diffusée par le CEA, le CNRS et l'INRIA 
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
//pris connaissance de la licence CeCILL-C, et que vous en avez accepté les
//termes. 
#endregion
#endregion

using ReusableFiniteStateMachine.Sources.Services;
using ReusableFiniteStateMachine.Sources.Cache.Interface;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States.Interfaces;

namespace ReusableFiniteStateMachine.Sources.States;

/// <summary>
/// Base abstract class that all concrete state classes will derive from.
/// It implement <see cref="IDisposable"/> interface for them here,
/// as well as performing its <see cref="IState.StateMachineOwner"/> initialization.
/// </summary>
public abstract class BaseState : IState
{
  protected bool _disposed = false;

  public IStateMachine? StateMachineOwner { get; private set; }

  ///<inheritdoc cref="IState.GetNextState(object?)"/>
  public abstract IState? GetNextState(object? argument = null);

  ///<inheritdoc cref="IState.Initialize(IStateMachine, object?)"/>
  internal virtual void Initialize(IStateMachine stateMachine, object? argument = null)
  {
    StateMachineOwner ??= stateMachine;

    Init(argument);
  }

  /// <summary>
  /// Initialize this <see cref="BaseState"/> with potential given <paramref name="argument"/>.
  /// </summary>
  /// <param name="argument">Any potential arguments needed to initialize this <see cref="IState"/>. (default is NULL)</param>
  internal protected abstract void Init(object? argument = null);

  ///<inheritdoc cref="IState.OnEnter(IState?)"/>
  public abstract void OnEnter(IState? previousState);

  ///<inheritdoc cref="IState.OnExit(IState?)"/>
  public abstract void OnExit(IState? nextState);

  ///<inheritdoc cref="IState.Update(object?)"/>
  public abstract void Update(object? argument = null);

  /// Using here an internal virtual method to allow testing it
  /// from testing project(s) with InternalsVisibleTo attribute setted,
  /// while keeping it hidden from other projects/assemblies.
  ///<inheritdoc cref="IState.ClearOwner"/>
  internal virtual void ClearOwner()
    => StateMachineOwner = default;

  /// <summary>
  /// <inheritdoc cref="IDisposable.Dispose"/>
  /// </summary>
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Disposing this <see cref="IState"/> will remove its potential cached value
  /// in <see cref="IStateCache.CachedStateInstances"/> cache and 
  /// clear its <see cref="StateMachineOwner"/> property to default.
  /// </summary>
  /// <param name="disposing">The <see langword="bool"/> parameter indicating whether or not this 
  /// <see cref="IState"/> is disposed from IDisposable.Dispose() or from its finalizer (not used here),
  /// following .Net IDisposable pattern.</param>
  internal protected virtual void Dispose(bool disposing)
  {
    if (_disposed || StateMachineOwner == default)
      return;

    if (disposing)
      Builder.Services.Cache.CleanCache(this);

    _disposed = true;
  }

  void IState.ClearOwner()
    => ClearOwner();

  void IState.Initialize(IStateMachine stateMachine, object? argument)
    => Initialize(stateMachine, argument);
}
