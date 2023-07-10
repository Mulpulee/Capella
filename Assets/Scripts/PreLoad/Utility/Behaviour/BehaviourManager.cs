using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonoBehaviour
{
    void Update();
}

public class BehaviourManager : IndestructibleSingleton<BehaviourManager>
{
    private List<IMonoBehaviour> _monoBehaviours = new List<IMonoBehaviour>();

    public void AddMonoBehaviour(IMonoBehaviour monoBehaviour) => _monoBehaviours.Add(monoBehaviour);
    public void RemoveMonoBehaviour(IMonoBehaviour monoBehaviour) => _monoBehaviours.Remove(monoBehaviour);

    protected override void OnSingletonInstantiated()
    {
        SceneLoader.onSceneLoaded += _monoBehaviours.Clear;
    }
    private void Update()
    {
        for(int i=0;i<_monoBehaviours.Count;i++)
            _monoBehaviours[i].Update();
    }
}
