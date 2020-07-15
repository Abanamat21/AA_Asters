using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileController : MonoBehaviour, IPoolable
{
    public float flySpeed;
    private Vector3 lastPos;
    public GameObject source;
    private TimeSpan ignoreSourcePeriod = new TimeSpan(0, 0, 0, 0, 300);
    private DateTime created;
    
    void Update()
    {
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * flySpeed * Time.fixedDeltaTime);

        RaycastHit hit;
        if (Physics.Linecast(lastPos, transform.localPosition, out hit))
        {            
            IHitable hitable;
            hitable = (IHitable)hit.collider.gameObject.GetComponent<IHitable>();
            if (hitable != null && canHitIt(hit.collider.gameObject))
            {
                hitable.hit(gameObject, source);

                if((IWall)hit.collider.gameObject.GetComponent<IWall>() == null)
                    Despawn();
            }
        }

        lastPos = transform.localPosition;
    }

    private bool canHitIt(GameObject gameObject)
    {
        return (gameObject == source && created + ignoreSourcePeriod > DateTime.Now) 
                || gameObject != source;
    }

    public void Despawn()
    {
        GameController.instance.services.poolManager.Despawn(gameObject);
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
