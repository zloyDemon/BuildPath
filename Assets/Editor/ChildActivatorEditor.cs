using UnityEditor;

[CustomEditor(typeof(ChildActivator), true)]
public class ChildActivatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChildActivator activator = (ChildActivator) target;
        if (activator.transform.childCount > 0)
        {
            activator.CurrentChildIndex = EditorGUILayout.IntSlider(activator.CurrentChildIndex, -1,
                activator.transform.childCount - 1);
        }
    }
}
