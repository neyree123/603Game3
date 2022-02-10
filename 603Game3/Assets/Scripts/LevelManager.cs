using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class LevelManager : MonoBehaviour
{
    public Bubble[,] bubbleArray;
    public int[,] colorArray;

    public GameObject bubblePrefab;

    public int width;
    public int height;

    public Vector3 ballDisplacement;

    private Color[] colors;
    private string filePath;
    private string pathEnd = "/save.dat";
    private bool loaded;

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
        bubbleArray = new Bubble[width, height];


        // set up the bubble array
        FillArray();

    }

    private void FillArray()
    {
        float deltaX = 0.47f;
        float deltaY = 0.4f;

        if (!loaded)
        {
            colorArray = new int[width, height];
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject bubble = Instantiate(bubblePrefab);

                bubble.transform.position = new Vector3(deltaX * i + ((j % 2 == 0) ? 0 : deltaX / 2), -deltaY * j, 0) + ballDisplacement;
                bubble.transform.parent = transform;

                bubbleArray[i, j] = bubble.GetComponent<Bubble>();

                int colIdx = loaded ? colorArray[i, j] : Random.Range(0, 4);
                colorArray[i, j] = colIdx;

                bubble.GetComponent<SpriteRenderer>().color = colors[colIdx];
                bubbleArray[i, j].setup(i, j, colIdx, GetComponent<LevelManager>());

            }
        }
    }

    // Update is called once per frame
    public void Save()
    {
        using (var stream = File.Open(filePath, FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.Unicode, false))
            {
                for (int i = 0; i < colorArray.GetLength(0); i++)
                {
                    for (int j = 0; j < colorArray.GetLength(1); j++)
                    {
                        writer.Write(colorArray[i, j]);
                    }
                }
            }
        }
    }


    public void Load()
    {
        if (File.Exists(filePath))
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    colorArray = new int[width, height];
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            colorArray[i, j] = reader.ReadInt32();
                        }
                    }
                }
            }
            loaded = true;
            FillArray();
        }
    }

    public void popBubble(int i, int j, int color)
    {
        if (!bubbleArray[i, j].popped && bubbleArray[i, j].color == color)
        {
            bubbleArray[i, j].Pop();

            // left and right 
            if (i > 0)
            {
                popBubble(i - 1, j, color);
            }
            if (i < width - 1)
            {
                popBubble(i + 1, j, color);
            }

            // top and bottom
            if (i%2 == 0)
            {
                if (j > 0)
                {
                    if (i > 0)
                    {
                        popBubble(i - 1, j - 1, color);
                    }
                    popBubble(i, j - 1, color);
                }
                if (j < height - 1)
                {
                    if (i > 0)
                    {
                        popBubble(i - 1, j + 1, color);
                    }
                    popBubble(i, j + 1, color);
                }
            }

            if (i % 2 == 1)
            {
                if (j > 0)
                {
                    if (i < width - 1)
                    {
                        popBubble(i + 1, j - 1, color);
                    }
                    popBubble(i, j - 1, color);
                }
                if (j < height - 1)
                {
                    if (i < width - 1)
                    {
                        popBubble(i + 1, j + 1, color);
                    }
                    popBubble(i, j + 1, color);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

