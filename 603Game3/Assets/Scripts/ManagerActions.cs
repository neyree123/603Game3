using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerActions : MonoBehaviour
{
    private LevelManager lManager;
    public static bool paused;
    private static GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
        lManager = GetComponent<LevelManager>();
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.End))
        {
            lManager.Save();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            lManager.Load();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public static void Pause()
    {
        paused = !paused;
        if(pauseMenu != null) pauseMenu.SetActive(paused);
        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
