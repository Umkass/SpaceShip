using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : Singleton<HUDController>
{
    #region Field Declarations

    [Header("UI Components")]
    [Space]
    public Text scoreText;
    public StatusText statusText;
    public Button onlyRestartButton;
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Ship Counter")]
    [SerializeField]
    [Space]
    private Image[] shipImages;
    #endregion

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        onlyRestartButton.onClick.AddListener(HandleOnlyRestartClicked);
        GameManager.Instance.ScoreUpdatedOnKill += GameManager_ScoreUpdatedOnKill;
        GameManager.Instance.LifeLost += HideShip;
        GameManager.Instance.ShowCurrentStatus += ShowStatus;
        GameManager.Instance.ShowPauseMenu += ShowPauseMenu;
        GameManager.Instance.ShowRestartButton += ShowRestartButton;
    }

    private void GameManager_ScoreUpdatedOnKill(int pointValue)
    {
        UpdateScore(pointValue);
    }

    #region Public methods

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("D5");
    }

    public void ShowPauseMenu(bool isShowing)
    {
        pauseMenu.gameObject.SetActive(isShowing);
    }
    public void ShowStatus(string newStatus)
    {
        statusText.gameObject.SetActive(true);
        StartCoroutine(statusText.ChangeStatus(newStatus));
    }

    public void HideShip(int imageIndex)
    {
        shipImages[imageIndex].gameObject.SetActive(false);
    }

    public void ResetShips()
    {
        foreach (Image ship in shipImages)
            ship.gameObject.SetActive(true);
    }

    public void ShowRestartButton(bool isShowing)
    {
        onlyRestartButton.gameObject.SetActive(isShowing);
    }
    public void HandleOnlyRestartClicked()
    {
        GameManager.Instance.RestartLevel();
    }

    #endregion
}
