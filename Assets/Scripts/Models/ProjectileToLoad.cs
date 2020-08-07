using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileToLoad
{
    public GameObject Prephab;
    public Vector3 Position;
    public Quaternion ObjectQuaternion;
    public GameObject Parent;
    public GameObject Source;


    public ProjectileToLoad(GameObject prephab, Vector3 position, Quaternion quaternion, GameObject parent, GameObject source)
    {
        Prephab = prephab;
        Position = position;
        ObjectQuaternion = quaternion;
        Parent = parent;
        Source = source;
    }
}
