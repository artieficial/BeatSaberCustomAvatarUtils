using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GraspController
{
    private Animator _avatar;
    private Dictionary<HumanBodyBones, Vector3> _referenceBones = new Dictionary<HumanBodyBones, Vector3>();

    private HumanBodyBones[] _leftThumbBones = {
        HumanBodyBones.LeftThumbProximal,
        HumanBodyBones.LeftThumbIntermediate,
        HumanBodyBones.LeftThumbDistal,
    };

    private HumanBodyBones[] _leftFingerBones = {
        HumanBodyBones.LeftIndexProximal,
        HumanBodyBones.LeftIndexIntermediate,
        HumanBodyBones.LeftIndexDistal,
        HumanBodyBones.LeftMiddleProximal,
        HumanBodyBones.LeftMiddleIntermediate,
        HumanBodyBones.LeftMiddleDistal,
        HumanBodyBones.LeftRingProximal,
        HumanBodyBones.LeftRingIntermediate,
        HumanBodyBones.LeftRingDistal,
        HumanBodyBones.LeftLittleProximal,
        HumanBodyBones.LeftLittleIntermediate,
        HumanBodyBones.LeftLittleDistal,
    };

    private HumanBodyBones[] _rightThumbBones = {
        HumanBodyBones.RightThumbProximal,
        HumanBodyBones.RightThumbIntermediate,
        HumanBodyBones.RightThumbDistal,
    };

    private HumanBodyBones[] _rightFingerBones = {
        HumanBodyBones.RightIndexProximal,
        HumanBodyBones.RightIndexIntermediate,
        HumanBodyBones.RightIndexDistal,
        HumanBodyBones.RightMiddleProximal,
        HumanBodyBones.RightMiddleIntermediate,
        HumanBodyBones.RightMiddleDistal,
        HumanBodyBones.RightRingProximal,
        HumanBodyBones.RightRingIntermediate,
        HumanBodyBones.RightRingDistal,
        HumanBodyBones.RightLittleProximal,
        HumanBodyBones.RightLittleIntermediate,
        HumanBodyBones.RightLittleDistal,
    };

    public void setAvatar(Animator avatar)
    {
        _avatar = avatar;
    }

    public void saveBoneTransforms()
    {
        foreach (HumanBodyBones bone in _leftThumbBones)
        {
            Transform transform = _avatar.GetBoneTransform(bone);
            if (transform != null)
            {
                _referenceBones[bone] = transform.localRotation.eulerAngles;
            }
        }

        foreach (HumanBodyBones bone in _leftFingerBones)
        {
            Transform transform = _avatar.GetBoneTransform(bone);
            if (transform != null)
            {
                _referenceBones[bone] = transform.localRotation.eulerAngles;
            }
        }

        foreach (HumanBodyBones bone in _rightThumbBones)
        {
            Transform transform = _avatar.GetBoneTransform(bone);
            if (transform != null)
            {
                _referenceBones[bone] = transform.localRotation.eulerAngles;
            }
        }

        foreach (HumanBodyBones bone in _rightFingerBones)
        {
            Transform transform = _avatar.GetBoneTransform(bone);
            if (transform != null)
            {
                _referenceBones[bone] = transform.localRotation.eulerAngles;
            }
        }
    }

    public void curlHand(float curlThumb, float curlFinger)
    {
        if (_referenceBones.Count == 0)
            return;

        curlHandLeft(curlThumb, curlFinger);
        curlHandRight(curlThumb, curlFinger);
    }

    public void curlHandLeft(float curlThumb, float curlFinger)
    {
        if (_referenceBones.Count == 0)
            return;

        foreach (HumanBodyBones bone in _leftThumbBones)
        {
            Transform transform = _avatar.GetBoneTransform(bone);
            if (transform != null)
            {
                Vector3 localRotation = _referenceBones[bone];
                Vector3 localRotationModified = new Vector3(localRotation.x - curlThumb, localRotation.y, localRotation.z);
                Quaternion quaternion = new Quaternion();
                quaternion.eulerAngles = localRotationModified;
                transform.localRotation = quaternion;
            }
        }
        foreach (HumanBodyBones bone in _leftFingerBones)
        {
            Transform transform = _avatar.GetBoneTransform(bone);
            if (transform != null)
            {
                Vector3 localRotation = _referenceBones[bone];
                Vector3 localRotationModified = new Vector3(localRotation.x, localRotation.y, localRotation.z + curlFinger);
                Quaternion quaternion = new Quaternion();
                quaternion.eulerAngles = localRotationModified;
                transform.localRotation = quaternion;
            }
        }
    }

    public void curlHandRight(float curlThumb, float curlFinger)
    {
        if (_referenceBones.Count == 0)
            return;

        foreach (HumanBodyBones bone in _rightThumbBones)
        {
            Transform transform = _avatar.GetBoneTransform(bone);
            if (transform != null)
            {
                Vector3 localRotation = _referenceBones[bone];
                Vector3 localRotationModified = new Vector3(localRotation.x - curlThumb, localRotation.y, localRotation.z);
                Quaternion quaternion = new Quaternion();
                quaternion.eulerAngles = localRotationModified;
                transform.localRotation = quaternion;
            }
        }
        foreach (HumanBodyBones bone in _rightFingerBones)
        {
            Transform transform = _avatar.GetBoneTransform(bone);
            if (transform != null)
            {
                Vector3 localRotation = _referenceBones[bone];
                Vector3 localRotationModified = new Vector3(localRotation.x, localRotation.y, localRotation.z - curlFinger);
                Quaternion quaternion = new Quaternion();
                quaternion.eulerAngles = localRotationModified;
                transform.localRotation = quaternion;
            }
        }
    }
}
