using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldController : MonoBehaviour
{
    private Dictionary<int, GameObject> asteroidPrephabs;
    public List<GameObject> asteroidPrephabsList;

    public GameObject NOLPrephab;
    public GameObject player;
    public GameObject ground;
    public Vector3 scale;

    public int asterroidsInWaveCount;
    private int curentAsteroidCount;

    public float minAsterSpeed;
    public float maxAsterSpeed;

    public float spawnMargin_Percents;

    private AsteroidFactory asteroidFactory;
    private PoolManager poolManager;

    void Awake()
    {
        Debug.Log("GameFieldController");
        asteroidPrephabs = new Dictionary<int, GameObject>();
        for (int i = 0; i < asteroidPrephabsList.Count; i++)
        {
            asteroidPrephabs.Add(i + 1, asteroidPrephabsList[i]);
        }

        if (GameController.instance.services.asteroidFactory == null)
            GameController.instance.services.asteroidFactory = new AsteroidFactory(gameObject, asteroidPrephabs, ground.transform.localScale, minAsterSpeed, maxAsterSpeed);
        if (GameController.instance.services.gameFieldController == null)
            GameController.instance.services.gameFieldController = this;
        asteroidFactory = GameController.instance.services.asteroidFactory;
        poolManager = GameController.instance.services.poolManager;
        scale = ground.transform.localScale;
    }

    void Start()
    {        
        //reset();
    }

    void Update()
    {
    }

    //хорошо бы в Factory запихать, но нет необходимости в вариативности создания НЛО
    private GameObject spawnNLO()
    {
        float NOLWidth = NOLPrephab.transform.localScale.x;

        float bottomLimit = ground.transform.localScale.y / 100 * spawnMargin_Percents;
        float topLimit = ground.transform.localScale.y - bottomLimit;
        float leftLimit = NOLWidth;
        float rightLimit = ground.transform.localScale.x - NOLWidth;
                
        Vector3 spawnPlace = new Vector3(0, 0, 0);
        spawnPlace.x = UnityEngine.Random.Range(leftLimit, rightLimit);
        spawnPlace.y = UnityEngine.Random.Range(bottomLimit, topLimit);

        GameObject newNLO = poolManager.Spawn(NOLPrephab, spawnPlace, Quaternion.Euler(0, 0, 0), gameObject);
        if(newNLO.TryGetComponent<NLOController>(out NLOController nloController))
        {
            nloController.flyDirection = UnityEngine.Random.Range(0, 2) == 0;
        }
        return newNLO;
    }
    public void reset()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.resetParameters();
            }
            if (child.TryGetComponent<AsteroidController>(out AsteroidController asteroid))
            {
                asteroid.Despawn();
            }
            if (child.TryGetComponent<ProjectileController>(out ProjectileController projectile))
            {
                projectile.Despawn();
            }
            if (child.TryGetComponent<NLOController>(out NLOController nlo))
            {
                nlo.Despawn();
            }
        }

        StopAllCoroutines();

        StartCoroutine(CoroutineFirstAsteroidWave());
        StartCoroutine(CoroutineNLO(1));
    }
    private IEnumerator CoroutineFirstAsteroidWave()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < asterroidsInWaveCount; i++)
        {
            poolManager.Spawn(asteroidFactory.createAster(asteroidPrephabs.Keys.Count));
        }
        StartCoroutine(CoroutineCheckNewAsteroidWave());
    }
    private IEnumerator CoroutineAsteroidWave(int count)
    {
        yield return new WaitForSeconds(2);
        for(int i = 0; i < count; i++)
        {
            poolManager.Spawn(asteroidFactory.createAster());
        }
        StartCoroutine(CoroutineCheckNewAsteroidWave());
    }
    private IEnumerator CoroutineCheckNewAsteroidWave()
    {
        yield return new WaitForSeconds(0.5f);
        curentAsteroidCount = getCurentCountByTag("asteroid");     
        if (curentAsteroidCount == 0)
        {
            asterroidsInWaveCount++;
            StartCoroutine(CoroutineAsteroidWave(asterroidsInWaveCount));
        }
        else
        {
            StartCoroutine(CoroutineCheckNewAsteroidWave());
        }
    }
    private IEnumerator CoroutineNLO(float wait)
    {
        yield return new WaitForSeconds(wait);
        spawnNLO();
    }
    public void StartCoroutineNLO(float wait)
    {
        StartCoroutine(CoroutineNLO(wait));
    }

    private int getCurentCountByTag(String tag)
    {
        int ret = 0;
        foreach(Transform child in transform)
        {
            if (child.gameObject.tag == tag && child.gameObject.activeInHierarchy)
                ret++;
        }
        return ret;
    }

}
