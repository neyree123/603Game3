using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class LevelManager : MonoBehaviour
{
    public BallScript[,] bubbleArray;
    private int[,] colorArray;

    public GameObject bubblePrefab;

    public int width;
    public int height;

    public Vector3 ballDisplacement;

    private Color[] colors;
    private string filePath;
    private string pathEnd = "/save.dat";
    private follow furby;

    // Start is called before the first frame update
    void Start()
    {
        furby = GameObject.Find("furbyHead").GetComponent<follow>();
        filePath = Application.persistentDataPath + pathEnd;
        // set up the colors
        colors = new Color[4];

        colors[0] = new Color(1, 0, 244.0f/255);
        colors[1] = new Color(0, 195.0f/255, 1);
        colors[2] = new Color(132.0f/255, 0, 1);
        colors[3] = new Color(33.0f/255, 1, 0);
        

        // set up the bubble array
        bubbleArray = new BallScript[width, height];

        if (BetweenSceneData.loaded)
        {
            Load();
        }
        // set up the bubble array
        FillArray();

    }

    private void FillArray()
    {
        float deltaX = 0.47f;
        float deltaY = 0.4f;


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject bubble = Instantiate(bubblePrefab);

                bubble.transform.position = new Vector3(deltaX * i + ((j % 2 == 0) ? 0 : deltaX / 2), -deltaY * j, 0) + ballDisplacement;
                bubble.transform.parent = transform;

                bubbleArray[i, j] = bubble.GetComponent<BallScript>();
                if (!BetweenSceneData.loaded)
                {
                    bubbleArray[i, j].ballColor = Random.Range(0, 4);
                }
                else
                {
                    if (colorArray[i, j] == -1)
                    {
                        bubbleArray[i, j].Pop();
                    }
                    else
                    {
                        bubbleArray[i, j].ballColor = colorArray[i, j];
                    }
                }
                bubbleArray[i, j].ChangeColor();
                //int colIdx = loaded ? colorArray[i, j] : Random.Range(0, 4);
                //colorArray[i, j] = colIdx;

                //bubble.GetComponent<SpriteRenderer>().color = colors[colIdx];
                bubbleArray[i, j].setup(i, j, GetComponent<LevelManager>());

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
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (bubbleArray[i, j].popped)
                        {
                            writer.Write(-1);
                        }
                        else
                        {
                            writer.Write(bubbleArray[i, j].ballColor);
                        }
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
            //FillArray();
        }
    }

    public void popBubble(int i, int j)
    {
        if (!bubbleArray[i, j].popped && bubbleArray[i, j].ballColor == furby.color)
        {
            bubbleArray[i, j].Pop();

            // left and right 
            if (i > 0)
            {
                popBubble(i - 1, j);
            }
            if (i < width - 1)
            {
                popBubble(i + 1, j);
            }

            // top and bottom
            if (i%2 == 0)
            {
                if (j > 0)
                {
                    if (i > 0)
                    {
                        popBubble(i - 1, j - 1);
                    }
                    popBubble(i, j - 1);
                }
                if (j < height - 1)
                {
                    if (i > 0)
                    {
                        popBubble(i - 1, j + 1);
                    }
                    popBubble(i, j + 1);
                }
            }

            if (i % 2 == 1)
            {
                if (j > 0)
                {
                    if (i < width - 1)
                    {
                        popBubble(i + 1, j - 1);
                    }
                    popBubble(i, j - 1);
                }
                if (j < height - 1)
                {
                    if (i < width - 1)
                    {
                        popBubble(i + 1, j + 1);
                    }
                    popBubble(i, j + 1);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

