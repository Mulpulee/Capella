using UnityEngine;

public class FSMStateSingleton<TType,TTarget> : FSMState<TTarget> where TType : new()
{

    private static TType _ins;
    public static TType ins
    {
        get
        {
            if(_ins == null)
            {
                _ins = new TType();
                Debug.Log($"FSMSingleton Initiated");
            }
            return _ins;
        }
    }

    public override void Enter(TTarget target) { }
    public override void Update(TTarget target) { }
}