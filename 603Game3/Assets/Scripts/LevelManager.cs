using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

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
    public float deltaX;
    public float deltaY;
    private FurbyController furbyHead;

    private Color[] empty;

    // Start is called before the first frame update
    void Start()
    {
        //-1 is none, 0 is pink, 1 is blue, 2 is purple, 3 is green
        //Size of the inner arrays is equal to height of the bubble layout
        //Total number of inner arrays is equal to width of the bubble layout
        colorArray = new int[,]
        {
            //1-3
            {-1, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {2, 0, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {3, 2, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        
            //4-6
            {-1, -1, -1, -1, -1, -1, -1, 3, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, 2, 3, 1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, 1, 2, 0, -1, -1, -1, -1, -1, -1},
        
            //7-9
            {-1, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {1, 2, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {3, 2, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        
            //10-12
            {-1, -1, -1, -1, -1, -1, -1, 0, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, 2, 1, 3, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, 0, 1, 2, -1, -1, -1, -1, -1, -1},
        
            //13-15
            {-1, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {1, 2, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
            {3, 2, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        
            //16-18
            {-1, -1, -1, -1, -1, -1, -1, 0, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, 2, 1, 3, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, 0, 1, 2, -1, -1, -1, -1, -1, -1},
        };

        GameObject head = GameObject.Find("furbyHead");
        furby = head.GetComponent<follow>();
        furbyHead = head.GetComponent<FurbyController>();
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

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject bubble = Instantiate(bubblePrefab);

                bubble.transform.position = new Vector3(deltaX * i + ((j % 2 == 0) ? 0 : deltaX / 2), -deltaY * j, 0) + ballDisplacement;
                bubble.transform.parent = transform;

                bubbleArray[i, j] = bubble.GetComponent<BallScript>();
                if (colorArray[i, j] == -1)
                {
                    bubbleArray[i, j].Pop();
                    bubbleArray[i, j].GetComponent<ParticleSystem>().Stop();
                }
                else
                {
                    bubbleArray[i, j].ballColor = colorArray[i, j];
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
                writer.Write((int)furbyHead.health);
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
                    furbyHead.health = reader.ReadInt32();
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
            if(j % 2 == 1)
            {
                if (j > 0)
                {
                    popBubble(i, j - 1);
                    if (i < width - 1)
                    {
                        popBubble(i + 1, j - 1);
                    }
                }
                if (j < height - 1)
                {
                    popBubble(i, j + 1);
                    if (i < width - 1)
                    {
                        popBubble(i + 1, j + 1);
                    }
                }
            }
            else
            {
                if (j > 0)
                {
                    popBubble(i, j - 1);
                    if (i > 0)
                    {
                        popBubble(i - 1, j - 1);
                    }
                }
                if (j < height - 1)
                {
                    popBubble(i, j + 1);
                    if (i > 0)
                    {
                        popBubble(i - 1, j + 1);
                    }
                }
            }
            if (i > 0)
            {
                popBubble(i - 1, j);
            }
            if(i < width - 1)
            {
                popBubble(i + 1, j);
            }
            //bubbleArray[i, j].Pop();

            // left and right 
            /*if (i > 0)
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
            }*/

            CheckForWin();
        }
    }

    public void CheckForWin()
    {
        //bool win = true;

        for (int i = 0; i < bubbleArray.GetLength(0); i++)
        {
            for (int j = 0; j < bubbleArray.GetLength(1); j++)
            {
                if (!bubbleArray[i,j].popped)
                {
                    //win = false;
                    return;
                }
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //if (win)
        //{
        //
        //}
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0)
        {
            //TriggerWin
            Debug.Log("Furby Win");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //CheckForWin();

    }
}

