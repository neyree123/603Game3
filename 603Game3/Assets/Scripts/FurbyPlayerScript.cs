using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurbyPlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool canFire = true;
    public float speed = 5;
    public float curveSpeed = .01f;


    private SpriteRenderer sr;
    private int currentColorNum = 0; //0 = Pink, 1 = Purple, 2 = Green, 3 = Blue
    public Color[] colors = new Color[4];

    private Vector3 startPos;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        startPos = transform.position;

        //Set Colors
        sr.color = colors[currentColorNum];

        //if (currentColorNum >= 3)
        //{
        //    WallColorChangeScript.instance.ChangeWallColor(colors[0]);
        //}
        //else
        //{
        //    WallColorChangeScript.instance.ChangeWallColor(colors[currentColorNum + 1]);
        //}

    }

    // Update is called once per frame
    void Update()
    {

        //Rotate towards direction of movement
        Vector2 moveDir = rb.velocity;
        if (moveDir != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
            transform.rotation = toRotation;
        }
        
        //Firing at Beginning
        if (canFire && Input.GetMouseButtonUp(0))
        {
            Fire();
        }
        //Right Click For Position Reset (Mostly For Testing)
        else if (Input.GetMouseButtonDown(1))
        {
            ResetPos();
        }

        //Curving left and right
        //Uses rb.rotation to ensure that the velocity change is always happening in the proper direction
        if (!canFire && Input.GetKey(KeyCode.A))
        {

            float rot = rb.rotation;
            Vector3 temp = new Vector2();

            if (rot >= -22.5 && rot < 22.5)
            {
                temp = rb.velocity + new Vector2(-curveSpeed, 0);
            }
            else if (rot >= 22.5 && rot < 67.5)
            {
                temp = rb.velocity + new Vector2(-curveSpeed, -curveSpeed);
            }
            else if (rot >= 67.5 && rot < 112.5)
            {
                temp = rb.velocity + new Vector2(0, -curveSpeed);
            }
            else if (rot >= 112.5 && rot < 157.5)
            {
                temp = rb.velocity + new Vector2(curveSpeed, -curveSpeed);
            }
            else if (rot >= 157.5 || rot < -157.5)
            {
                temp = rb.velocity + new Vector2(curveSpeed, 0);
            }
            else if (rot >= -157.5 && rot < -112.5)
            {
                temp = rb.velocity + new Vector2(curveSpeed, curveSpeed);
            }
            else if (rot >= -112.5 && rot < -67.5)
            {
                temp = rb.velocity + new Vector2(0, curveSpeed);
            }
            else if (rot >= -67.5 && rot < -22.5)
            {
                temp = rb.velocity + new Vector2(-curveSpeed, curveSpeed);
            }

            rb.velocity = Vector2.zero;
            rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);

        }
        else if (!canFire && Input.GetKey(KeyCode.D))
        {

            float rot = rb.rotation;
            Vector3 temp = new Vector2();

            if (rot >= -22.5 && rot < 22.5)
            {
                temp = rb.velocity + new Vector2(curveSpeed, 0);
            }
            else if (rot >= 22.5 && rot < 67.5)
            {
                temp = rb.velocity + new Vector2(curveSpeed, curveSpeed);
            }
            else if (rot >= 67.5 && rot < 112.5)
            {
                temp = rb.velocity + new Vector2(0, curveSpeed);
            }
            else if (rot >= 112.5 && rot < 157.5)
            {
                temp = rb.velocity + new Vector2(-curveSpeed, curveSpeed);
            }
            else if (rot >= 157.5 || rot < -157.5)
            {
                temp = rb.velocity + new Vector2(-curveSpeed, 0);
            }
            else if (rot >= -157.5 && rot < -112.5)
            {
                temp = rb.velocity + new Vector2(-curveSpeed, -curveSpeed);
            }
            else if (rot >= -112.5 && rot < -67.5)
            {
                temp = rb.velocity + new Vector2(0, -curveSpeed);
            }
            else if (rot >= -67.5 && rot < -22.5)
            {
                temp = rb.velocity + new Vector2(curveSpeed, -curveSpeed);               
            }

            rb.velocity = Vector2.zero;
            rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Launches the Furby Directly Upwards
    /// </summary>
    public void Fire()
    {

        Vector3 dir = (new Vector2(0, 10)).normalized;
        Vector2 velocity = new Vector2(dir.x, dir.y) * speed;
        rb.AddForce(velocity, ForceMode2D.Impulse);

        canFire = false;
    }

    /// <summary>
    /// Changes the Furby's Color and Triggers the Walls to Change Color
    /// </summary>
    private void ChangeColor()
    {
        //Change Furby Color
        if (currentColorNum >= 3)
        {
            currentColorNum = 0;
        }
        else
        {
            currentColorNum++;
        }

        sr.color = colors[currentColorNum];

        //Change Wall Colors
        //if (currentColorNum >= 3)
        //{
        //    WallColorChangeScript.instance.ChangeWallColor(colors[0]);
        //}
        //else
        //{
        //    WallColorChangeScript.instance.ChangeWallColor(colors[currentColorNum + 1]);
        //}

        
    }

    /// <summary>
    /// Resets the Furby to the Position it Started In
    /// </summary>
    private void ResetPos()
    {
        rb.velocity = new Vector2(0, 0);
        transform.position = startPos;
        canFire = true;
    }

    //Collison Fun
    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            ChangeColor();
        }
        else if (collision.gameObject.tag == "Ball")
        { 
            //Compare colors
            if ((int)collision.gameObject.GetComponent<BallScript>().ballColor == currentColorNum)
            {
                Destroy(collision.gameObject);
                //And Destroy Connected
            }
        }
    }
}
