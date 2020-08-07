using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Exstentions;

public class AsteroidController : MonoBehaviour, IHitable, IPoolable
{
    #region Поля
    public float FlySpeed { get; set; }
    public int Size;
    private Vector3 lastPos { get; set; }
    public float destructionAward;
    #endregion

    #region Служебные методы
    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.TryGetComponent<IStrikable>(out IStrikable strikable))
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
        transform.Translate(Vector3.forward * FlySpeed * Time.fixedDeltaTime);

        gameObject.WallEnteringLineCast(lastPos, transform.localPosition);
        lastPos = transform.localPosition;
    }
    #endregion

    #region Публичные методы
    public void Hit(GameObject projectile, GameObject causer)
    {
        Split();
    }
    public void Split()
    {
        List<AsteroidToLoad> asteroidsToLoad = AsteroidFactory.Instance.afterDestroitCreation(transform.localPosition, transform.rotation, Size);
        if (asteroidsToLoad != null && asteroidsToLoad.Count > 0)
            asteroidsToLoad.ForEach(x => PoolManager.Instance.Spawn(x));
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
        GameController.Instance.incrementScore(destructionAward);
    }
    #endregion
}
