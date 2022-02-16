using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenSceneData : MonoBehaviour
{
    public static bool loaded;
    public GameObject options;
    // Start is called before the first frame update
    void Start()
    {
        loaded = false;
        options = GameObject.Find("Options");
        options.SetActive(false);
    }
}
