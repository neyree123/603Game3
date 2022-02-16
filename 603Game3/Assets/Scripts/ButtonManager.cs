using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    private LevelManager lManager;
    // Start is called before the first frame update
    public void StartGame()
    {
        BetweenSceneData.loaded = false;
        //SceneManager.LoadScene("Game Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartMenu()
    {
        ManagerActions.Pause();
        SceneManager.LoadScene("Start Scene");
    }

    public void Save()
    {
        if (lManager == null) lManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lManager.Save();
    }

    public void Load()
    {
        BetweenSceneData.loaded = true;
        SceneManager.LoadScene("Game Scene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
