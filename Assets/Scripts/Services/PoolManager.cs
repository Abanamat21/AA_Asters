using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private Dictionary<int, Stack<GameObject>> pools = new Dictionary<int, Stack<GameObject>>();
    private Dictionary<int, int> allPoolableIDs = new Dictionary<int, int>();

    delegate GameObject loadAction();

    public GameObject Spawn(GameObject prephab, Vector3 position, Quaternion rotation, GameObject parent)
    {
        return Spawn(prephab, position, rotation, parent, 
            delegate
            {
                return GameController.instance.services.loader.InstantiateIt(prephab, position, rotation, parent);
            });
    }

    public GameObject Spawn(AsteroidToLoad asteroid)
    {
        return Spawn(asteroid.prephab, asteroid.position, asteroid.quaternion, asteroid.parent,
            delegate
            {
                return GameController.instance.services.loader.InstantiateIt(asteroid);
            });
    }

    public GameObject Spawn(ProjectileToLoad projectile)
    {
        return Spawn(projectile.prephab, projectile.position, projectile.quaternion, projectile.parent,
            delegate
            {
                return GameController.instance.services.loader.InstantiateIt(projectile);
            });
    }

    private GameObject Spawn(GameObject prephab, Vector3 position, Quaternion rotation, GameObject parent, loadAction loadFunc)
    {
        int key = prephab.GetInstanceID();
        Stack<GameObject> pool;
        bool stacked = pools.TryGetValue(key, out pool);
        if (!stacked) pools.Add(key, new Stack<GameObject>());
        GameObject gameObject;

        if (stacked && pool.Count > 0)
        {
            gameObject = pool.Pop();
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.rotation = rotation;
            gameObject.transform.localPosition = position;
            gameObject.transform.gameObject.SetActive(true);
        }
        else
        {
            gameObject = loadFunc.Invoke();
            allPoolableIDs.Add(gameObject.GetInstanceID(), key);
        }

        IPoolable poolable = gameObject.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnSpawn();

        return gameObject;

    }

    public void Despawn(GameObject gameObject)
    {
        int id = allPoolableIDs[gameObject.GetInstanceID()];
        gameObject.SetActive(false);
        pools[id].Push(gameObject);
        IPoolable poolable = gameObject.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnDespawn();
    }

}
