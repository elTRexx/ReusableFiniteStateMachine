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

using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Sources.Cache.Interface;
using ReusableFiniteStateMachine.Sources.Factories.Interfaces;
using ReusableFiniteStateMachine.Sources.Utilities;

namespace ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;

/// <summary>
/// Interface for any state machine this library provide and handle.
/// </summary>
public interface IStateMachine : IDisposable
{
  StartState? StartState { get; }
  EndState EndState { get; }

  IState? CurrentState { get; set; }

  bool IsEnded { get; }

  CancellationTokenSource CancellationTokenSource { get; }

  event EventHandler<EventArgs>? MachineStarted;
  event EventHandler<EventArgs>? MachineEnded;
  event EventHandler<EventArgs>? TransitionFailed;
  event EventHandler<StateChangedEventArgs>? CurrentStateChanged;

  /// <summary>
  /// Delegate that embed the centralized logic to compute the next <see cref="IState"/> 
  /// for this <see cref="IStateMachine"/> state machine at any given step.
  /// (default is NULL as we prioritize decentralized paradim here)
  /// </summary>
  Func<object?, IState?>? GetNextStateCentralized { get; set; }

  /// <summary>
  /// Reset this <see cref="IStateMachine"/> to default 'inner state' (properties, delegates, events and cancellation token source)
  /// <param name="resetCurrentState">Indicate whether or not to reset this <see cref="CurrentState"/> 
  /// to <see cref="EndState"/> or keep it to <see langword="null"/> (default is <see langword="true"/>).</param>
  /// Note : called with <see langword="false"/> argument is used when disposing this <see cref="IStateMachine"/>.
  /// </summary>
  /// <param name="resetCurrentState"><see langword="bool"/> flag indicating wether or not to all reset this <see cref="IStateMachine.CurrentState"/> back to its default value (<see cref="EndState"/>) (default value is <see langword="true"/>).</param>
  void ResetStateMachine(bool resetCurrentState = true);

  /// <summary>
  /// Logic to get the next <see cref="IState"/> of this <see cref="IStateMachine"/> state machine,
  /// prioritizing a decentralized paradim where each <see cref="IState"/> compute their next possibles <see cref="IState"/>,
  /// still alowing a centralized option by using the <see cref="GetNextStateCentralized"/> delegate.
  /// </summary>
  /// <param name="argument">Any potential arguments needed to compute the next state 
  /// for this <see cref="IStateMachine"/> state machine. (default is NULL)</param>
  /// <returns>The next logical <see cref="IState"/> this <see cref="IStateMachine"/> should return at any step.</returns>
  IState? GetNextState(object? argument = default);

  /// <summary>
  /// Initialize this <see cref="IStateMachine"/> state machine with given <paramref name="firstState"/> as its first <see cref="IState"/> to begin with,
  /// and then potentially running it automatically along given <paramref name="autorun"/> value.
  /// </summary>
  /// <param name="firstState">The first <see cref="IState"/> that this <see cref="IStateMachine"/> state machine should begin with.</param>
  /// <param name="autorun">Setting wether or not to automatically run this <see cref="IStateMachine"/> state machine after this initialization occur.</param>
  /// <param name="argument">Any potential arguments needed to compute the next state 
  /// for this <see cref="IStateMachine"/> state machine when running it. (default is NULL)</param>
  void InitializeStateMachine(IState firstState, bool autorun = false, object? argument = default);

  /// <summary>
  /// Automatically run this <see cref="IStateMachine"/> state machine until either its <see cref="EndState"/> is reached
  /// or its end is requested.
  /// </summary>
  /// <param name="argument">Any potential arguments needed to compute the next state 
  /// for this <see cref="IStateMachine"/> state machine when running it. (default is NULL)</param>
  void AutorunStateMachine(object? argument = default);

  /// <summary>
  /// Update this <see cref="IStateMachine"/> state machine by computing its next logical <see cref="IState"/>
  /// setting it as its <see cref="CurrentState"/> and calling its own <see cref="IState.Update(object?)"/> method.
  /// </summary>
  /// <param name="argument">Any potential arguments needed to compute the next state 
  /// for this <see cref="IStateMachine"/> state machine when updating it. (default is NULL)</param>
  void Update(object? argument = default);

  /// <summary>
  /// Call this to explicitlly stop running this <see cref="IStateMachine"/> state machine.
  /// </summary>
  void ForceStopStateMachine();

  /// <inheritdoc cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/>
  /// <summary>
  /// Utility method that directly reroute calling <see cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/> method
  /// with itself as the <see cref="IStateMachine"/> argument.
  /// </summary>
  /// <typeparam name="T_State">The sub-type of <see cref="IState"/> to get from cache, or create and add directly to this state machine.</typeparam>
  /// <param name="argument">Any potential arguments needed to initialize a newly created <typeparamref name="T_State"/> object.</param>
  /// <returns>The <typeparamref name="T_State"/> instance cached in <see cref="IStateCache.CachedStateInstances"/> cache under this state machine as the key,
  /// or creating a new one, initializing and caching it under this state machine as the key if none was already cached in the first place.</returns>
  T_State GetOrAddState<T_State>(object? argument = default)
    where T_State : class, IState, new();

  #region Events
  #region Flushing
  /// <summary>
  /// Flush all delegates registered for <see cref="MachineStarted"/> event.
  /// </summary>
  void FlushMachineStarted();

  /// <summary>
  /// Flush all delegates registered for <see cref="MachineEnded"/> event.
  /// </summary>
  void FlushMachineEnded();

  /// <summary>
  /// Flush all delegates registered for <see cref="TransitionFailed"/> event.
  /// </summary>
  void FlushTransitionFailed();

  /// <summary>
  /// Flush all delegates registered for <see cref="CurrentStateChanged"/> event.
  /// </summary>
  void FlushCurrentStateChanged();

  /// <summary>
  /// Flush all reagistered delegates for 
  /// <see cref="MachineStarted"/>, <see cref="MachineEnded"/>, <see cref="TransitionFailed"/> 
  /// and <see cref="CurrentStateChanged"/> events.
  /// </summary>
  void FlushMachineEvents()
  {
    FlushMachineStarted();
    FlushTransitionFailed();
    FlushMachineEnded();
    FlushCurrentStateChanged();
  }
  #endregion

  #region Raising
  /// <summary>
  /// Raise this <see cref="MachineStarted"/> event
  /// </summary>
  /// <param name="eventArgs">Any potential <see cref="EventArgs"/> needed when raising 
  /// this <see cref="MachineStarted"/> event. (default is NULL)</param>
  void RaiseStateMachineStarted(EventArgs? eventArgs = default);

  /// <summary>
  /// Raise this <see cref="MachineEnded"/> event
  /// </summary>
  /// <param name="eventArgs">Any potential <see cref="EventArgs"/> needed when raising 
  /// this <see cref="MachineEnded"/> event. (default is NULL)</param>
  void RaiseStateMachineEnded(EventArgs? eventArgs = default);
  #endregion
  #endregion
}
