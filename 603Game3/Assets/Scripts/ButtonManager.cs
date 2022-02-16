using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    private LevelManager lManager;
    private GameObject options;
    private GameObject mainButtons;
    private Slider slider;
    public static float vol = .5f;
    // Start is called before the first frame update
    public void StartGame()
    {
        BetweenSceneData.loaded = false;
        //SceneManager.LoadScene("Game Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartMenu()
    {
        ManagerActions.paused = false;
        Time.timeScale = 1f;
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

    public void EnterOptions()
    {
        if (mainButtons == null) mainButtons = GameObject.Find("Canvas");
        options = mainButtons.GetComponent<BetweenSceneData>().options;
        if (slider == null) slider = options.transform.GetChild(1).GetComponent<Slider>();
        slider.value = vol;
        options.SetActive(true);
        mainButtons.SetActive(false);
    }

    public void LeaveOptions()
    {
        if (mainButtons == null) mainButtons = GameObject.Find("Canvas");
        options = mainButtons.GetComponent<BetweenSceneData>().options;
        options.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void ChangeVolume(AudioMixer mixer)
    {
        if (slider == null) slider = options.transform.GetChild(1).GetComponent<Slider>();
        if (slider.value < .05f)
        {
            mixer.SetFloat("masterVol", -80.0f);
        }
        else
        {
            mixer.SetFloat("masterVol", slider.value * 20);
        }
        vol = slider.value;
    }
}
