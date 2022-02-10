using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColorChangeScript : MonoBehaviour
{
    private List<SpriteRenderer> wallsSR;
    public static WallColorChangeScript instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        wallsSR = new List<SpriteRenderer>();

        for (int i = 0; i < transform.childCount; i++)
        {
            wallsSR.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }

    public void ChangeWallColor(Color color)
    {
        foreach (SpriteRenderer sr in wallsSR)
        {
            sr.color = color;
        }
    }
}
