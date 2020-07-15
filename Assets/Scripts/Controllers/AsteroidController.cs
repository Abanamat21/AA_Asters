using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IHitable, IPoolable
{
    private float flySpeed;
    public int size;
    private Vector3 lastPos;

    public float destructionAward;

    private void OnCollisionEnter(Collision collision)
    {
        IStrikable strikable = collision.gameObject.GetComponent<IStrikable>();
        if (strikable != null)
        {
            strikable.Strike(gameObject);
            Despawn();
        }
    }
    void Start()
    {
        lastPos = transform.localPosition;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * flySpeed * Time.fixedDeltaTime);

        RaycastHit hit;
        if (Physics.Linecast(lastPos, transform.localPosition, out hit))
        {
            IWall wall = (IWall)hit.collider.gameObject.GetComponent<IWall>();
            if (wall != null)
            {
                wall.warpIt(gameObject);
            }
        }

        lastPos = transform.localPosition;
    }

    public void hit(GameObject projectile, GameObject causer)
    {
        Split();
    }

    public void setFlySpeed(float speed)
    {
        flySpeed = speed;
    }
    public void setAsterSize(int _size)
    {
        size = _size;
    }
    public void Split()
    {
        List<AsteroidToLoad> asteroidsToLoad = GameController.instance.services.asteroidFactory.afterDestroitCreation(transform.localPosition, transform.rotation, size);
        if(asteroidsToLoad != null && asteroidsToLoad.Count > 0)
            asteroidsToLoad.ForEach(x => GameController.instance.services.poolManager.Spawn(x));
        Despawn();
    }
    public void Despawn()
    {
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);        
        GameController.instance.services.poolManager.Despawn(gameObject);
    }

    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
        GameController.instance.incrementScore(destructionAward);
    }
}
