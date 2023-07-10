using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPoolable
{
    void SetRelease(Action pRelease);
}

/// <summary>
/// Thread Safe Object Pool for Single Type of Object
/// Free from UnityFramework
/// </summary>
/// <typeparam name="TObject"></typeparam>
public class ObjectBag<TObject> where TObject : IPoolable
{
    protected Func<TObject> _objectSpawn;
    protected Action<TObject> _objectReset;
    protected ConcurrentBag<TObject> _objectBag;

    public virtual TObject GetObject()
    {
        TObject instance = _objectBag.TryTake(out TObject item) ? item : _objectSpawn.Invoke();
        instance.SetRelease(() => Release(instance));

        _objectReset.Invoke(instance);
        return instance;
    }

    public virtual void Release(TObject pObject)
    {
        _objectBag.Add(pObject);
    }

    public ObjectBag() { }

    public ObjectBag(Func<TObject> pObjectSpawn,Action<TObject> pObjectReset,Int32 pInitSize = 20)
    {
        _objectSpawn = pObjectSpawn;
        _objectReset = pObjectReset;
        _objectBag = new ConcurrentBag<TObject>();
        for(Int32 i=0;i<pInitSize;i++)
        {
            _objectBag.Add(_objectSpawn.Invoke());
        }
    }
}


/// <summary>
/// Thread Safe Object Pool.
/// Free from Unity Framework
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TObject"></typeparam>
public class ObjectPool<TKey,TObject> where TObject : IPoolable
{
    protected Dictionary<TKey,ObjectBag<TObject>> _poolByKey;

    public ObjectPool()
    {
        _poolByKey = new Dictionary<TKey, ObjectBag<TObject>>();
    }

    public virtual void AssignObject(TKey pKey,Func<TObject> pObjectSpawn,Action<TObject> pResetObject,Int32 pSize = 20)
    {
        ObjectBag<TObject> objectBag = new ObjectBag<TObject>(pObjectSpawn, pResetObject, pSize);
        _poolByKey.Add(pKey, objectBag);
    }

    public virtual Boolean TrySpawnObject(TKey pKey,out TObject oObject)
    {
        Boolean value = _poolByKey.TryGetValue(pKey, out ObjectBag<TObject> item);
        oObject = item.GetObject();
        return value;
    }

    public virtual TObject SpawnObject(TKey pKey)
    {
        Boolean value = _poolByKey.TryGetValue(pKey, out ObjectBag<TObject> item);
        
        if(!value)
            throw new Exception($"{pKey} is unknown key");

        var instance = item.GetObject();

        return instance;
    }

    public virtual void ReleaseObject(TKey pKey,TObject pObject)
    {
        Boolean value = _poolByKey.TryGetValue(pKey, out ObjectBag<TObject> item);

        if (!value)
            throw new Exception($"{pKey} is unknown key");

        item.Release(pObject);
    }
}
