using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    public GameObject inventory; //leave public, used in other scripts if active in hierarchy

    public GameObject pauseScreen;

    public bool isPaused = false;
    //public Camera catCamera;

    private void Awake()
    {
        Time.timeScale = 1f; 
        
    }


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isPaused)
        {
            OpenInventory();
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

    public void OpenTutorialScene()
    {
        Time.timeScale = 1f; 
       
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene("TutorialScene");
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
     
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenuScene");
    }
}
