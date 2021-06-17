using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
     UnityEvent OnClickEvent = new UnityEvent();
    [SerializeField] Button resumeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(HandleResumeClicked);
        restartButton.onClick.AddListener(HandleRestartClicked);
        quitButton.onClick.AddListener(HandleReQuitClicked);
    }

    private void HandleResumeClicked()
    {
        GameManager.Instance.Pause();
    }
    private void HandleRestartClicked()
    {
        GameManager.Instance.RestartLevel();
    }
    private void HandleReQuitClicked()
    {
        GameManager.Instance.QuitGame();
    }
}

