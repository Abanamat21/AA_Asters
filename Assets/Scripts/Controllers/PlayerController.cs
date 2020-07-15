using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHitable, IStrikable
{
    public int maxHealth;
    public float imunTime_Sec;
    private bool isImun;
    private int _curentHealth;

    public float moveAcceleration;
    private float maxSpeed = 200f;
    private Vector3 moveVector;

    public float rotateSpeed;
    public GameObject cameraObject;
    private Vector3 rotateVector;
    private Camera gameCamera;

    public float cooldown_Sec;
    public GameObject midleGun;
    public GameObject projectile;
    private DateTime lastShootTime;

    private Vector3 lastPos;
    private Rigidbody rb;

    public int curentHealth { 
        get 
        { 
            return _curentHealth;
        } 
        private set
        {
            if (isImun) return;
            _curentHealth = value;
            try
            {
                GameController.instance.services.uiController.setPlayerHP(curentHealth);
            }
            catch { }
        } 
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCamera = cameraObject.GetComponent<Camera>(); 
        resetParameters();
    }

    void Update()
    {
        if (GameController.instance.paused) return;
        ControlType controlType = GameController.instance.controlType;

        #region Движение
        float axis = Input.GetAxisRaw(controlType.moveForvardAxisRawName);
        if(axis > 0) GetComponent<AudioSource>().Play();
        Vector3 moveUp = transform.up * axis;
        Vector3 corelation = moveUp.normalized * moveAcceleration;
        Vector3 newMoveVector = moveVector + corelation;
        if (newMoveVector.magnitude < maxSpeed || newMoveVector.magnitude < moveVector.magnitude) //Если скорость меньше максимальной или меньше скорости на прошлом кадре
        {
            moveVector = newMoveVector;
        }

        #endregion

        #region Вращение
        if(controlType.rotateAxisRawName != "[MOUSE]")
        {
            rotateVector = -(transform.forward * Input.GetAxisRaw(controlType.rotateAxisRawName) * rotateSpeed); //по нажатию
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
            rotateVector = fullRotate.normalized * rotateSpeed;
            if ((rotateVector * Time.fixedDeltaTime).magnitude > fullRotate.magnitude)
            {
                rotateVector = fullRotate;
            }
        }

        #endregion

        #region Стрельба
        int cooldown_MSec = Convert.ToInt32(cooldown_Sec * 1000);
        if (Input.GetButtonDown(controlType.shootButtonName) && lastShootTime < DateTime.Now - new TimeSpan(0, 0, 0, 0, cooldown_MSec))
        {
            Shoot(transform.rotation * Quaternion.Euler(-90f, 0f, 0f));
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
            //transform.Translate(moveVector * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + moveVector * Time.fixedDeltaTime);
        }

        if(rotateVector != null)
        {
            //transform.rotation = transform.rotation * Quaternion.Euler(rotateVector);
            //rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateVector * Time.fixedDeltaTime));
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateVector * Time.fixedDeltaTime));
        }


        #region Столкновение со стенами

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
        #endregion
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IStrikable>(out IStrikable strikable))
        {
            strikable.Strike(gameObject);
        }
    }
    public void Shoot(Quaternion quaternion)
    {
        PoolManager poolManager = GameController.instance.services.poolManager;
        Vector3 spuwnPlace = transform.localPosition + transform.up * 3;
        poolManager.Spawn(new ProjectileToLoad(projectile, spuwnPlace, quaternion, gameObject.transform.parent.gameObject, gameObject));        
    }

    public void hit(GameObject projectile, GameObject causer)
    {
        curentHealth = curentHealth - 1;
        Debug.Log("Hited!! curentHealth = " + curentHealth);
    }

    public void Strike(GameObject causer)
    {
        curentHealth = 0;
        Debug.Log("Striked!! curentHealth = " + curentHealth);
    }

    private IEnumerator CoroutineImun()
    {
        isImun = true;
        Animation animation = GetComponent<Animation>();
        animation.Play();
        yield return new WaitForSeconds(imunTime_Sec);
        animation.Stop();
        isImun = false;
    }
    private void death()
    {        
        GameController.instance.defeat();
    }

    public void resetParameters()
    {
        lastShootTime = DateTime.MinValue;
        lastPos = transform.localPosition;
        curentHealth = maxHealth;
        //transform.localPosition = Vector3.zero; //хорощо бы так сделать
        if (transform.parent.TryGetComponent<GameFieldController>(out GameFieldController gameFieldController))
            transform.localPosition = new Vector3(gameFieldController.scale.x / 2, gameFieldController.scale.y / 2, 0);
        else
            throw new AAExceptions.ImportentComponentNotFound($"{transform.parent} controller is not GameFieldController");
        transform.rotation = Quaternion.Euler(0, 0, 0);
        moveVector = Vector3.zero;
        StartCoroutine(CoroutineImun());
    }
}
