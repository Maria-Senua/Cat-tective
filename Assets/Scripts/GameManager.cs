using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public bool isGamePaused;
    public bool inventoryOpen = false;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
                if (FindObjectOfType<ScenesController>().isPaused)
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

        if (Input.GetKeyDown(KeyCode.I) && !isGamePaused)
        {
            Debug.Log("opening inv " + FindObjectOfType<ScenesController>().inventory.activeInHierarchy);
            FindObjectOfType<ScenesController>().OpenInventory();
        }

        inventoryOpen = FindObjectOfType<ScenesController>().inventory.activeInHierarchy;
    }

    public void TriggerTimeTravelScene()
    {
        FindObjectOfType<ScenesController>().ShowTimeTravelScene();
    }
}
