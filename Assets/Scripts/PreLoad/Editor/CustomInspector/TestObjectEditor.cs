using UnityEngine;
using UnityEditor;
using Extensions;


#if UNITY_EDITOR
[CustomEditor(typeof(TestObject))]
public class TestObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestObject test = (TestObject)target;
        if (GUILayout.Button("Do Test"))
        {
            test.DoTest();
        }

    }
}
#endif
