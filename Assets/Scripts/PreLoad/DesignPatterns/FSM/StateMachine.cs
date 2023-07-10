
public class StateMachine<T> where T : class
{
    private T _target;
    private FSMState<T> _currentState = null;

    public void Delete()
    {
        _target = null;
        _currentState = null;
    }
    public void ChangeState(FSMState<T> newState)
    {
        if (newState == _currentState)
            return;

        if(_currentState != null)
            _currentState.Exit(_target);

        _currentState = newState;

        if(_currentState != null)
            _currentState.Enter(_target);
    }
    public void Init(T initTarget,FSMState<T> initState)
    {
        _target = initTarget;
        ChangeState(initState);
    }
    public void Update()
    {
        if (_currentState == null)
            return;
        _currentState.Update(_target);
    }
    public void FixedUpdate()
    {
        if (_currentState == null)
            return;
        _currentState.FixedUpdate(_target);
    }
}

