using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
	#region Field Declarations

	[Header("UI Components")]
    [Space]
	public Text scoreText;
    public StatusText statusText;
    public Button restartButton;

    [Header("Ship Counter")]
    [SerializeField]
    [Space]
    private Image[] shipImages;
    private GameSceneController gameSceneController;
    #endregion


    private void Awake()
    {
      
    }
    private void Start()
    {
        gameSceneController = FindObjectOfType<GameSceneController>();
        ShowStatus(gameSceneController.currentLevel.levelName);
        gameSceneController.ScoreUpdatedOnKill += GameSceneController_ScoreUpdatedOnKill;
        gameSceneController.LifeLost += HideShip;
        gameSceneController.Lost += ShowRestartButton;
    }

    private void GameSceneController_ScoreUpdatedOnKill(int pointValue)
    {
        UpdateScore(pointValue);
    }

    #region Public methods

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("D5");
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

    public void ShowRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}
