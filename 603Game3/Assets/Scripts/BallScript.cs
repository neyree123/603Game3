using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallColor
{
    Pink = 0,
    Purple = 1,
    Green = 2,
    Blue = 3
}

public class BallScript : MonoBehaviour
{

    public BallColor ballColor;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ChooseColor()
    {
        float val = Random.value; //The follow lines of code will be removed once we have a ball layout
        if (val < .25f)
        {
            ballColor = BallColor.Pink;
        }
        else if (val < .5f)
        {
            ballColor = BallColor.Purple;
        }
        else if (val < .75f)
        {
            ballColor = BallColor.Green;
        }
        else
        {
            ballColor = BallColor.Blue;
        }
    }

    public void ChangeColor()
    {
        switch (ballColor)
        {
            case BallColor.Pink:
                GetComponent<SpriteRenderer>().color = new Color(1.0f, .0627451f, .94117647f);
                break;
            case BallColor.Purple:
                GetComponent<SpriteRenderer>().color = new Color(.6901961f, .14901961f, 1.0f);
                break;
            case BallColor.Green:
                GetComponent<SpriteRenderer>().color = new Color(.2235294f, 1.0f, .07843137f);
                break;
            case BallColor.Blue:
                GetComponent<SpriteRenderer>().color = new Color(.10588235f, .01176471f, .6392157f);
                break;
            default:
                break;
        }
    }

}
