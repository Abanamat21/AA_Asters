using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject GameField;
    public GameFieldController gameFieldController { private set; get; }
    public GameObject uiManager;
    public UIController uiController { private set; get; }
    public ControlType controlType { get; set; }
    public bool paused { get; private set; }
    public float score { get; private set; }
    public readonly float maxScore = 10000;

    private static GameController instance { get; set; }
    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null) instance = this;
        instance.gameFieldController = GameField.GetComponent<GameFieldController>() ?? throw new AAExceptions.ImportentComponentNotFound($"GameFieldController component is not founded in {GameField.transform.name}");
        instance.uiController = uiManager.GetComponent<UIController>() ?? throw new AAExceptions.ImportentComponentNotFound($"UIController component is not founded in {uiController.transform.name}");
    }

    internal void onPlayerHPChanged(int curentHealth)
    {
        instance.uiController.setPlayerHP(curentHealth);
    }

    private GameController()
    {
        controlType = ControlType.keyboard;
        score = 0;
        paused = true;
    }

    public void NewGame()
    {
        GameController.Instance.gameFieldController.Reset(); 
        score = 0;
        setPaused(false);               
    }

    public void setPaused(bool _paused)
    {
        if (GameController.Instance.uiController == null) throw new AAExceptions.ServiceIsUninitialized("UIController was not initialized");
        GameController.Instance.uiController.MenuCanvas.SetActive(_paused);
        GameController.Instance.uiController.HUDCanvas.SetActive(!_paused);
        GameController.Instance.uiController.ContinueButton.SetActive(_paused);
        Time.timeScale = _paused ? 0 : 1;
        paused = _paused;
        Debug.Log("setPaused " + _paused);
    }

    public void incrementScore(float increment)
    {
        score = score + increment;        
        if (score >= maxScore)
        {
            victory();
        }
    }

    public void defeat()
    {
        endGame(false);
        Debug.Log("Looooooser!!!!! U'r FAIL!!!");
    }

    public void victory()
    {
        endGame(true);
        Debug.Log("VICTORIA!!!!! No, seriously, it's true!!!");
    }

    private void endGame(bool victory)
    {
        //services.gameFieldController.reset();
        Time.timeScale = 0;
        GameController.Instance.uiController.fillEndGame(victory, score);
    }
}
