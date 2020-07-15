using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject InstantiateIt(GameObject gameObject, Vector3 position, Quaternion quaternion, GameObject parent)
    {
        return Instantiate(gameObject, position, quaternion, parent.transform);
    }
    public GameObject InstantiateIt(ProjectileToLoad projectile)
    {
        GameObject newProjectile = Instantiate(projectile.prephab, projectile.position, projectile.quaternion, projectile.parent.transform);
        ProjectileController projectileController = newProjectile.GetComponent<ProjectileController>();
        projectileController.source = projectile.source;
        return newProjectile;
    }

    public GameObject InstantiateIt(AsteroidToLoad asteroid)
    {
        GameObject newAsteroid = Instantiate(asteroid.prephab, asteroid.position, asteroid.quaternion, asteroid.parent.transform);
        AsteroidController asteroidController = newAsteroid.GetComponent<AsteroidController>();
        if (asteroid.size != 0) asteroidController.setAsterSize(asteroid.size);
        asteroidController.setFlySpeed(asteroid.speed);
        return newAsteroid;
    }
}
