using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject restartUI;

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    public void ShowScreen()
    {
        restartUI.SetActive(true);
        Time.timeScale = 0f;
        playerInput.DeactivateInput();
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        restartUI.SetActive(false);
        Time.timeScale = 1f;
        playerInput.ActivateInput();
        Cursor.lockState = CursorLockMode.Locked;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
