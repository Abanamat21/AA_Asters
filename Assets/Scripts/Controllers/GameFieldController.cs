using System;
using System.Collections;
using UnityEngine;

public class GameFieldController : MonoBehaviour
{
    #region Поля
    public GameObject NOLPrephab;
    public GameObject Player;
    public GameObject Ground;
    public Vector3 Scale { private set; get; }

    public int AsterroidsInWaveCount;
    private int curentAsteroidCount;

    public float SpawnMargin_Percents;
    #endregion

    #region Служебные методы
    void Awake()
    {
        Debug.Log("GameFieldController");

        Scale = Ground.transform.localScale;
    }
    void Start()
    {
        //reset();
    }
    void Update()
    {
    }
    #endregion

    #region Публичные методы
    public void Reset()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.ResetParameters();
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

        StartCoroutine(coroutineFirstAsteroidWave());
        StartCoroutine(coroutineNLOWave(1));
    }
    //хорошо бы в Factory запихать, но нет необходимости в вариативности создания НЛО
    public void StartCoroutineNLOWave(float wait)
    {
        StartCoroutine(coroutineNLOWave(wait));
    }
    #endregion

    #region Внутренние методы
    private IEnumerator coroutineFirstAsteroidWave()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < AsterroidsInWaveCount; i++)
        {
            PoolManager.Instance.Spawn(AsteroidFactory.Instance.createAster(AsteroidFactory.Instance.prephabsList.Count));
        }
        StartCoroutine(coroutineCheckNewAsteroidWave());
    }
    private IEnumerator coroutineAsteroidWave(int count)
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < count; i++)
        {
            PoolManager.Instance.Spawn(AsteroidFactory.Instance.createAster());
        }
        StartCoroutine(coroutineCheckNewAsteroidWave());
    }
    private IEnumerator coroutineCheckNewAsteroidWave()
    {
        yield return new WaitForSeconds(0.5f);
        curentAsteroidCount = getCurentCountByTag("asteroid");
        if (curentAsteroidCount == 0)
        {
            AsterroidsInWaveCount++;
            StartCoroutine(coroutineAsteroidWave(AsterroidsInWaveCount));
        }
        else
        {
            StartCoroutine(coroutineCheckNewAsteroidWave());
        }
    }
    private IEnumerator coroutineNLOWave(float wait)
    {
        yield return new WaitForSeconds(wait);
        spawnNLO();
    }
    private GameObject spawnNLO()
    {
        float NOLWidth = NOLPrephab.transform.localScale.x;

        float bottomLimit = Ground.transform.localScale.y / 100 * SpawnMargin_Percents;
        float topLimit = Ground.transform.localScale.y - bottomLimit;
        float leftLimit = NOLWidth;
        float rightLimit = Ground.transform.localScale.x - NOLWidth;

        Vector3 spawnPlace = new Vector3(0, 0, 0);
        spawnPlace.x = UnityEngine.Random.Range(leftLimit, rightLimit);
        spawnPlace.y = UnityEngine.Random.Range(bottomLimit, topLimit);

        GameObject newNLO = PoolManager.Instance.Spawn(NOLPrephab, spawnPlace, Quaternion.Euler(0, 0, 0), gameObject);
        if (newNLO.TryGetComponent<NLOController>(out NLOController nloController))
        {
            nloController.FlyDirection = UnityEngine.Random.Range(0, 2) == 0;
        }
        return newNLO;
    }
    private int getCurentCountByTag(String tag)
    {
        int ret = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == tag && child.gameObject.activeInHierarchy)
                ret++;
        }
        return ret;
    }
    #endregion

}
