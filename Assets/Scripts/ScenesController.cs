using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    public GameObject inventory; //leave public, used in other scripts if active in hierarchy

    [SerializeField] GameObject pauseScreen;

    public bool isPaused = false;
    //public Camera catCamera;


    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartTutorial()
    {
        LevelManager.sharedInstance.SetLevel(1);
        LevelManager.sharedInstance.StartSelectedLevel();
        SceneManager.LoadScene("TutorialScene");
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;  
    }

    public void ShowTimeTravelScene()
    {
        InventoryManager.instance.ClearInventory();
        SceneManager.LoadScene("TimeTravelCutScene");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenInventory()
    {
        inventory.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
    }

    public void CloseInventory()
    {
        inventory.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
    }
}
