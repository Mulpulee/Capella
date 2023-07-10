using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class FSMBase<TstateEnum, TstateTarget> : IMonoBehaviour 
    where TstateEnum : System.Enum 
    where TstateTarget : class
{
    protected Dictionary<TstateEnum, FSMState<TstateTarget>> _stateByEnum;
    protected StateMachine<TstateTarget> _stateMachine;
    private TstateEnum _nowState;

    public TstateEnum State => _nowState;

    public virtual void Initialize()
    {
        BehaviourManager.ins.AddMonoBehaviour(this);
        _stateByEnum = new Dictionary<TstateEnum, FSMState<TstateTarget>>();
        _stateMachine = new StateMachine<TstateTarget>();
        StateMachineInit();
    }

    public void Delete()
    {
        BehaviourManager.ins.RemoveMonoBehaviour(this);
        _stateByEnum.Clear();
        _stateByEnum = null;

        _stateMachine.Delete();
        _stateMachine = null;
    }

    public void Update()
    {
        _stateMachine?.Update();
    }
    public virtual void ChangeState(TstateEnum state)
    {
        _nowState = state;
        if(_stateByEnum == null)
        {
            Initialize();
            Debug.LogError("Skill Was Not Inited");
        }
        if (_stateByEnum.ContainsKey(state))
            _stateMachine.ChangeState(_stateByEnum[state]);
        else
            Debug.LogError($"Not Included : {state}");
    }
    abstract protected void StateMachineInit(); 
}
