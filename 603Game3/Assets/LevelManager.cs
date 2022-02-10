using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[,] bubbleArray;

    public GameObject bubblePrefab;

    public int width;
    public int height;

    private Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        // set up the colors
        colors = new Color[4];


        colors[0] = new Color(1, 0, 244.0f/255);
        colors[1] = new Color(0, 195.0f/255, 1);
        colors[2] = new Color(132.0f/255, 0, 1);
        colors[3] = new Color(33.0f/255, 1, 0);
        

        // set up the bubble array
        bubbleArray = new GameObject[width, height];

        float deltaX = 0.75f;
        float deltaY = 0.65f;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject bubble = Instantiate(bubblePrefab);

                bubble.transform.position = new Vector3(deltaX * i + ((j % 2 == 0) ? 0 : deltaX / 2), deltaY * j, 0);
                bubble.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, 4)];
                bubble.transform.parent = transform;

                bubbleArray[i, j] = bubble;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
