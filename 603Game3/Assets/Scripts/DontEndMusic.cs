using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontEndMusic : MonoBehaviour
{
    public static bool playedMusic = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (playedMusic)
        {
            Destroy(gameObject);
        }
        playedMusic = true;
        DontDestroyOnLoad(gameObject);
    }
}
