using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GraspController
{
    private Animator _avatar;
    private bool _savedBoneTransforms = false;
    private Dictionary<HumanBodyBones, Vector3> _referenceBones = new Dictionary<HumanBodyBones, Vector3>();

    enum rotationAxis {
        x,
        y,
        z,
        xneg,
        yneg,
        zneg,
    };

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

        _savedBoneTransforms = true;
    }

    public bool getSavedBoneTransforms()
    {
        return _savedBoneTransforms;
    }

    public void curlHand(float curlThumb, float curlFinger, bool left)
    {
        if (_referenceBones.Count == 0)
            return;

        curlHandLeft(curlThumb, left ? curlFinger : curlFinger);
        curlHandRight(curlThumb, left ? curlFinger : curlFinger);
    }

    private rotationAxis rotateZWise(Transform transform)
    {
        transform.localPosition += new Vector3(1, 0, 0);
        Vector3 xreferencePosition = transform.position;
        transform.localPosition += new Vector3(-1, 1, 0);
        Vector3 yreferencePosition = transform.position;
        transform.localPosition += new Vector3(0, -1, 1);
        Vector3 zreferencePosition = transform.position;
        transform.localPosition += new Vector3(0, 0, -1);

        xreferencePosition -= transform.position;
        yreferencePosition -= transform.position;
        zreferencePosition -= transform.position;

        float xzdif = 1 - Math.Abs(xreferencePosition.z);
        bool xpos = xreferencePosition.z > 0;
        float yzdif = 1 - Math.Abs(yreferencePosition.z);
        bool ypos = yreferencePosition.z > 0;
        float zzdif = 1 - Math.Abs(zreferencePosition.z);
        bool zpos = zreferencePosition.z > 0;

        Vector3 graspRotation;
        if (xzdif < yzdif)
        {
            if (xzdif < zzdif)
            {
                if (xpos)
                {
                    return rotationAxis.x;
                }
                else
                {
                    return rotationAxis.xneg;
                }
            }
            else
            {
                if (ypos)
                {
                    return rotationAxis.y;
                }
                else
                {
                    return rotationAxis.yneg;
                }
            }
        }
        else
        {
            if (yzdif < zzdif)
            {
                if (ypos)
                {
                    return rotationAxis.y;
                }
                else
                {
                    return rotationAxis.yneg;
                }
            }
            else
            {
                if (zpos)
                {
                    return rotationAxis.z;
                }
                else
                {
                    return rotationAxis.zneg;
                }
            }
        }
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
                rotationAxis rotationAxisResult = rotateZWise(transform);
                Vector3 localRotationModified = localRotation;
                switch (rotationAxisResult)
                {
                    case rotationAxis.x:
                        localRotationModified = new Vector3(localRotation.x + curlFinger, localRotation.y, localRotation.z);
                        break;
                    case rotationAxis.xneg:
                        localRotationModified = new Vector3(localRotation.x - curlFinger, localRotation.y, localRotation.z);
                        break;
                    case rotationAxis.y:
                        localRotationModified = new Vector3(localRotation.x, localRotation.y + curlFinger, localRotation.z);
                        break;
                    case rotationAxis.yneg:
                        localRotationModified = new Vector3(localRotation.x, localRotation.y - curlFinger, localRotation.z);
                        break;
                    case rotationAxis.z:
                        localRotationModified = new Vector3(localRotation.x, localRotation.y, localRotation.z + curlFinger);
                        break;
                    case rotationAxis.zneg:
                        localRotationModified = new Vector3(localRotation.x, localRotation.y, localRotation.z - curlFinger);
                        break;
                }
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
                Debug.Log("Right");
                rotationAxis rotationAxisResult = rotateZWise(transform);
                Vector3 localRotationModified = localRotation;
                Debug.Log(rotationAxisResult);
                switch (rotationAxisResult)
                {
                    case rotationAxis.x:
                        localRotationModified = new Vector3(localRotation.x - curlFinger, localRotation.y, localRotation.z);
                        break;
                    case rotationAxis.xneg:
                        localRotationModified = new Vector3(localRotation.x + curlFinger, localRotation.y, localRotation.z);
                        break;
                    case rotationAxis.y:
                        localRotationModified = new Vector3(localRotation.x, localRotation.y - curlFinger, localRotation.z);
                        break;
                    case rotationAxis.yneg:
                        localRotationModified = new Vector3(localRotation.x, localRotation.y + curlFinger, localRotation.z);
                        break;
                    case rotationAxis.z:
                        localRotationModified = new Vector3(localRotation.x, localRotation.y, localRotation.z - curlFinger);
                        break;
                    case rotationAxis.zneg:
                        localRotationModified = new Vector3(localRotation.x, localRotation.y, localRotation.z + curlFinger);
                        break;
                }
                Quaternion quaternion = new Quaternion();
                quaternion.eulerAngles = localRotationModified;
                transform.localRotation = quaternion;
            }
        }
    }
}
