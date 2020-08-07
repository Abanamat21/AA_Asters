using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory : MonoBehaviour
{
    private static AsteroidFactory instance;
    public static AsteroidFactory Instance
    {
        get
        {
            if (instance == null) 
                instance = new AsteroidFactory();
            return instance;
        }
    }

    public GameObject DefalteParent;

    //private Dictionary<int, GameObject> prephabs;
    public List<GameObject> prephabsList;

    public float MinSpeed;
    public float MaxSpeed;

    public float SpawnMargin_Percents;
    public GameObject SpawnField;

    private Vector3 spawnScale;

    void Awake()
    {
        if (instance == null)
        {
            //instance = new AsteroidFactory();
            instance = this;
            spawnScale = SpawnField.transform.localScale;     
        }
    }
    //public AsteroidFactory()
    //{
    //    //prephabs = new Dictionary<int, GameObject>();
    //    //for (int i = 0; i < prephabsList.Count; i++)
    //    //{
    //    //    prephabs.Add(i + 1, prephabsList[i]);
    //    //}
    //    instance = this;
    //    spawnScale = spawnField.transform.localScale;
    //}

    public AsteroidToLoad createAster(Vector3 spawnPosition, Quaternion quaternion, float speed, int size = 0)
    {
        //if (prephabs == null || prephabs.Count == 0) throw new AAExceptions.ImportentComponentNotFound("prephabs was not set in AsteroidFactory");
        if (prephabsList == null || prephabsList.Count == 0) throw new AAExceptions.ImportentComponentNotFound("prephabs was not set in AsteroidFactory");
        Vector3 spawnPlace = spawnPosition;
        Quaternion spawnQuaternion = quaternion;
        GameObject aster;
        if(size == 0) return createAster(Random.Range(1, prephabsList.Count));
        aster = prephabsList[size - 1];
        if (spawnPlace == Vector3.zero && spawnQuaternion == Quaternion.Euler(0, 0, 0))
        {
            (spawnPlace, spawnQuaternion) = generateSpawnPosition(aster.GetComponent<SphereCollider>().radius);
        }

        return new AsteroidToLoad(aster, size, spawnPlace, spawnQuaternion, DefalteParent, speed);
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

        WallType wallType = (WallType)Random.Range(1, 5);
        switch (wallType)
        {
            case WallType.top:
                x = Random.Range(leftLimit, rightLimit);
                y = topLimit;
                rot = Random.Range(90 + rotMargin, 270 - rotMargin);
                break;
            case WallType.bottom:
                x = Random.Range(leftLimit, rightLimit);
                y = bottomLimit;
                rot = Random.Range(90 - rotMargin, -90 + rotMargin);                    
                break;
            case WallType.right:
                x = leftLimit;
                y = Random.Range(bottomLimit, topLimit);
                rot = Random.Range(180 + rotMargin, 360 - rotMargin);
                break;
            case WallType.left:
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
        return Random.Range(MinSpeed, MaxSpeed);
    }
}
