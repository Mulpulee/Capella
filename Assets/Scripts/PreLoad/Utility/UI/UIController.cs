using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public static class UIController
{
    private static Component FindObject(string pTag, GameObject pGameObject, Type pType)
    {
        Component[] tempTags = pGameObject.GetComponentsInChildren(pType);

        if (tempTags.Length == 0)
        {
            UnityEngine.Debug.LogError($"Cannot Find IAutoUI with Typeof : {pType} at tag : {pTag}");
            return null;
        }

        for (int i = 0; i < tempTags.Length; i++)
        {
            if (!string.Equals(tempTags[i].gameObject.name, pTag))
                continue;

            var component = tempTags[i].GetComponent(pType);

            if (component == null)
                UnityEngine.Debug.LogError($"Component is not found in tag {pTag}");

            UnityEngine.Debug.Log($"UI element {component.transform.name} activated");
            return component;
        }

        UnityEngine.Debug.LogError($"Component is not found in tag {pTag},{pType}");
        return null;
    }

    public static Component[] FindObjects(string pTag, GameObject pGameObject, Type pType)
    {
        Component[] tempTags = pGameObject.GetComponentsInChildren(pType);

        List<Component> compList = new List<Component>();

        if (tempTags.Length == 0)
        {
            Debug.LogError($"Cannot Find IAutoUI with Typeof : {pType} at tag : {pTag}");
            return null;
        }

        for (int i = 0; i < tempTags.Length; i++)
        {
            if (!string.Equals(tempTags[i].gameObject.name, pTag))
                continue;

            compList.Add(tempTags[i]);
        }

        return compList.ToArray();
    }

    public static void OnUIObjectUpdate()
    {
        Dictionary<Type, List<MonoBehaviour>> instancesByType = new Dictionary<Type, List<MonoBehaviour>>();
        MonoBehaviour[] foundObjects = MonoBehaviour.FindObjectsOfType<MonoBehaviour>();

        foreach (var item in foundObjects)
        {
            Type nowType = item.GetType();
            if (nowType.IsDefined(typeof(ViewAttribute)))
            {
                if (instancesByType.ContainsKey(nowType))
                    instancesByType[nowType].Add(item);
                else
                    instancesByType.Add(nowType, new List<MonoBehaviour>() { item });
            }
        }

        foreach (var instanceTypePair in instancesByType)
        {
            foreach (var item in instanceTypePair.Value)
            {
                Component component = item.GetComponent(instanceTypePair.Key);

                FieldInfo[] fieldInfos = instanceTypePair.Key.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                foreach (FieldInfo fieldItem in fieldInfos)
                {
                    if (!Attribute.IsDefined(fieldItem, typeof(UIElementAttribute)))
                        continue;

                    if (fieldItem.FieldType.IsArray)
                    {
                        Type elementType = fieldItem.FieldType.GetElementType();
                        var instances = FindObjects(fieldItem.Name, component.gameObject, elementType);

                        if (instances != null)
                        {
                            Array newArray = Array.CreateInstance(elementType, instances.Length);
                            Array.Copy(instances, newArray, instances.Length);

                            fieldItem.SetValue(component, newArray);
                        }
                    }
                    else
                    {
                        Component instance = FindObject(fieldItem.Name, component.gameObject, fieldItem.FieldType);

                        if (instance != null)
                            fieldItem.SetValue(component, instance);

                    }

                    Debug.Log($"Initiated : {fieldItem.Name}");
                }

            }
        }
    }


}
