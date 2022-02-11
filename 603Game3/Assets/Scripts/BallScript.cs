using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    public int ballColor;
    private LevelManager level;
    private int i;
    private int j;
    public bool popped;
    public LayerMask furbyMask;
    // Start is called before the first frame update
    void Start()
    {
        popped = false;
    }

    public void setup(int _i, int _j, LevelManager _level)
    {
        i = _i;
        j = _j;
        level = _level;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (furbyMask == (furbyMask | (1 << collision.gameObject.layer)))
        {
            level.popBubble(i, j);
        }
    }

    public void ChangeColor()
    {
        switch (ballColor)
        {
            case 0:
                GetComponent<SpriteRenderer>().color = new Color(1.0f, .0627451f, .94117647f);
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = new Color(.10588235f, .01176471f, .6392157f);
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = new Color(.6901961f, .14901961f, 1.0f);
                break;
            case 3:
                GetComponent<SpriteRenderer>().color = new Color(.2235294f, 1.0f, .07843137f);
                break;
            default:
                break;
        }
    }

    public void Pop()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        popped = true;
    }

}
