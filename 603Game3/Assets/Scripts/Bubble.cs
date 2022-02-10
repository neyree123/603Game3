using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private LevelManager level;
    private int i;
    private int j;
    public int color;
    public bool popped;

    private void Start()
    {
        popped = false;
    }

    public void setup(int _i, int _j, int _color, LevelManager _level)
    {
        i = _i;
        j = _j;
        color = _color;
        level = _level;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && collision.gameObject.GetComponent<follow>().color == color)
        {
            level.popBubble(i, j, color);
        }
    }

    public void Pop()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        popped = true;
    }
}
