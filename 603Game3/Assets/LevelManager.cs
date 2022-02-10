using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class LevelManager : MonoBehaviour
{
    public BallScript[,] bubbleArray;
    public int[,] colorArray;

    public GameObject bubblePrefab;

    public int width;
    public int height;

    private Color[] colors;
    private string filePath;
    private string pathEnd = "/save.dat";
    private bool loaded;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + pathEnd;
        // set up the colors
        colors = new Color[4];


        colors[0] = new Color(1, 0, 244.0f/255);
        colors[1] = new Color(0, 195.0f/255, 1);
        colors[2] = new Color(132.0f/255, 0, 1);
        colors[3] = new Color(33.0f/255, 1, 0);
        loaded = false;
        // set up the bubble array
        FillArray();
    }

    private void FillArray()
    {
        bubbleArray = new BallScript[width, height];
        float deltaX = 0.75f;
        float deltaY = 0.65f;
        if (!loaded)
        {
            colorArray = new int[width, height];
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject bubble = Instantiate(bubblePrefab);

                bubble.transform.position = new Vector3(deltaX * i + ((j % 2 == 0) ? 0 : deltaX / 2), deltaY * j, 0);
                //bubble.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, 4)];
                bubble.transform.parent = transform;
                BallScript script = bubble.GetComponent<BallScript>();
                bubbleArray[i, j] = script;
                if (loaded)
                {
                    script.ballColor = (BallColor)colorArray[i, j];
                }
                else
                {
                    script.ChooseColor();
                    colorArray[i, j] = (int)script.ballColor;
                }
                script.ChangeColor();
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
            foreach(Transform child in transform)
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
}
