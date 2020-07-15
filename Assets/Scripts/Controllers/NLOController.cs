using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NLOController : MonoBehaviour, IHitable, IStrikable, IPoolable
{
    public float speed;
    public bool flyDirection;
    public GameObject projectile;
    public float cooldownMin_Sec;
    public float cooldownMax_Sec;
    public float destructionAward;
    private GameObject player;
    public float spawnPeriodMin_Sec;
    public float spawnPeriodMax_Sec;
    private Vector3 flyVector;
    private Vector3 lastPos;

    void Start()
    {
        lastPos = transform.localPosition;
        GameFieldController gameFieldController = transform.parent.gameObject.GetComponent<GameFieldController>();
        if (gameFieldController != null)
            player = gameFieldController.player;
        else
            throw new AAExceptions.ImportentComponentNotFound("GameFieldController is not founded in NLOControllers parent");
        flyVector = (flyDirection ? Vector3.right : Vector3.left).normalized * speed;

        StartCoroutine(CoroutineShooting());
    }

    void FixedUpdate()
    {
        if (flyVector != null)
        {
            transform.Translate(flyVector * Time.fixedDeltaTime);
        }

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

    private IEnumerator CoroutineShooting()
    {
        yield return new WaitForSeconds(Random.Range(cooldownMin_Sec, cooldownMax_Sec));
        Vector3 from = projectile.transform.forward;
        Vector3 to = player.transform.localPosition - transform.localPosition;
        Shoot(Quaternion.FromToRotation(from, to));
        StartCoroutine(CoroutineShooting());        
    }

    private void Shoot(Quaternion quaternion)
    {
        Vector3 spuwnPlace = transform.localPosition;
        GameController.instance.services.poolManager.Spawn(new ProjectileToLoad(projectile, spuwnPlace, quaternion, gameObject.transform.parent.gameObject, gameObject));
    }

    public void hit(GameObject projectile, GameObject causer)
    {
        GameController.instance.services.gameFieldController.StartCoroutineNLO(Random.Range(spawnPeriodMin_Sec, spawnPeriodMax_Sec));
        Despawn();
    }

    public void Strike(GameObject causer)
    {
        GameController.instance.services.gameFieldController.StartCoroutineNLO(Random.Range(spawnPeriodMin_Sec, spawnPeriodMax_Sec));
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
