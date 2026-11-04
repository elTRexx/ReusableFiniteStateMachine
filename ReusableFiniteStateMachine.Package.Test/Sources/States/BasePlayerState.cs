using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.States.Interfaces;

namespace ReusableFiniteStateMachine.Package.Test.Sources.States
{
  internal abstract class BasePlayerState : BaseState
  {
    public override IState? GetNextState(object? argument = null)
    {
      throw new NotImplementedException();
    }

    public override void OnEnter(IState? previousState)
    {
      throw new NotImplementedException();
    }

    public override void OnExit(IState? nextState)
    {
      throw new NotImplementedException();
    }

    public override void Update(object? argument = null)
    {
      throw new NotImplementedException();
    }

    protected override void Init(object? argument = null)
    {
      throw new NotImplementedException();
    }
  }
}
