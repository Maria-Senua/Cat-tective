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
    public float timeTravelTime;
    public float finalVideoTime = 4f;
    Scene currentScene;
    private string sceneName;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    private void Update()
    {
        if (sceneName == "TimeTravelCutScene")
        {
            Cursor.lockState = CursorLockMode.Locked;
            timeTravelTime -= Time.deltaTime;

            if (timeTravelTime <= 0) OpenEmptyScene();
        }
        if (sceneName == "FinalCutScene")
        {
            Cursor.lockState = CursorLockMode.Locked;
            finalVideoTime -= Time.deltaTime;

            if (finalVideoTime <= 0) GoToMenu();
        }
    }

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

        if (TutorialManager.sharedInstance != null)
        {
            TutorialManager.sharedInstance.currentIndex = 0;
            TutorialManager.sharedInstance.ActivateTutorial(0);
            TutorialManager.sharedInstance.startClimbing = false;
            TutorialManager.sharedInstance.startChilling = false;
            TutorialManager.sharedInstance.startPlaying = false;
            TutorialManager.sharedInstance.isSearching = false;
            TutorialManager.sharedInstance.solvedPuzzle = false;
            TutorialManager.sharedInstance.foundWatch = false;
        }
    }

    public void ShowTimeTravelScene()
    {
        //InventoryManager.instance.ClearInventory();
        //InventoryManager.instance.InitializeCombinedSprites();
        SceneManager.LoadScene("TimeTravelCutScene");
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowFinalCutScene()
    {
        SceneManager.LoadScene("FinalCutScene");
    }

    public void StartCrimeLevel()
    {
        LevelManager.sharedInstance.currentLevel = 3;

        SceneManager.LoadScene("CrimeScene");

    }



    public void OpenEmptyScene()
    {
        LevelManager.sharedInstance.SetLevel(2);
        LevelManager.sharedInstance.StartSelectedLevel();
        //InventoryManager.instance.ResetInventoryState();
        //InventoryManager.instance.InitializeCombinedSprites();

        SceneManager.LoadScene("EmptyScene");
        //Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void OpenInventory()
    {
        inventory.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
        Cursor.lockState = CursorLockMode.None;
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
            isPaused = false; 
        }

    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
