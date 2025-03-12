using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public bool isGamePaused;

    private void Awake()
    {

        sharedInstance = this;

        FindObjectOfType<ScenesController>().pauseScreen.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGamePaused = FindObjectOfType<ScenesController>().isPaused;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (FindObjectOfType<ScenesController>().inventory.activeInHierarchy)
            {
                FindObjectOfType<ScenesController>().CloseInventory();
            } else
            {
                if (isGamePaused)
                {
                    FindObjectOfType<ScenesController>().Resume();
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    FindObjectOfType<ScenesController>().Pause();

                }
            }
            
        }
    }
}
