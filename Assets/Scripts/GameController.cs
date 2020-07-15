using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController
{
    public ControlType controlType { get; set; }
    public Services services { get; set; }
    public bool paused { get; private set; }
    public float score { get; private set; }
    public readonly float maxScore = 10000;

    private static GameController _instance;
    public static GameController instance { 
        get 
        { 
            if (_instance == null)
            {
                _instance = new GameController();

            }
            return _instance; 
        }
    }
    private GameController()
    {
        controlType = ControlType.keyboard;
        score = 0;
        paused = true;
    }
    public static void instantiate()
    {
        if(_instance == null)
            _instance = new GameController();
    }

    public void NewGame()
    {
        services.gameFieldController.reset(); 
        score = 0;
        setPaused(false);               
    }

    public void setPaused(bool _paused)
    {
        if (services.uiController == null) throw new AAExceptions.ServiceIsUninitialized("UIController was not initialized");
        services.uiController.menuCanvas.SetActive(_paused);
        services.uiController.hudCanvas.SetActive(!_paused);
        services.uiController.ContinueButton.SetActive(_paused);
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
        services.uiController.fillEndGame(victory, score);
    }
}
