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
    public LayerMask bulletMask;
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
            //play bubble pop sound
            //health from FurbyController maybe?
            //if(health>7) {furby_bubble}
            //else if(7>health>4) {furby_bubble_halfglitch2}
            //else if(4>health>0) {furby_bubble_glitch2}

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bulletMask == (bulletMask | (1 << collision.gameObject.layer)))
        {
            if (!collision.GetComponent<BulletScript>().isCharged)
            {
                Destroy(collision.gameObject);
            }           
        }
    }

    public void ChangeColor()
    {
        switch (ballColor)
        {
            case 0:
                GetComponent<SpriteRenderer>().color = new Color(1, 0, 244.0f / 255);
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = new Color(0, 195.0f / 255, 1);
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = new Color(132.0f / 255, 0, 1);
                break;
            case 3:
                GetComponent<SpriteRenderer>().color = new Color(33.0f / 255, 1, 0);
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
