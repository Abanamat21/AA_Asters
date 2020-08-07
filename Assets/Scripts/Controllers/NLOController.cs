using System.Collections;
using UnityEngine;
using Assets.Scripts.Exstentions;

public class NLOController : MonoBehaviour, IHitable, IStrikable, IPoolable
{
    #region Поля
    public float Speed;
    public bool FlyDirection;
    private Vector3 flyVector;
    private Vector3 lastPos;

    public GameObject Projectile;
    public float CooldownMin_Sec;
    public float CooldownMax_Sec;

    public float DestructionAward;
    public GameObject Player;
    public float SpawnPeriodMin_Sec;
    public float SpawnPeriodMax_Sec;
    #endregion

    #region Служебные методы
    void Start()
    {
        lastPos = transform.localPosition;
        GameFieldController gameFieldController = transform.parent.gameObject.GetComponent<GameFieldController>();
        if (gameFieldController != null)
            Player = gameFieldController.Player;
        else
            throw new AAExceptions.ImportentComponentNotFound("GameFieldController component is not founded in NLOControllers parent");
        flyVector = (FlyDirection ? Vector3.right : Vector3.left).normalized * Speed;

        StartCoroutine(CoroutineShooting());
    }

    void FixedUpdate()
    {
        if (flyVector != null)
        {
            transform.Translate(flyVector * Time.fixedDeltaTime);
        }

        gameObject.WallEnteringLineCast(lastPos, transform.localPosition);
        lastPos = transform.localPosition;
    }
    #endregion


    #region Публичные методы
    public void Hit(GameObject projectile, GameObject causer)
    {
        GameController.Instance.gameFieldController.StartCoroutineNLOWave(Random.Range(SpawnPeriodMin_Sec, SpawnPeriodMax_Sec));
        Despawn();
    }

    public void Strike(GameObject causer)
    {
        GameController.Instance.gameFieldController.StartCoroutineNLOWave(Random.Range(SpawnPeriodMin_Sec, SpawnPeriodMax_Sec));
        Despawn();
    }

    public void Despawn()
    {
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        PoolManager.Instance.Despawn(gameObject);
    }

    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
        GameController.Instance.incrementScore(DestructionAward);
    }
    #endregion

    #region Внутренние методы
    private IEnumerator CoroutineShooting()
    {
        yield return new WaitForSeconds(Random.Range(CooldownMin_Sec, CooldownMax_Sec));
        Vector3 from = Projectile.transform.forward;
        Vector3 to = Player.transform.localPosition - transform.localPosition;
        Shoot(Quaternion.FromToRotation(from, to));
        StartCoroutine(CoroutineShooting());
    }

    private void Shoot(Quaternion quaternion)
    {
        Vector3 spuwnPlace = transform.localPosition;
        PoolManager.Instance.Spawn(new ProjectileToLoad(Projectile, spuwnPlace, quaternion, gameObject.transform.parent.gameObject, gameObject));
    }
    #endregion
}
