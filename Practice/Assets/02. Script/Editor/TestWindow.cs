using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;

public class TestWindow : EditorWindow
{
    bool toggleValue;

    Object obj = null;

    AnimFloat animFloat = new AnimFloat(0.0001f);
    Texture tex;

    float[] numbers = new float[] { 0, 1, 2 };

    GUIContent[] contents = new GUIContent[]
    {
        new GUIContent("X"),
        new GUIContent("Y"),
        new GUIContent("Z")
    };

    [MenuItem("Window/Example")]
    static void Open()
    {
        GetWindow<TestWindow>();
    }

    private void OnGUI()
    {
        //EditorGUILayout.LabelField("테스트윈도우입니다.");

        //EditorGUI.BeginChangeCheck();

        //toggleValue = EditorGUILayout.ToggleLeft("Toggle", toggleValue);

        //if (EditorGUI.EndChangeCheck())
        //{
        //    if (toggleValue)
        //    {
        //        Debug.Log($"토글벨류 : {toggleValue}");
        //    }
        //}

        //Display();

        //EditorGUILayout.Space();

        //GUI.enabled = false;

        //Display();

        //GUI.enabled = true;

        //bool on = animFloat.value == 1;

        //if (GUILayout.Button(on ? "Close" : "Open", GUILayout.Width(64)))
        //{
        //    animFloat.target = on ? 0.0001f : 1;
        //    animFloat.speed = 0.05f;

        //    var env = new UnityEvent();
        //    env.AddListener(() => Repaint());
        //    animFloat.valueChanged = env;
        //}

        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.BeginFadeGroup(animFloat.value);
        //Display();
        //EditorGUILayout.EndFadeGroup();
        //Display();
        //EditorGUILayout.EndHorizontal();

        obj = EditorGUILayout.ObjectField(obj, typeof(Object), true);
        EditorGUI.MultiFloatField(new Rect(4, 24, 200, EditorGUIUtility.singleLineHeight), new GUIContent("Label"), contents, numbers);
    }


    void Display()
    {
        //EditorGUILayout.ToggleLeft("Toggle2", false);
        //EditorGUILayout.IntSlider(5, 0, 10);
        //GUILayout.Button("Button");

        //EditorGUILayout.BeginVertical();
        //EditorGUILayout.ToggleLeft("Toggle", false);

        //var options = new[] { GUILayout.Width(128), GUILayout.Height(128) };

        //tex = EditorGUILayout.ObjectField(tex, typeof(Texture), false, options) as Texture;

        //GUILayout.Button("Button");
        //EditorGUILayout.EndVertical();
    }
}
