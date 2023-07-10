using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIComponents
{
    CanvasGroup,
    RectTransform,
    Image,
    Slider,
    InputField,

    View,
    Presenter,

    Custom,
    CustomText,
    CustomEventButton,
    CustomBloomingButton,
    CustomButton,
}

public enum ExcludedUIComponents
{
    CanvasGroup,
    RectTransform,
    Image,
    CustomText,
    CustomBloomingButton,
    CustomButton,
    View,
    Presenter,
    Canvas,
    Mask,
    Transform,
    Button,
    UIHelper,
    CanvasRenderer,
    Count,
}

public class UIHelper : MonoBehaviour
{
    public List<UIData> data;

    [Serializable]
    public struct UIData 
    {
        [SerializeField] private UIComponents component;

        [NonSerialized] public Type actualType;

        public GameObject from;
        public Boolean isList;

        public UIData ConvertActualType()
        {
            var array = from.GetComponents<Component>();

            for(int i=0;i<array.Length;i++)
            {
                if (component == UIComponents.Custom)
                {
                    Debug.Log($"Custom Component Detection : ");
                    Boolean flag = false;

                    for(int k=0;k< (int)ExcludedUIComponents.Count; k++)
                    {
                        Debug.Log($"{((ExcludedUIComponents)k).ToString()} compared to {array[i].GetType().Name}");

                        if (((ExcludedUIComponents)k).ToString().Equals(array[i].GetType().Name))
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag)
                        continue;

                    Debug.Log($"Detected : {array[i].GetType()}");
                    actualType = array[i].GetType();
                    return this;
                }
                else
                {
                    if (array[i].GetType().Name.Equals(component.ToString()))
                    {
                        actualType = array[i].GetType();
                        return this;
                    }

                    if (array[i].GetType().Name.Contains(component.ToString()))
                    {
                        actualType = array[i].GetType();
                        return this;
                    }
                }
            }

            Debug.LogError("Error");
            return this;
        }
    }

    public List<UIData> Generate()
    {
        List<UIData> result = new List<UIData>();

        foreach (var item in data)
            result.Add(item.ConvertActualType());

        return result;
    }

}
