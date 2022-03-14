using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CustomAvatar;

[ExecuteInEditMode]
public class BeatSaberConvertorWindow : EditorWindow
{
    // Choose the avatar
    private Animator _avatar;
    private Dictionary<string, Transform> _transforms = new Dictionary<string, Transform>();

    // UI Settings
    private Vector2 _scroll;

    private DynamicBonesController _dynamicBonesController = new DynamicBonesController();

    [MenuItem ("Window/Beat Saber Converter")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(BeatSaberConvertorWindow), false, "Beat Saber Converter");
    }

    private void createStructure()
    {
        GameObject avatarBase = new GameObject();
        avatarBase.name = _avatar.gameObject.name;
        _avatar.gameObject.transform.position = new Vector3(0, 0, 0);
        _avatar.gameObject.transform.parent = avatarBase.transform;
        _avatar.runtimeAnimatorController = null;

        AvatarDescriptor avatarDescriptor = avatarBase.AddComponent<AvatarDescriptor>();
        VRIKManager vrikManager = _avatar.gameObject.AddComponent<VRIKManager>();
        vrikManager.solver_locomotion_footDistance = 0.15f;
        vrikManager.solver_locomotion_stepThreshold = 0.2f;
        vrikManager.solver_locomotion_stepSpeed = 1.5f;
        avatarBase.gameObject.AddComponent<CustomAvatarHelper>();

        GameObject body = new GameObject();
        body.name = "Body";
        body.transform.parent= avatarBase.transform;

        Dictionary<string, Transform> transforms = new Dictionary<string, Transform>()
        {
            ["Head"] = _avatar.GetBoneTransform(HumanBodyBones.Head),
            ["LeftHand"] = _avatar.GetBoneTransform(HumanBodyBones.LeftHand),
            ["RightHand"] = _avatar.GetBoneTransform(HumanBodyBones.RightHand),
            ["Pelvis"] = _avatar.GetBoneTransform(HumanBodyBones.Hips),
            ["LeftLeg"] = _avatar.GetBoneTransform(HumanBodyBones.LeftLowerLeg),
            ["RightLeg"] = _avatar.GetBoneTransform(HumanBodyBones.RightLowerLeg),
        };

        foreach (KeyValuePair<string, Transform> entry in transforms)
        {
            Transform clone = UnityEngine.Object.Instantiate(body.transform, entry.Value.position, entry.Value.rotation);
            clone.parent = avatarBase.transform;
            clone.name = entry.Key;
            if (entry.Key == "LeftHand")
            {
                clone.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.right);
            }
            else if (entry.Key == "RightHand")
            {
                clone.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.left);
            }
            else
            {
                clone.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            _transforms[entry.Key] = clone;
        }

        for(int i = 0; i < avatarBase.transform.childCount; i++)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(avatarBase.transform.GetChild(i).gameObject);
        }
    }

    private void setShaders()
    {
        Texture transparent = (Texture) AssetDatabase.LoadAssetAtPath("Assets/BeatSaberCustomAvatarUtils/Textures/Transparent.png", typeof(Texture));

        foreach (Renderer renderer in _avatar.GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in renderer.sharedMaterials)
            {
                if (material.mainTexture == null)
                {
                    material.shader = Shader.Find("BeatSaber/Transparent");
                    material.SetTexture("_MainTex", transparent);
                }
                else
                {
                    Texture texture = material.mainTexture;
                    Vector2 offset = material.mainTextureOffset;
                    Vector2 scale = material.mainTextureScale;
                    material.shader = Shader.Find("BeatSaber/Lit Glow");
                    material.SetTexture("_Tex", texture);
                    material.SetTextureOffset("_Tex", offset);
                    material.SetTextureScale("_Tex", scale);
                    material.SetFloat("_Ambient", 0.05f);
                    material.SetFloat("_Glow", 0.1f);
                }
                AssetDatabase.SaveAssets();
            }
        }
    }

    private void convertDynamicBones()
    {
        _dynamicBonesController.setAvatar(_avatar.gameObject);
        _dynamicBonesController.toPlaceholder();
        _dynamicBonesController.removeDynamicBonesLibrary();
        _dynamicBonesController.toDynamicBones();
    }

    void OnGUI()
    {
        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        if (_avatar == null)
        {
            _avatar = (Animator) FindObjectOfType(typeof(Animator));
        }
        _avatar = (Animator) EditorGUILayout.ObjectField("Avatar", _avatar, typeof(Animator), true);

        EditorGUI.BeginDisabledGroup(_avatar == null);
        if (GUILayout.Button("Convert"))
        {
            convertDynamicBones();
            createStructure();
            setShaders();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndScrollView();
    }
}
