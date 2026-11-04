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
//Copyright Aurélien Pascal Maignan, (25 October 2026) 

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

using ReusableFiniteStateMachine.Sources.Factories.Interfaces;
using ReusableFiniteStateMachine.Sources.Services;
using ReusableFiniteStateMachine.Sources.Cache.Interface;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Sources.Utilities;

namespace ReusableFiniteStateMachine.Sources.StateMachine;

/// <summary>
/// Default <see cref="IStateMachine"/> concrete class implementation.
/// Can be used as it, or being derived from for a more specialized state machine reusing sharing default logics.
/// </summary>
public class StateMachine : IStateMachine
{
  protected bool _disposed = false;

  private readonly Lock _lock = new();

  private StartState? _startState;
  public StartState? StartState => _startState;

  private EndState? _endState;
  public EndState EndState
   => _endState ??= GetOrAddState<EndState>();

  private IState? _currentState;
  private IState? _previousState;
  public IState? CurrentState
  {
    get => _currentState;
    set
    {
      if (_currentState == value)
        return;

      _currentState?.OnExit(value);
      //value?.OnEnter(_currentState);

      OnCurrentStateChanged(new StateChangedEventArgs(_currentState, value));
      
      _previousState = _currentState;
      _currentState = value;

      _currentState?.OnEnter(_previousState);
    }
  }

  public Func<object?, IState?> GetNextStateCentralized { get; set; }
    = _ => default;

  public bool IsEnded
  {
    get
    {
      lock (_lock)
        return CurrentState == EndState; 
    }
  }

  private CancellationTokenSource? _cancellationTokenSource;

  public CancellationTokenSource CancellationTokenSource
    => _cancellationTokenSource ??= new();

  public event EventHandler<EventArgs>? MachineStarted;
  public event EventHandler<EventArgs>? MachineEnded;
  public event EventHandler<EventArgs>? TransitionFailed;
  public event EventHandler<StateChangedEventArgs>? CurrentStateChanged;

  /// <summary>
  /// constructor of this <see cref="StateMachine"/> class that default its 'inner state' 
  /// by calling <see cref="ResetStateMachine"/>.
  /// </summary>
  public StateMachine()
    => _Reset();

  ///<inheritdoc cref="IStateMachine.InitializeStateMachine(IState, bool, object?)"/>
  public void InitializeStateMachine(IState firstState, bool autorun = false, object? argument = default)
  {
    if (firstState == default)
      return;
    
    _startState = GetOrAddState<StartState>(firstState);
    
    CurrentState = _startState;
    
    if (autorun)
      AutorunStateMachine(argument);
  }

  ///<inheritdoc cref="IStateMachine.AutorunStateMachine(object?)"/>
  public virtual void AutorunStateMachine(object? argument = default)
  {
    if (IsEnded)
      return;

    MachineEnded += _CancelUpdateLoop;
    Task.Run(() =>
    {
      while (!CancellationTokenSource.Token.IsCancellationRequested)
        Update(argument);
      
      MachineEnded -= _CancelUpdateLoop;

      CurrentState = EndState;
    }, CancellationTokenSource.Token);
  }

  /// <summary>
  /// Cancel this state machine's <see cref="CancellationTokenSource"/> to exit its infinite update loop.
  /// </summary>
  /// <param name="sender">Not needed here. Used to comply with <see cref="MachineEnded"/> event signature.</param>
  /// <param name="eventArgs">Not needed here. Used to comply with <see cref="MachineEnded"/> event signature.</param>
  private void _CancelUpdateLoop(object? sender, EventArgs? eventArgs)
    => CancellationTokenSource.Cancel();

  ///<inheritdoc cref="IStateMachine.ForceStopStateMachine"/>
  public void ForceStopStateMachine()
    => _CancelUpdateLoop(this, default);

  ///<inheritdoc cref="IStateMachine.GetNextState(object?)"/>
  public virtual IState? GetNextState(object? argument = default)
    => CurrentState?.GetNextState(argument) ?? GetNextStateCentralized?.Invoke(argument);

  ///<inheritdoc cref="IStateMachine.Update(object?)"/>
  public virtual void Update(object? argument = default)
  {
    if (IsEnded)
        return;

    var nextState = GetNextState(argument);

    if (nextState == default)
    {
      ///Something went wrong.
      ///End the state machine.
      OnTransitionFailed(default);

      CurrentState = EndState;
      return;
    }

    ///OnEnter() & OnExit() already automatically called in CurrentState setter.
    ///Setter should avoid resetting it again, but to be sure.      

    CurrentState = nextState;

    CurrentState.Update(argument);
  }

  #region Events
  ///<inheritdoc cref="IStateMachine.RaiseStateMachineStarted(EventArgs?)"/>
  public void RaiseStateMachineStarted(EventArgs? eventArgs = default)
    => OnMachineStarted(eventArgs);

