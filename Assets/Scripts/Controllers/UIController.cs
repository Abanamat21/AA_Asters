using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject TogleControlTypeButton;
    public GameObject ContinueButton;
    public GameObject menuCanvas;
    public GameObject hudCanvas;
    public GameObject endGameCanvas;

    public GameObject EndGameTitleLabel;
    public GameObject ScoreValueLabel;
    public GameObject EndGameScoreValueLabel;

    public GameObject HPValueLabel;

    void Start()
    {
        endGameCanvas.SetActive(false);
        GameController.instance.services.uiController = this;
        setControlType(GameController.instance.controlType);
        GameController.instance.setPaused(true);
    }

    void Update()
    {
        if (!GameController.instance.paused && Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.instance.setPaused(true);
        }
        Text ScoreValueLabelText = ScoreValueLabel.GetComponent<Text>();
        ScoreValueLabelText.text = GameController.instance.score.ToString();
    }

    public void NewGame()
    {
        Debug.Log("GameStart!");        
        GameController.instance.NewGame();
        endGameCanvas.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void Сontinue()
    {
        GameController.instance.setPaused(false);
    }

    public void TogleControlType()
    {
        ControlType oldControlType = GameController.instance.controlType;
        ControlType newControlType;
        if (oldControlType.name == ControlType.keyboard.name)
        {
            newControlType = ControlType.keyboardMouse;
        }
        else if (oldControlType.name == ControlType.keyboardMouse.name)
        {
            newControlType = ControlType.keyboard;
        } else
        {
            newControlType = ControlType.keyboardMouse;
        }
        Debug.Log(newControlType.name);
        GameController.instance.controlType = newControlType;
        setControlType(newControlType);
    }

    private void setControlType(ControlType controlType)
    {
        TogleControlTypeButton.GetComponentInChildren<Text>().text = controlType.menuDisplayName;
    }

    public void setPlayerHP(int curentHealth)
    {
        Text HPValueLabelText = HPValueLabel.GetComponent<Text>();
        HPValueLabelText.text = curentHealth.ToString();
    }

    public void fillEndGame(bool victory, float score)
    {
        hudCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        endGameCanvas.SetActive(true);
        Text EndGameTitleLabelText = EndGameTitleLabel.GetComponent<Text>();
        EndGameTitleLabelText.text = victory ? "Победа!" : "Поражение!";
        Text EndGameScoreValueLabelText = EndGameScoreValueLabel.GetComponent<Text>();
        EndGameScoreValueLabelText.text = score.ToString();
    }
}
