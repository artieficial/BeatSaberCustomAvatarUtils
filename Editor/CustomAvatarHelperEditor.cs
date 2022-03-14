using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(CustomAvatarHelper))]
[CanEditMultipleObjects]
public class CustomAvatarHelperEditor : Editor
{
    public void OnSceneGUI()
    {
        CustomAvatarHelper customAvatarHelper = (target as CustomAvatarHelper);

        EditorGUI.BeginChangeCheck();
        Quaternion quaternionLeft = Handles.Disc(customAvatarHelper.getLeftHandRotation(), customAvatarHelper.getLeftHandPosition(), Vector3.up, 0.5f, false, 1);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customAvatarHelper.gameObject.transform.Find("LeftHand"), "Rotated Left Hand");
            if (customAvatarHelper.getMirrored())
            {
                Undo.RecordObject(customAvatarHelper.gameObject.transform.Find("RightHand"), "Rotated Right Hand");
            }
            customAvatarHelper.setLeftHandRotation(quaternionLeft);
        }

        EditorGUI.BeginChangeCheck();
        Quaternion quaternionRight = Handles.Disc(customAvatarHelper.getRightHandRotation(), customAvatarHelper.getRightHandPosition(), Vector3.up, 0.5f, false, 1);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customAvatarHelper.gameObject.transform.Find("RightHand"), "Rotated Right Hand");
            if (customAvatarHelper.getMirrored())
            {
                Undo.RecordObject(customAvatarHelper.gameObject.transform.Find("LeftHand"), "Rotated Left Hand");
            }
            customAvatarHelper.setRightHandRotation(quaternionRight);
        }

        EditorGUI.BeginChangeCheck();
        Vector3 positionLeft = Handles.PositionHandle(customAvatarHelper.getLeftHandPosition(), customAvatarHelper.getLeftHandRotation());
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customAvatarHelper.gameObject.transform.Find("LeftHand"), "Moved Left Hand");
            if (customAvatarHelper.getMirrored())
            {
                Undo.RecordObject(customAvatarHelper.gameObject.transform.Find("RightHand"), "Rotated Right Hand");
            }
            customAvatarHelper.setLeftHandPosition(positionLeft);
        }

        EditorGUI.BeginChangeCheck();
        Vector3 positionRight = Handles.PositionHandle(customAvatarHelper.getRightHandPosition(), customAvatarHelper.getRightHandRotation());
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customAvatarHelper.gameObject.transform.Find("RightHand"), "Moved Right Hand");
            if (customAvatarHelper.getMirrored())
            {
                Undo.RecordObject(customAvatarHelper.gameObject.transform.Find("LeftHand"), "Rotated Left Hand");
            }
            customAvatarHelper.setRightHandPosition(positionRight);
        }

        EditorGUI.BeginChangeCheck();
        Quaternion quaternionOffsetLeftHandGrasp = Handles.Disc(customAvatarHelper.getLeftHandGrasp(), customAvatarHelper.getLeftHandPosition(), Vector3.forward, 0.25f, false, 1);
        if (EditorGUI.EndChangeCheck())
        {
            customAvatarHelper.setLeftHandGrasp(quaternionOffsetLeftHandGrasp);
        }

        EditorGUI.BeginChangeCheck();
        Quaternion quaternionOffsetRightHandGrasp = Handles.Disc(customAvatarHelper.getRightHandGrasp(), customAvatarHelper.getRightHandPosition(), Vector3.back, 0.25f, false, 1);
        if (EditorGUI.EndChangeCheck())
        {
            customAvatarHelper.setRightHandGrasp(quaternionOffsetRightHandGrasp);
        }

        EditorGUI.BeginChangeCheck();
        Quaternion quaternionOffsetLeftHandThumb = Handles.Disc(customAvatarHelper.getLeftHandThumb(), customAvatarHelper.getLeftHandPosition(), Vector3.left, 0.25f, false, 1);
        if (EditorGUI.EndChangeCheck())
        {
            customAvatarHelper.setLeftHandThumb(quaternionOffsetLeftHandThumb);
        }

        EditorGUI.BeginChangeCheck();
        Quaternion quaternionOffsetRightHandThumb = Handles.Disc(customAvatarHelper.getRightHandThumb(), customAvatarHelper.getRightHandPosition(), Vector3.right, 0.25f, false, 1);
        if (EditorGUI.EndChangeCheck())
        {
            customAvatarHelper.setRightHandThumb(quaternionOffsetRightHandThumb);
        }

        bool syncShader = customAvatarHelper.getSyncShader();
        int shaderIndex = customAvatarHelper.getShaderIndex();
        if (syncShader && (shaderIndex == 0 || shaderIndex == 2))
        {
            EditorGUI.BeginChangeCheck();
            Vector3 lightSource = new Vector3(0, 3, 1);
            Quaternion lightDirection = Handles.RotationHandle(customAvatarHelper.getLightDirection(), lightSource);
            Handles.DrawDottedLine(lightSource, lightSource + customAvatarHelper.getLightDirection() * Vector3.up, 0.2f);
            if (EditorGUI.EndChangeCheck())
            {
                customAvatarHelper.setLightDirection(lightDirection);
                customAvatarHelper.setShaders();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        CustomAvatarHelper customAvatarHelper = (target as CustomAvatarHelper);
        bool syncShader = customAvatarHelper.getSyncShader();
        customAvatarHelper.setMirrored(EditorGUILayout.Toggle("Mirrored", customAvatarHelper.getMirrored()));
        customAvatarHelper.setSyncShader(EditorGUILayout.Toggle("Sync Shader", syncShader));
        Texture transparent = (Texture) AssetDatabase.LoadAssetAtPath("Assets/BeatsaberConverter/Textures/Transparent.png", typeof(Texture));
        customAvatarHelper.setTransparent(transparent);

        Dictionary<string, bool> modifiedTransparencies = new Dictionary<string, bool>();
        foreach (KeyValuePair<string, bool> entry in customAvatarHelper.getTransparencies())
        {
            bool transparentMaterial = EditorGUILayout.Toggle(string.Format("Transparent {0}", entry.Key), entry.Value);
            if (transparentMaterial != entry.Value)
            {
                modifiedTransparencies[entry.Key] = transparentMaterial;  
            }
        }

        // Don't modify in place
        foreach(KeyValuePair<string, bool> entry in modifiedTransparencies)
        {
            customAvatarHelper.setMaterialTransparent(entry.Key, entry.Value);
        }

        if (customAvatarHelper.getSyncShader())
        {
            int shaderIndex = customAvatarHelper.getShaderIndex();
            float glow = customAvatarHelper.getGlow();
            float ambient = customAvatarHelper.getAmbient();
            customAvatarHelper.setShaderIndex(EditorGUILayout.Popup("Shader", shaderIndex, customAvatarHelper.getShaderOptions()));
            customAvatarHelper.setGlow(EditorGUILayout.Slider("Glow", customAvatarHelper.getGlow(), 0, 1));
            if (shaderIndex == 0 || shaderIndex == 2)
            {
                customAvatarHelper.setAmbient(EditorGUILayout.Slider("Ambient", customAvatarHelper.getAmbient(), 0, 1));
            }

            if (syncShader != customAvatarHelper.getSyncShader() || shaderIndex != customAvatarHelper.getShaderIndex() || glow != customAvatarHelper.getGlow() || ambient != customAvatarHelper.getAmbient())
            {
                customAvatarHelper.setShaders();
            }
        }

        AssetDatabase.SaveAssets();
    }
}