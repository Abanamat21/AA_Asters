using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private static Loader instance { get; set; }
    public static Loader Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject InstantiateIt(GameObject gameObject, Vector3 position, Quaternion quaternion, GameObject parent)
    {
        return Instantiate(gameObject, position, quaternion, parent.transform);
    }
    public GameObject InstantiateIt(ProjectileToLoad projectile)
    {
        GameObject newProjectile = Instantiate(projectile.Prephab, projectile.Position, projectile.ObjectQuaternion, projectile.Parent.transform);
        ProjectileController projectileController = newProjectile.GetComponent<ProjectileController>();
        projectileController.Source = projectile.Source;
        return newProjectile;
    }

    public GameObject InstantiateIt(AsteroidToLoad asteroid)
    {
        GameObject newAsteroid = Instantiate(asteroid.Prephab, asteroid.Position, asteroid.ObjectQuaternion, asteroid.Parent.transform);
        AsteroidController asteroidController = newAsteroid.GetComponent<AsteroidController>();
        asteroidController.Size = asteroid.Size;
        asteroidController.FlySpeed = asteroid.Speed;
        return newAsteroid;
    }
}
