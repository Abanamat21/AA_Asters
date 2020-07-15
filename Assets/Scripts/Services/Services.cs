using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Services : MonoBehaviour
{
    public PoolManager poolManager = new PoolManager();
    public AsteroidFactory asteroidFactory;
    public Loader loader;
    public UIController uiController;
    public GameFieldController gameFieldController;

    void Awake()
    {
        Debug.Log("Services");
        GameController.instance.services = this;
        loader = GetComponent<Loader>();
    }


}
