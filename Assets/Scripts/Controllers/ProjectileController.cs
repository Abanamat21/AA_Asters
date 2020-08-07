using UnityEngine;
using System;

public class ProjectileController : MonoBehaviour, IPoolable
{
    public float FlySpeed;
    private Vector3 lastPos;
    public GameObject Source;
    private TimeSpan ignoreSourcePeriod = new TimeSpan(0, 0, 0, 0, 300); //300 милисекунд
    private DateTime created;
    
    void Update()
    {
    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * FlySpeed * Time.fixedDeltaTime);

        RaycastHit hit;
        if (Physics.Linecast(lastPos, transform.localPosition, out hit))
        {            
            IHitable hitable;
            hitable = hit.collider.gameObject.GetComponent<IHitable>();
            if (hitable != null && canHitIt(hit.collider.gameObject))
            {
                hitable.Hit(gameObject, Source);

                if(hit.collider.gameObject.GetComponent<IWall>() == null)
                    Despawn();
            }
        }

        lastPos = transform.localPosition;
    }

    private bool canHitIt(GameObject gameObject)
    {
        return (gameObject == Source && created + ignoreSourcePeriod > DateTime.Now) 
                || gameObject != Source;
    }
    public void Despawn()
    {
        PoolManager.Instance.Despawn(gameObject);
    }
    public void OnSpawn()
    {
        lastPos = transform.localPosition;
        created = DateTime.Now;
        GetComponent<AudioSource>().Play();
    }
    public void OnDespawn()
    {
    }
}
