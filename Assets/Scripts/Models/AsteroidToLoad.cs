using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidToLoad
{
    public GameObject prephab;
    public int size;
    public Vector3 position;
    public Quaternion quaternion;
    public GameObject parent;
    public float speed;

    public AsteroidToLoad(GameObject _prephab, int _size, Vector3 _position, Quaternion _quaternion, GameObject _parent, float _speed)
    {
        prephab = _prephab;
        size = _size;
        position = _position;
        quaternion = _quaternion;
        parent = _parent;
        speed = _speed;
    }
}
