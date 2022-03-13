using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicBonesConverterWindow : EditorWindow
{
    private GameObject _avatar;

    DynamicBonesController _converter = new DynamicBonesController();

    // UI Settings
    private Vector2 _scroll;

    [MenuItem ("Window/Convert Dynamic Bones")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(DynamicBonesConverterWindow));
    }

    void OnGUI()
    {
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        _avatar = (GameObject) EditorGUILayout.ObjectField("Avatar", _avatar, typeof(GameObject), true);

        if (_avatar != null)
        {
            _converter.setAvatar(_avatar);
        }

        EditorGUI.BeginDisabledGroup(_avatar == null);
        if (GUILayout.Button("Convert To Placeholder"))
        {
            _converter.toPlaceholder();
        }

        if (GUILayout.Button("Convert To Dynamic Bones"))
        {
            _converter.toDynamicBones();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndScrollView();
    }
}
