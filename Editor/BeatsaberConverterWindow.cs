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
        private bool _autoResize = true;
    private Dictionary<string, Transform> _transforms = new Dictionary<string, Transform>();

    // UI Settings
    private Vector2 _scroll;

    private DynamicBonesController _dynamicBonesController = new DynamicBonesController();
    private CustomAvatarHelper _customAvatarHelper;

    [MenuItem ("Window/Beat Saber Converter")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(BeatSaberConvertorWindow), false, "Beat Saber Converter");
    }

    private void scaleModel()
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in _avatar.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            bounds.Encapsulate(skinnedMeshRenderer.bounds);
        }

        float scale = 2 / bounds.size.y;
        _avatar.gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void recurseDescendants(GameObject go)
    {
        foreach (Transform child in go.transform)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);
            recurseDescendants(child.gameObject);
        }
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
        _customAvatarHelper = (CustomAvatarHelper) avatarBase.gameObject.AddComponent<CustomAvatarHelper>();

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

        recurseDescendants(avatarBase.gameObject);
    }

    private async void setShaders()
    {
        Texture transparent = (Texture) AssetDatabase.LoadAssetAtPath("Assets/BeatSaberCustomAvatarUtils/Textures/Transparent.png", typeof(Texture));

        if(!AssetDatabase.IsValidFolder("Assets/Materials"))
        {    
            AssetDatabase.CreateFolder("Assets", "Materials");
        }

        foreach (Renderer renderer in _avatar.GetComponentsInChildren<Renderer>())
        {
            Material[] materials = renderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                Material material = materials[i];
                if (material.mainTexture == null )
                {
                    Material materialGen = new Material(Shader.Find("BeatSaber/Transparent"));
                    materialGen.SetTexture("_MainTex", transparent);
                    AssetDatabase.CreateAsset(materialGen, string.Format("Assets/Materials/{0}.mat", material.name));
                    materials[i] = materialGen;
                    _customAvatarHelper.addMaterial(materialGen, true);
                }
                else
                {
                    Material materialGen = new Material(Shader.Find("BeatSaber/Lit Glow"));
                    materialGen.SetTexture("_MainTex", material.mainTexture);
                    materialGen.SetTexture("_Tex", material.mainTexture);
                    materialGen.SetTextureOffset("_Tex", material.mainTextureOffset);
                    materialGen.SetTextureScale("_Tex", material.mainTextureScale);
                    materialGen.SetFloat("_Ambient", 0.05f);
                    materialGen.SetFloat("_Glow", 0.1f);
                    AssetDatabase.CreateAsset(materialGen, string.Format("Assets/Materials/{0}.mat", material.name));
                    materials[i] = materialGen;
                    _customAvatarHelper.addMaterial(materialGen, false);
                }
                AssetDatabase.SaveAssets();
            }
            renderer.sharedMaterials = materials;
        }
    }

    private void convertDynamicBones()
    {
        _dynamicBonesController.setAvatar(_avatar.gameObject);
        _dynamicBonesController.toPlaceholder();

        string[] guids = AssetDatabase.FindAssets("DynamicBone");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.Contains(".dll") && !path.Contains("BeatSaberCustomAvatarUtils"))
            {
                AssetDatabase.DeleteAsset(path);
            }
        }

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
        _autoResize = EditorGUILayout.Toggle("Auto Resize", _autoResize);

        EditorGUI.BeginDisabledGroup(_avatar == null);
        if (GUILayout.Button("Convert"))
        {
            if (_autoResize)
            {
                scaleModel();
            }
            convertDynamicBones();
            createStructure();
            setShaders();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndScrollView();
    }
}
