using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMBehaviour<TstateEnum, TstateTarget> : MonoBehaviour
    where TstateEnum : System.Enum
    where TstateTarget : class
{
    public StateMachine<TstateTarget> stateMachine { get; private set; }
    protected Dictionary<TstateEnum, FSMState<TstateTarget>> stateByEnum;
    [ReadOnly] public TstateEnum nowState;

    private void Start() => FSMBehaviourStart();

    private void Awake()
    {
        stateByEnum = new Dictionary<TstateEnum, FSMState<TstateTarget>>();
        stateMachine = new StateMachine<TstateTarget>();
        FSMBehaviourAwake();
        StateMachineInit();
    }

    private void Update()
    {
        stateMachine.Update();
        FSMBehaviourUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
        FSMBehaviourFixedUpdate();
    }

    public virtual void FSMBehaviourUpdate() { }
    public virtual void FSMBehaviourFixedUpdate() { }
    public virtual void FSMBehaviourAwake() { }
    public virtual void FSMBehaviourStart() { }
    public virtual void OnStateChanged(TstateEnum state) { }

    public virtual void ChangeState(TstateEnum state)
    {
        nowState = state;
        stateMachine.ChangeState(stateByEnum[state]);
        OnStateChanged(state);
    }
    abstract protected void StateMachineInit();
}
