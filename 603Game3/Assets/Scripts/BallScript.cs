using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    public int ballColor;
    private LevelManager level;
    private int i;
    private int j;
    public bool popped = true;
    public LayerMask furbyMask;
    public LayerMask bulletMask;

    float poppedTimer;
    public AudioClip highHealthHit;
    public AudioClip mediumHealthHit;
    public AudioClip lowHealthHit;
    public AudioClip highHealthBounce;
    public AudioClip mediumHealthBounce;
    public AudioClip lowHealthBounce;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        poppedTimer = 0;
        source = GetComponent<AudioSource>();
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
            bool eaten = level.popBubble(i, j);
            float hp = collision.gameObject.GetComponent<FurbyController>().health;
            if(hp < 4.0f)
            {
                if(eaten) source.clip = lowHealthHit;
                else source.clip = lowHealthBounce;
            }
            else if(hp < 7.0f)
            {
                if (eaten) source.clip = mediumHealthHit;
                else source.clip = mediumHealthBounce;
            }
            else
            {
                if (eaten) source.clip = highHealthHit;
                else source.clip = highHealthBounce;
            }
            source.Play();
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
        Color color = new Color();

        switch (ballColor)
        {
            case 0:
                //color = new Color(1.0f, .0627451f, .94117647f);
                color = new Color(1, 0, 244.0f / 255);
                break;
            case 1:
                //color = new Color(.10588235f, .01176471f, .6392157f);
                color = new Color(0, 195.0f / 255, 1);
                break;
            case 2:
                //color = new Color(.6901961f, .14901961f, 1.0f);
                color = new Color(132.0f / 255, 0, 1);
                break;
            case 3:
                //color = new Color(.2235294f, 1.0f, .07843137f);
                color = new Color(33.0f / 255, 1, 0);
                break;
            default:
                break;
        }
        GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        //GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<ParticleSystem>().startColor = color;

    }

    private void Update()
    {
        if (popped)
        {
            if (poppedTimer < 1)
                poppedTimer += Time.deltaTime;
            else
                GetComponent<ParticleSystem>().Stop();
        }
    }

    public void Pop()
    {
        if (!popped)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<ParticleSystem>().Play();
            popped = true;
        }
    }

}
