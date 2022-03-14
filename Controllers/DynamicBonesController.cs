using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DynamicBonesController
{
    private GameObject _avatar;
    private Dictionary<DynamicBoneCollider, DynamicBoneColliderConverter> _dynamicBoneColliderMap = new Dictionary<DynamicBoneCollider, DynamicBoneColliderConverter>();
    private Dictionary<DynamicBoneColliderConverter, DynamicBoneCollider> _dynamicBoneColliderConverterMap = new Dictionary<DynamicBoneColliderConverter, DynamicBoneCollider>();
    private Dictionary<DynamicBonePlaneCollider, DynamicBonePlaneColliderConverter> _dynamicBonePlaneColliderMap = new Dictionary<DynamicBonePlaneCollider, DynamicBonePlaneColliderConverter>();
    private Dictionary<DynamicBonePlaneColliderConverter, DynamicBonePlaneCollider> _dynamicBonePlaneColliderConverterMap = new Dictionary<DynamicBonePlaneColliderConverter, DynamicBonePlaneCollider>();

    public void setAvatar(GameObject avatar)
    {
        _avatar = avatar;
    }
    
    private void removeDynamicBoneColliders()
    {
        DynamicBoneCollider[] colliders = _avatar.GetComponentsInChildren<DynamicBoneCollider>();
        if (colliders != null)
        {
            foreach (DynamicBoneCollider collider in colliders)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }
    }

    private void removeDynamicBonePlaneColliders()
    {
        DynamicBonePlaneCollider[] colliders = _avatar.GetComponentsInChildren<DynamicBonePlaneCollider>();
        if (colliders != null)
        {
            foreach (DynamicBonePlaneCollider collider in colliders)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }
    }

    private void removeDynamicBones()
    {
        DynamicBone[] dynamicBones = _avatar.GetComponentsInChildren<DynamicBone>();
        if (dynamicBones != null)
        {
            foreach (DynamicBone dynamicBone in dynamicBones)
            {
                UnityEngine.Object.DestroyImmediate(dynamicBone);
            }
        }
    }

    private void removeDynamicBoneColliderConverters()
    {
        DynamicBoneColliderConverter[] colliders = _avatar.GetComponentsInChildren<DynamicBoneColliderConverter>();
        if (colliders != null)
        {
            foreach (DynamicBoneColliderConverter collider in colliders)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }
    }

    private void removeDynamicBonePlaneColliderConverters()
    {
        DynamicBonePlaneColliderConverter[] colliders = _avatar.GetComponentsInChildren<DynamicBonePlaneColliderConverter>();
        if (colliders != null)
        {
            foreach (DynamicBonePlaneColliderConverter collider in colliders)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }
    }

    private void removeDynamicBoneConverters()
    {
        DynamicBoneConverter[] dynamicBones = _avatar.GetComponentsInChildren<DynamicBoneConverter>();
        if (dynamicBones != null)
        {
            foreach (DynamicBoneConverter dynamicBone in dynamicBones)
            {
                UnityEngine.Object.DestroyImmediate(dynamicBone);
            }
        }
    }

    private void convertFromDynamicBoneColliders()
    {
        DynamicBoneCollider[] colliders = _avatar.GetComponentsInChildren<DynamicBoneCollider>();
        if (colliders != null)
        {
            foreach (DynamicBoneCollider collider in colliders)
            {
                DynamicBoneColliderConverter converter = collider.gameObject.AddComponent<DynamicBoneColliderConverter>() as DynamicBoneColliderConverter;
                converter.m_Direction = (DynamicBoneColliderConverter.Direction) Enum.Parse(typeof(DynamicBoneColliderConverter.Direction), collider.m_Direction.ToString());
                converter.m_Center = collider.m_Center;

                converter.m_Bound = (DynamicBoneColliderConverter.Bound) Enum.Parse(typeof(DynamicBoneColliderConverter.Bound), collider.m_Bound.ToString());
                converter.m_Radius = collider.m_Radius;
                converter.m_Height = collider.m_Height;

                _dynamicBoneColliderMap[collider] = converter;
            }
        }
    }

    private void convertFromDynamicBonePlaneColliders()
    {
        DynamicBonePlaneCollider[] colliders = _avatar.GetComponentsInChildren<DynamicBonePlaneCollider>();
        if (colliders != null)
        {
            foreach (DynamicBonePlaneCollider collider in colliders)
            {
                DynamicBonePlaneColliderConverter converter = collider.gameObject.AddComponent<DynamicBonePlaneColliderConverter>() as DynamicBonePlaneColliderConverter;
                converter.m_Direction = (DynamicBonePlaneColliderConverter.Direction) Enum.Parse(typeof(DynamicBonePlaneColliderConverter.Direction), collider.m_Direction.ToString());
                converter.m_Center = collider.m_Center;

                converter.m_Bound = (DynamicBonePlaneColliderConverter.Bound) Enum.Parse(typeof(DynamicBonePlaneColliderConverter.Bound), collider.m_Bound.ToString());

                _dynamicBonePlaneColliderMap[collider] = converter;
            }
        }
    }

    private void convertFromDynamicBones()
    {
        DynamicBone[] dynamicBones = _avatar.GetComponentsInChildren<DynamicBone>();
        if (dynamicBones != null)
        {
            foreach (DynamicBone dynamicBone in dynamicBones)
            {
                DynamicBoneConverter converter = dynamicBone.gameObject.AddComponent<DynamicBoneConverter>() as DynamicBoneConverter;
                converter.m_Root = dynamicBone.m_Root;
                converter.m_UpdateRate = dynamicBone.m_UpdateRate;

                converter.m_UpdateMode = (DynamicBoneConverter.UpdateMode) Enum.Parse(typeof(DynamicBoneConverter.UpdateMode), dynamicBone.m_UpdateMode.ToString());

                converter.m_Damping = dynamicBone.m_Damping;
                converter.m_DampingDistrib = dynamicBone.m_DampingDistrib;

                converter.m_Elasticity = dynamicBone.m_Elasticity;
                converter.m_ElasticityDistrib = dynamicBone.m_ElasticityDistrib;

                converter.m_Stiffness = dynamicBone.m_Stiffness;
                converter.m_StiffnessDistrib = dynamicBone.m_StiffnessDistrib;

                converter.m_Radius = dynamicBone.m_Radius;
                converter.m_RadiusDistrib = dynamicBone.m_RadiusDistrib;

                converter.m_EndLength = dynamicBone.m_EndLength;
                converter.m_EndOffset = dynamicBone.m_EndOffset;

                converter.m_Gravity = dynamicBone.m_Gravity;
                converter.m_Force = dynamicBone.m_Force;

                converter.m_Colliders = new List<DynamicBoneColliderConverter>();
                foreach (DynamicBoneCollider collider in dynamicBone.m_Colliders)
                {
                    converter.m_Colliders.Add(_dynamicBoneColliderMap[collider]);
                }
                converter.m_Exclusions = dynamicBone.m_Exclusions;

                converter.m_FreezeAxis = (DynamicBoneConverter.FreezeAxis) Enum.Parse(typeof(DynamicBoneConverter.FreezeAxis), dynamicBone.m_FreezeAxis.ToString());

                converter.m_DistantDisable = dynamicBone.m_DistantDisable;
                converter.m_ReferenceObject = dynamicBone.m_ReferenceObject;
                converter.m_DistanceToObject = dynamicBone.m_DistanceToObject;
            }
        }
    }

    private void convertToDynamicBoneColliders()
    {
        DynamicBoneColliderConverter[] converters = _avatar.GetComponentsInChildren<DynamicBoneColliderConverter>();
        if (converters != null)
        {
            foreach (DynamicBoneColliderConverter converter in converters)
            {
                DynamicBoneCollider collider = converter.gameObject.AddComponent<DynamicBoneCollider>() as DynamicBoneCollider;
                collider.m_Direction = (DynamicBoneCollider.Direction) Enum.Parse(typeof(DynamicBoneCollider.Direction), converter.m_Direction.ToString());
                collider.m_Center = converter.m_Center;

                collider.m_Bound = (DynamicBoneCollider.Bound) Enum.Parse(typeof(DynamicBoneCollider.Bound), converter.m_Bound.ToString());
                collider.m_Radius = converter.m_Radius;
                collider.m_Height = converter.m_Height;

                _dynamicBoneColliderConverterMap[converter] = collider;
            }
        }
    }

    private void convertToDynamicBonePlaneColliders()
    {
        DynamicBonePlaneColliderConverter[] converters = _avatar.GetComponentsInChildren<DynamicBonePlaneColliderConverter>();
        if (converters != null)
        {
            foreach (DynamicBonePlaneColliderConverter converter in converters)
            {
                DynamicBonePlaneCollider collider = converter.gameObject.AddComponent<DynamicBonePlaneCollider>() as DynamicBonePlaneCollider;
                collider.m_Direction = (DynamicBonePlaneCollider.Direction) Enum.Parse(typeof(DynamicBonePlaneCollider.Direction), converter.m_Direction.ToString());
                collider.m_Center = converter.m_Center;

                collider.m_Bound = (DynamicBonePlaneCollider.Bound) Enum.Parse(typeof(DynamicBonePlaneCollider.Bound), converter.m_Bound.ToString());

                _dynamicBonePlaneColliderConverterMap[converter] = collider;
            }
        }
    }

    private void convertToDynamicBones()
    {
        DynamicBoneConverter[] converters = _avatar.GetComponentsInChildren<DynamicBoneConverter>();

        if (converters != null)
        {
            foreach (DynamicBoneConverter converter in converters)
            {
                DynamicBone dynamicBone = converter.gameObject.AddComponent<DynamicBone>() as DynamicBone;
                dynamicBone.m_Root = converter.m_Root;
                dynamicBone.m_UpdateRate = converter.m_UpdateRate;

                dynamicBone.m_UpdateMode = (DynamicBone.UpdateMode) Enum.Parse(typeof(DynamicBone.UpdateMode), converter.m_UpdateMode.ToString());

                dynamicBone.m_Damping = converter.m_Damping;
                dynamicBone.m_DampingDistrib = converter.m_DampingDistrib;

                dynamicBone.m_Elasticity = converter.m_Elasticity;
                dynamicBone.m_ElasticityDistrib = converter.m_ElasticityDistrib;

                dynamicBone.m_Stiffness = converter.m_Stiffness;
                dynamicBone.m_StiffnessDistrib = converter.m_StiffnessDistrib;

                dynamicBone.m_Radius = converter.m_Radius;
                dynamicBone.m_RadiusDistrib = converter.m_RadiusDistrib;

                dynamicBone.m_EndLength = converter.m_EndLength;
                dynamicBone.m_EndOffset = converter.m_EndOffset;

                dynamicBone.m_Gravity = converter.m_Gravity;
                dynamicBone.m_Force = converter.m_Force;

                dynamicBone.m_Colliders = new List<DynamicBoneColliderBase>();
                foreach (DynamicBoneColliderConverter colliderConverter in converter.m_Colliders)
                {
                    dynamicBone.m_Colliders.Add(_dynamicBoneColliderConverterMap[colliderConverter]);
                }
                dynamicBone.m_Exclusions = converter.m_Exclusions;

                dynamicBone.m_FreezeAxis = (DynamicBone.FreezeAxis) Enum.Parse(typeof(DynamicBone.FreezeAxis), dynamicBone.m_FreezeAxis.ToString());

                dynamicBone.m_DistantDisable = converter.m_DistantDisable;
                dynamicBone.m_ReferenceObject = converter.m_ReferenceObject;
                dynamicBone.m_DistanceToObject = converter.m_DistanceToObject;
            }
        }
    }

    public void toPlaceholder()
    {
        convertFromDynamicBoneColliders();
        convertFromDynamicBonePlaneColliders();
        convertFromDynamicBones();
        _dynamicBoneColliderMap.Clear();
        _dynamicBonePlaneColliderMap.Clear();
        removeDynamicBones();
        removeDynamicBonePlaneColliders();
        removeDynamicBoneColliders();
    }

    public void toDynamicBones()
    {
        convertToDynamicBoneColliders();
        convertToDynamicBonePlaneColliders();
        convertToDynamicBones();
        _dynamicBoneColliderMap.Clear();
        _dynamicBonePlaneColliderConverterMap.Clear();
        removeDynamicBoneConverters();
        removeDynamicBonePlaneColliderConverters();
        removeDynamicBoneColliderConverters();
    }

    public void removeDynamicBonesLibrary()
    {
        string[] guids = AssetDatabase.FindAssets("DynamicBone");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.Contains(".dll") && !path.Contains("BeatSaberCustomAvatarUtils"))
            {
                AssetDatabase.DeleteAsset(path);
            }
        }
    }
}
