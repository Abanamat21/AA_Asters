using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Поля
    public GameObject TogleControlTypeButton;
    public GameObject ContinueButton;
    public GameObject MenuCanvas;
    public GameObject HUDCanvas;
    public GameObject EndGameCanvas;
    public GameObject EndGameTitleLabel;
    public GameObject ScoreValueLabel;
    public GameObject EndGameScoreValueLabel;
    public GameObject HPValueLabel;
    #endregion

    #region Служебные методы
    void Start()
    {
        EndGameCanvas.SetActive(false);
        setControlType(GameController.Instance.controlType);
        GameController.Instance.setPaused(true);
        ContinueButton.SetActive(false);
    }
    void Update()
    {
        if (!GameController.Instance.paused && Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.setPaused(true);
        }
        Text ScoreValueLabelText = ScoreValueLabel.GetComponent<Text>();
        ScoreValueLabelText.text = GameController.Instance.score.ToString();
    }
    #endregion

    #region "События"
    public void NewGame()
    {
        Debug.Log("GameStart!");        
        GameController.Instance.NewGame();
        EndGameCanvas.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Сontinue()
    {
        GameController.Instance.setPaused(false);
    }
    public void TogleControlType()
    {
        ControlType oldControlType = GameController.Instance.controlType;
        ControlType newControlType;
        if (oldControlType.Name == ControlType.keyboard.Name)
        {
            newControlType = ControlType.keyboardMouse;
        }
        else if (oldControlType.Name == ControlType.keyboardMouse.Name)
        {
            newControlType = ControlType.keyboard;
        } else
        {
            newControlType = ControlType.keyboardMouse;
        }
        Debug.Log(newControlType.Name);
        GameController.Instance.controlType = newControlType;
        setControlType(newControlType);
    }
    #endregion

    private void setControlType(ControlType controlType)
    {
        TogleControlTypeButton.GetComponentInChildren<Text>().text = controlType.MenuDisplayName;
    }
    public void setPlayerHP(int curentHealth)
    {
        Text HPValueLabelText = HPValueLabel.GetComponent<Text>();
        HPValueLabelText.text = curentHealth.ToString();
    }
    public void fillEndGame(bool victory, float score)
    {
        HUDCanvas.SetActive(false);
        MenuCanvas.SetActive(false);
        EndGameCanvas.SetActive(true);
        Text EndGameTitleLabelText = EndGameTitleLabel.GetComponent<Text>();
        EndGameTitleLabelText.text = victory ? "Победа!" : "Поражение!";
        Text EndGameScoreValueLabelText = EndGameScoreValueLabel.GetComponent<Text>();
        EndGameScoreValueLabelText.text = score.ToString();
    }
}
