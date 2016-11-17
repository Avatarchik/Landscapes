using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(FOHUIButton))]
[CanEditMultipleObjects]
public class EditorFOHUIButton : Editor
{
    private FOHUIButton button;
    private FOHUIWindow parent;

    private int onPointerEnterIndex;
    private int onPointerClickIndex;
    private int onPointerExitIndex;

    private string[] methodNames;
    private GUIContent[] options;

    private void Awake()
    {
        button = target as FOHUIButton;
        parent = button.GetComponentInParent<FOHUIWindow>();
  
        if (EditorPrefs.HasKey(parent.gameObject.name + button.gameObject.name + "PointerClick"))
            onPointerClickIndex = EditorPrefs.GetInt(parent.gameObject.name + button.gameObject.name + "PointerClick");

        if (EditorPrefs.HasKey(parent.gameObject.name + button.gameObject.name + "PointerEnter"))
            onPointerEnterIndex = EditorPrefs.GetInt(parent.gameObject.name + button.gameObject.name + "PointerEnter");

        if (EditorPrefs.HasKey(parent.gameObject.name + button.gameObject.name + "PointerExit"))
            onPointerExitIndex = EditorPrefs.GetInt(parent.gameObject.name + button.gameObject.name + "PointerExit");

        methodNames = GetMethodsInWindow();
        options = new GUIContent[methodNames.Length];
        for (int i = 0; i < options.Length; i++)
        {
            options[i] = new GUIContent("");
        }
    }

    private string[] GetMethodsInWindow()
    {
        MethodInfo[] methods = parent.GetType().GetMethods(BindingFlags.Instance
            | BindingFlags.Public  | BindingFlags.DeclaredOnly);
        MethodInfo doNothing = parent.GetType().GetMethod("FOHDoNothing");

        string[] methodNames = new string[methods.Length+1];

        methodNames[0] = doNothing.Name;
        for (int i = 1; i < methodNames.Length; i++)
        {
            methodNames[i] = methods[i-1].Name;
        }

        return methodNames;
    }

    public override void OnInspectorGUI()
    {
        methodNames = GetMethodsInWindow();

        for (int i = 0; i < options.Length; i++)
        {
            options[i].text = methodNames[i];
        }

        onPointerEnterIndex = EditorGUILayout.Popup(new GUIContent("OnPointerEnter") , onPointerEnterIndex, options);
        onPointerExitIndex = EditorGUILayout.Popup(new GUIContent("OnPointerExit"), onPointerExitIndex, options);
        onPointerClickIndex = EditorGUILayout.Popup(new GUIContent("OnPointerClick") , onPointerClickIndex, options);

        if (GUI.changed)
        {
            button.SetOnPointerEnterMethodName(methodNames[onPointerEnterIndex]);
            button.SetOnPointerClickMethodName(methodNames[onPointerClickIndex]);
            button.SetOnPointerExitMethodName(methodNames[onPointerExitIndex]);
            EditorPrefs.SetInt(parent.gameObject.name + button.gameObject.name + "PointerClick" , onPointerClickIndex);
            EditorPrefs.SetInt(parent.gameObject.name + button.gameObject.name + "PointerEnter", onPointerEnterIndex);
            EditorPrefs.SetInt(parent.gameObject.name + button.gameObject.name + "PointerExit", onPointerExitIndex);
        }
    }
}
