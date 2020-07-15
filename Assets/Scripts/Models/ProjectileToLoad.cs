using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileToLoad
{
    public GameObject prephab;
    public Vector3 position;
    public Quaternion quaternion;
    public GameObject parent;
    public GameObject source;


    public ProjectileToLoad(GameObject _prephab, Vector3 _position, Quaternion _quaternion, GameObject _parent, GameObject _source)
    {
        prephab = _prephab;
        position = _position;
        quaternion = _quaternion;
        parent = _parent;
        source = _source;
    }
}
