using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidToLoad
{
    public GameObject Prephab;
    public int Size;
    public Vector3 Position;
    public Quaternion ObjectQuaternion;
    public GameObject Parent;
    public float Speed;

    public AsteroidToLoad(GameObject prephab, int size, Vector3 position, Quaternion quaternion, GameObject parent, float speed)
    {
        Prephab = prephab;
        Size = size;
        Position = position;
        ObjectQuaternion = quaternion;
        Parent = parent;
        Speed = speed;
    }
}
