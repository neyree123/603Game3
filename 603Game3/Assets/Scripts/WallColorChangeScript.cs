using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColorChangeScript : MonoBehaviour
{
    private List<SpriteRenderer> wallsSR;
    public static WallColorChangeScript instance;
    private Color[] colors;
    private int colorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        wallsSR = new List<SpriteRenderer>();

        for (int i = 0; i < transform.childCount; i++)
        {
            wallsSR.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }

        colors = new Color[4];
        colors[0] = new Color(1, 0, 244.0f / 255);
        colors[1] = new Color(0, 195.0f / 255, 1);
        colors[2] = new Color(132.0f / 255, 0, 1);
        colors[3] = new Color(33.0f / 255, 1, 0);

        ChangeWallColor();
    }

    public void ChangeWallColor()
    {
        colorIndex += 1;

        if (colorIndex > 3)
        {
            colorIndex = 0;
        }

        GetComponent<ParticleSystem>().startColor = colors[colorIndex];

        foreach (SpriteRenderer sr in wallsSR)
        {
            sr.color = colors[colorIndex];
        }
    }
}
