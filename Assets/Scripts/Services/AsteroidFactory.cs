using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory
{
    private GameObject defalteParent;

    Dictionary<int, GameObject> prephabs;

    private float minSpeed;
    private float maxSpeed;

    private Vector3 spawnScale;

    public AsteroidFactory(GameObject _defalteParent, Dictionary<int, GameObject> _prephabs, Vector3 _spawnScale, float _minSpeed, float _maxSpeed)
    {
        defalteParent = _defalteParent;
        prephabs = _prephabs;
        spawnScale = _spawnScale;
        minSpeed = _minSpeed;
        maxSpeed = _maxSpeed;
    }


    //TODO привязать к префабам!
    public AsteroidToLoad createAster(Vector3 spawnPosition, Quaternion quaternion, float speed, int size = 0)
    {
        if (prephabs == null || prephabs.Count == 0) throw new AAExceptions.ImportentComponentNotFound("prephabs was not set in AsteroidFactory");
        Vector3 spawnPlace = spawnPosition;
        Quaternion spawnQuaternion = quaternion;
        GameObject aster;
        if(size == 0) return createAster(Random.Range(1, prephabs.Count));
        aster = prephabs[size];
        if (spawnPlace == Vector3.zero && spawnQuaternion == Quaternion.Euler(0, 0, 0))
        {
            (spawnPlace, spawnQuaternion) = generateSpawnPosition(aster.GetComponent<SphereCollider>().radius);
        }

        return new AsteroidToLoad(aster, size, spawnPlace, spawnQuaternion, defalteParent, speed);
    }
    public AsteroidToLoad createAster(Vector3 spawnPosition, Quaternion quaternion, int size = 0)
    {
        return createAster(spawnPosition, quaternion, generateSpeed(), size);
    }
    public AsteroidToLoad createAster(int size = 0)
    {
        Vector3 retPos = Vector3.zero;
        Quaternion retQ = Quaternion.Euler(0, 0, 0);
        return createAster(retPos, retQ, generateSpeed(), size);
    }    

    public List<AsteroidToLoad> afterDestroitCreation(Vector3 oldPosition, Quaternion oldQuaternion, int oldAsteroidSize)
    {
        List<AsteroidToLoad> ret = new List<AsteroidToLoad>();
        int newAsteroidSize = oldAsteroidSize - 1;
        if (newAsteroidSize > 0)
        {
            float newSpeed = generateSpeed();
            ret.Add(createAster(oldPosition, oldQuaternion * Quaternion.Euler(0, 45, 0), newSpeed, newAsteroidSize));
            ret.Add(createAster(oldPosition, oldQuaternion * Quaternion.Euler(0, -45, 0), newSpeed, newAsteroidSize));
        }
        return ret;
    }
    public (Vector3, Quaternion) generateSpawnPosition(float asterRadius)
    {
        float spawnMargin = 5f;
        float rotMargin = 10f;
        Vector3 retPos;
        Quaternion retQ;

        float leftLimit = asterRadius + spawnMargin;
        float rightLimit = spawnScale.x - asterRadius - spawnMargin;
        float topLimit = spawnScale.y - asterRadius - spawnMargin;
        float bottomLimit = asterRadius + spawnMargin;


        float x, y;
        float rot;

        int wallChous = Random.Range(1, 5);
        switch (wallChous)
        {
            case 1: //top
                x = Random.Range(leftLimit, rightLimit);
                y = topLimit;
                rot = Random.Range(90 + rotMargin, 270 - rotMargin);
                break;
            case 2: //bottom
                x = Random.Range(leftLimit, rightLimit);
                y = bottomLimit;
                rot = Random.Range(90 - rotMargin, -90 + rotMargin);                    
                break;
            case 3: //left
                x = leftLimit;
                y = Random.Range(bottomLimit, topLimit);
                rot = Random.Range(180 + rotMargin, 360 - rotMargin);
                break;
            case 4: //right
                x = rightLimit;
                y = Random.Range(bottomLimit, topLimit);
                rot = Random.Range(0 + rotMargin, 90 - rotMargin);
                break;
            default:
                goto case 1;
        }
        retPos = new Vector3(x, y, 0);
        retQ = Quaternion.Euler(rot, -90, 90);

        return (retPos, retQ);
    }

    public float generateSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }
}
