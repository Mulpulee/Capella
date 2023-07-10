using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

public class UnityObjectBag<TObject> : ObjectBag<TObject> where TObject : Component,IPoolable
{
    private Transform _objectParent;
    public UnityObjectBag(Func<TObject> pObjectSpawn, Action<TObject> pObjectReset, Int32 pInitSize,Transform pParent)
    {
        _objectSpawn = pObjectSpawn;
        _objectReset = pObjectReset;
        _objectBag = new ConcurrentBag<TObject>();
        _objectParent = pParent;
        for (Int32 i = 0; i < pInitSize; i++)
        {
            var instance = _objectSpawn.Invoke();
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(pParent);
            _objectBag.Add(instance);
        }
    }

    public override TObject GetObject()
    {
        TObject spawned = base.GetObject();
        spawned.gameObject.SetActive(true);
        spawned.transform.parent = null;
        return spawned;
    }

    public override void Release(TObject pObject)
    {
        base.Release(pObject);
        pObject.transform.SetParent(_objectParent);
        pObject.gameObject.SetActive(false);
    }
}

public class UnityObjectPool<TKey, TObject> : ObjectPool<TKey,TObject> where TObject : Component,IPoolable
{
    private Transform _poolParent;
    public UnityObjectPool(Transform pParent)
    {
        _poolParent = pParent;
    }

    public override void AssignObject(TKey pKey, Func<TObject> pObjectSpawn, Action<TObject> pResetObject, Int32 pSize = 20)
    {
        UnityObjectBag<TObject> objectBag = new UnityObjectBag<TObject>(pObjectSpawn, pResetObject, pSize,_poolParent);
        _poolByKey.Add(pKey, objectBag);
    }

    public override Boolean TrySpawnObject(TKey pKey, out TObject oObject)
    {
        Boolean value = _poolByKey.TryGetValue(pKey, out ObjectBag<TObject> item);
        oObject = item.GetObject();
        return value;
    }

    public override TObject SpawnObject(TKey pKey)
    {
        Boolean value = _poolByKey.TryGetValue(pKey, out ObjectBag<TObject> item);

        if (!value)
            throw new Exception($"{pKey} is unknown key");

        var instance = item.GetObject();

        return instance;
    }

    public override void ReleaseObject(TKey pKey, TObject pObject)
    {
        Boolean value = _poolByKey.TryGetValue(pKey, out ObjectBag<TObject> item);

        if (!value)
            throw new Exception($"{pKey} is unknown key");

        item.Release(pObject);
    }
}