  ///<inheritdoc cref="IStateMachine.RaiseStateMachineEnded(EventArgs?)"/>
  public void RaiseStateMachineEnded(EventArgs? eventArgs = default)
    => OnMachineEnded(eventArgs);

  /// <summary>
  /// Invoke/Raise this <see cref="MachineStarted"/> event.
  /// </summary>
  /// <param name="eventArgs">Any potential <see cref="EventArgs"/> needed when raising 
  /// this <see cref="MachineStarted"/> event.</param>
  protected virtual void OnMachineStarted(EventArgs eventArgs)
    => MachineStarted?.Invoke(this, eventArgs);

  /// <summary>
  /// Invoke/Raise this <see cref="MachineEnded"/> event.
  /// </summary>
  /// <param name="eventArgs">Any potential <see cref="EventArgs"/> needed when raising 
  /// this <see cref="MachineEnded"/> event.</param>
  protected virtual void OnMachineEnded(EventArgs eventArgs)
    => MachineEnded?.Invoke(this, eventArgs);

  /// <summary>
  /// Invoke/Raise this <see cref="TransitionFailed"/> event.
  /// </summary>
  /// <param name="eventArgs">Any potential <see cref="EventArgs"/> needed when raising 
  /// this <see cref="TransitionFailed"/> event.</param>
  protected virtual void OnTransitionFailed(EventArgs eventArgs)
   => TransitionFailed?.Invoke(this, eventArgs);

  /// <summary>
  /// Invoke/Raise this <see cref="CurrentStateChanged"/> event.
  /// </summary>
  /// <param name="stateChangedEventArgs">Any potential <see cref="StateChangedEventArgs"/> needed when raising 
  /// this <see cref="CurrentStateChanged"/> event.</param>
  protected virtual void OnCurrentStateChanged(StateChangedEventArgs stateChangedEventArgs)
   => CurrentStateChanged?.Invoke(this, stateChangedEventArgs);

  ///<inheritdoc cref="IStateMachine.FlushMachineStarted"/>
  public void FlushMachineStarted() 
    => MachineStarted = default;

  ///<inheritdoc cref="IStateMachine.FlushMachineEnded"/>
  public void FlushMachineEnded()
    => MachineEnded = default;

  ///<inheritdoc cref="IStateMachine.FlushTransitionFailed"/>
  public void FlushTransitionFailed() 
    => TransitionFailed = default;

  ///<inheritdoc cref="IStateMachine.FlushCurrentStateChanged"/>
  public void FlushCurrentStateChanged()
    => CurrentStateChanged = default;
  #endregion

  /// <summary>
  /// Reset this <see cref="CancellationTokenSource"/> inner field to default (NULL) 
  /// after properly canceled and disposed it.
  /// </summary>
  private void _RefreshCancellationTokenSource()
  {
    if (_cancellationTokenSource != default)
    {
      _cancellationTokenSource.Cancel();
      _cancellationTokenSource.Dispose();
      _cancellationTokenSource = default;
    }
  }

  private void _Reset(bool resetCurrentState = true)
  {
    _CleanStateMachine();
    
    if(resetCurrentState)
      _currentState = EndState;
  }

  private void _CleanStateMachine()
  {
    _startState = default;
    _endState = default;
    _currentState = default;

    GetNextStateCentralized = _ => default;

    _RefreshCancellationTokenSource();

    ((IStateMachine)this).FlushMachineEvents();
  }

  ///<inheritdoc cref="IStateMachine.ResetStateMachine"/>
  public virtual void ResetStateMachine(bool resetCurrentState = true)
    => _Reset(resetCurrentState);

  /// <inheritdoc cref="IDisposable.Dispose"/>
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Disposing this <see cref="IStateMachine"/> will dispose the corresponding owned <see cref="IState"/> 
  /// from <see cref="IStateCache.CachedStateInstances"/> and itself from it too
  /// and also reset its 'internal state'.
  /// </summary>
  /// <param name="disposing">The <see langword="bool"/> parameter indicating whether or not this 
  /// <see cref="IStateMachine"/> is disposed from IDisposable.Dispose() or from its finalizer (not used here),
  /// following .Net IDisposable pattern.</param>
  internal protected virtual void Dispose(bool disposing)
  {
    if (_disposed)
      return;

    if (disposing)
    {
      Builder.Services.Cache.CleanCache(this);

      ResetStateMachine(false);
    }

    _disposed = true;
  }

  ///<inheritdoc cref="IStateFactory.GetOrCreateState{T_State}(IStateMachine, object?)"/>
  public T_State GetOrAddState<T_State>(object? argument = default) 
    where T_State : class, IState, new()
    => Builder.Services.State_Factory.GetOrCreateState<T_State>(this, argument);
}