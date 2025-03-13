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
    Scene currentScene;
    private string sceneName;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    private void Start()
    {
        Resume();
    }

    private void Update()
    {
        if (sceneName == "TimeTravelCutScene")
        {
            Cursor.lockState = CursorLockMode.Locked;
            timeTravelTime -= Time.deltaTime;

            if (timeTravelTime <= 0) StartCrimeLevel();
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
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;  
    }

    public void ShowTimeTravelScene()
    {
        InventoryManager.instance.ClearInventory();
        SceneManager.LoadScene("TimeTravelCutScene");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartCrimeLevel()
    {
        Debug.Log("Current Level: " + LevelManager.sharedInstance.currentLevel);
        Debug.Log("Is Tutorial? " + LevelManager.sharedInstance.isTutorial);
        Debug.Log("Picked Evidence Count: " + InventoryManager.instance.pickedEvidences.Count);


        LevelManager.sharedInstance.ResetLevelState();
        InventoryManager.instance.ResetInventoryState();

        SceneManager.LoadScene("CrimeScene");
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
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
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
        Cursor.lockState = CursorLockMode.None;
        
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
