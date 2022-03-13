using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBoneConverter : MonoBehaviour
{
    public Transform m_Root = null;
    public List<Transform> m_Roots = null;
    public float m_UpdateRate = 60.0f;

    public enum UpdateMode
    {
        Normal,
        AnimatePhysics,
        UnscaledTime,
        Default
    }
    public UpdateMode m_UpdateMode = UpdateMode.Default;

    public float m_Damping = 0.1f;
    public AnimationCurve m_DampingDistrib = null;

    public float m_Elasticity = 0.1f;
    public AnimationCurve m_ElasticityDistrib = null;

    public float m_Stiffness = 0.1f;
    public AnimationCurve m_StiffnessDistrib = null;

    public float m_Friction = 0;
    public AnimationCurve m_FrictionDistrib = null;

    public float m_Radius = 0;
    public AnimationCurve m_RadiusDistrib = null;

    public float m_EndLength = 0;
    public Vector3 m_EndOffset = Vector3.zero;

    public Vector3 m_Gravity = Vector3.zero;
    public Vector3 m_Force = Vector3.zero;

    public float m_BlendWeight = 1.0f;

    public List<DynamicBoneColliderConverter> m_Colliders = null;
    public List<Transform> m_Exclusions = null;

    public enum FreezeAxis
    {
        None, X, Y, Z
    }
    public FreezeAxis m_FreezeAxis = FreezeAxis.None;

    public bool m_DistantDisable = false;
    public Transform m_ReferenceObject = null;
    public float  m_DistanceToObject = 20;
}
