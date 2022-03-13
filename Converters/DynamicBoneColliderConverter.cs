using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBoneColliderConverter : MonoBehaviour
{
    public enum Direction
    {
        X, Y, Z
    }
    public Direction m_Direction = Direction.Y;
    public Vector3 m_Center = Vector3.zero;

    public enum Bound
    {
        Outside,
        Inside
    }
    public Bound m_Bound = Bound.Outside;

    public float m_Radius = 0.5f;
    public float m_Height = 0;
    public float m_Radius2 = 2;
}
