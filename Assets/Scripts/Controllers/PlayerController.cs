using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Exstentions;

public class PlayerController : MonoBehaviour, IHitable, IStrikable
{
    #region Поля
    public float MoveAcceleration;
    private float maxSpeed = 200f;
    private Vector3 moveVector;

    public float RotateSpeed;
    public GameObject CameraObject;
    private Vector3 rotateVector;
    private float lastFDT;
    private Camera gameCamera;

    public float Cooldown_Sec;
    public GameObject MidleGun;
    public GameObject Projectile;
    private DateTime lastShootTime;

    private Vector3 lastPos;
    private Rigidbody rb;

    public int MaxHealth;
    public float ImunTime_Sec;
    private bool isImun;
    private int curentHealth;
    public int CurentHealth
    {
        get
        {
            return curentHealth;
        }
        private set
        {
            if (isImun) return;
            curentHealth = value;
            try
            {
                GameController.Instance.onPlayerHPChanged(curentHealth);
            }
            catch { }
        }
    }
    #endregion

    #region Служебные методы
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCamera = CameraObject.GetComponent<Camera>();
        ResetParameters();
    }
    void Update()
    {
        if (GameController.Instance.paused) return;
        ControlType controlType = GameController.Instance.controlType;

        #region Движение
        float axis = Input.GetAxisRaw(controlType.MoveForvardAxisRawName);
        if (axis > 0) GetComponent<AudioSource>().Play();
        Vector3 moveUp = transform.up * axis;
        Vector3 corelation = moveUp.normalized * MoveAcceleration;
        Vector3 newMoveVector = moveVector + corelation;
        if (newMoveVector.magnitude < maxSpeed || newMoveVector.magnitude < moveVector.magnitude) //Если скорость меньше максимальной или меньше скорости на прошлом кадре
        {
            moveVector = newMoveVector;
        }

        #endregion

        #region Вращение
        if (controlType.RotateAxisRawName != "[MOUSE]")
        {
            rotateVector = -(transform.forward * Input.GetAxisRaw(controlType.RotateAxisRawName) * RotateSpeed); //по нажатию
        }
        else
        {
            Vector3 point = gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameCamera.nearClipPlane));
            Vector3 from = transform.up;
            from.z = 0;
            Vector3 to = point - transform.localPosition;
            point.z = 0;
            Vector3 fullRotate = Quaternion.FromToRotation(from, to).eulerAngles;
            fullRotate.y = 0;
            fullRotate.x = 0;
            if (Math.Abs(fullRotate.magnitude) > 180)
            {
                if (fullRotate.magnitude > 0)
                    fullRotate.z = fullRotate.z - 360;
                else
                    fullRotate.z = fullRotate.z + 360;
            }
            rotateVector = fullRotate.normalized * RotateSpeed;
            if ((rotateVector * lastFDT).magnitude > fullRotate.magnitude)
            {
                rotateVector = fullRotate;
            }            
        }

        #endregion

        #region Стрельба
        int cooldown_MSec = Convert.ToInt32(Cooldown_Sec * 1000);
        if (Input.GetButtonDown(controlType.ShootButtonName) && lastShootTime < DateTime.Now - new TimeSpan(0, 0, 0, 0, cooldown_MSec))
        {
            shoot(transform.rotation * Quaternion.Euler(-90f, 0f, 0f));
            lastShootTime = DateTime.Now;
        }
        #endregion

        #region Смерть
        if (curentHealth <= 0)
        {
            death();
        }
        #endregion                
    }
    void FixedUpdate()
    {
        if (moveVector != null)
        {
            rb.MovePosition(rb.position + moveVector * Time.fixedDeltaTime);
        }

        if (rotateVector != null)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateVector * Time.fixedDeltaTime));
        }

        lastFDT = Time.fixedDeltaTime;

        #region Столкновение со стенами
        gameObject.WallEnteringLineCast(lastPos, transform.localPosition);
        lastPos = transform.localPosition;
        #endregion
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IStrikable>(out IStrikable strikable))
        {
            strikable.Strike(gameObject);
        }
    }
    #endregion

    #region Публичные методы
    public void Hit(GameObject projectile, GameObject causer)
    {
        curentHealth = curentHealth - 1;
        Debug.Log("Hited!! curentHealth = " + curentHealth);
    }
    public void Strike(GameObject causer)
    {
        curentHealth = 0;
        Debug.Log("Striked!! curentHealth = " + curentHealth);
    }
    public void ResetParameters()
    {
        lastShootTime = DateTime.MinValue;
        lastPos = transform.localPosition;
        curentHealth = MaxHealth;
        if (transform.parent.TryGetComponent<GameFieldController>(out GameFieldController gameFieldController))
            transform.localPosition = new Vector3(gameFieldController.Scale.x / 2, gameFieldController.Scale.y / 2, 0);
        else
            throw new AAExceptions.ImportentComponentNotFound($"{transform.parent.name} controller is not GameFieldController");
        transform.rotation = Quaternion.Euler(0, 0, 0);
        moveVector = Vector3.zero;
        StartCoroutine(coroutineImun());
    }
    #endregion

    #region Внутренние методы
    private void shoot(Quaternion quaternion)
    {
        Vector3 spuwnPlace = transform.localPosition + transform.up * 3;
        PoolManager.Instance.Spawn(new ProjectileToLoad(Projectile, spuwnPlace, quaternion, gameObject.transform.parent.gameObject, gameObject));
    }
    private IEnumerator coroutineImun()
    {
        isImun = true;
        Animation animation = GetComponent<Animation>();
        animation.Play();
        yield return new WaitForSeconds(ImunTime_Sec);
        animation.Stop();
        isImun = false;
    }
    private void death()
    {
        GameController.Instance.defeat();
    }
    #endregion
}
