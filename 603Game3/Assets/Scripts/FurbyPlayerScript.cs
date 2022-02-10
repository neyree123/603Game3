using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurbyPlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector3 myScreenPos;
    public bool canFire = true;
    public float speed = 5;
    public float curveSpeed = .01f;
    public bool isGhost = false;

    //public Projection projection;

    private SpriteRenderer sr;
    private int currentColorNum = 0; //0 = Pink, 1 = Purple, 2 = Green, 3 = Blue
    public Color[] colors = new Color[4];

    private Vector3 startPos;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        sr = GetComponent<SpriteRenderer>();
        sr.color = colors[currentColorNum];
        startPos = transform.position;

        //projection = GetComponent<Projection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGhost)
        {
            return;
        }

        //Rotate towards direction of movement
        Vector2 moveDir = rb.velocity;
        if (moveDir != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
            transform.rotation = toRotation;
        }
        
        //Firing at Beginning
        if (canFire && Input.GetMouseButton(0))
        {
            //projection.SimulateTrajectory(this);
        }
        else if (canFire && Input.GetMouseButtonUp(0))
        {
            Fire(rb);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ResetPos();
        }
        //Debug.Log(rb.velocity);

        //Curving left and right
        //Uses rb.rotation to ensure that the velocity change is always happening in the propoer direction
        if (!canFire && Input.GetKey(KeyCode.A))
        {

            if (rb.rotation >= 150 || rb.rotation <= -130)
            {
                Vector3 temp = rb.velocity + new Vector2(curveSpeed, 0);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else if (rb.rotation >= 130)
            {
                Vector3 temp = rb.velocity + new Vector2(curveSpeed, -curveSpeed);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else if (rb.rotation >= 88)
            {
                Vector3 temp = rb.velocity + new Vector2(0, -curveSpeed);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else if (rb.rotation >= 60)
            {
                Vector3 temp = rb.velocity + new Vector2(-curveSpeed, -curveSpeed);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else if (rb.rotation <= -60)
            {
                Vector3 temp = rb.velocity + new Vector2(curveSpeed, curveSpeed);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else
            {
                Vector3 temp = rb.velocity + new Vector2(-curveSpeed, 0);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }

        }
        else if (!canFire && Input.GetKey(KeyCode.D))
        {

            if (rb.rotation > 60)
            {
                Vector3 temp = rb.velocity + new Vector2(-curveSpeed, curveSpeed);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else if (rb.rotation <= -160 || rb.rotation > 100)
            {
                Vector3 temp = rb.velocity + new Vector2(-curveSpeed, 0);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else if (rb.rotation <= -88)
            {
                Vector3 temp = rb.velocity + new Vector2(0, -curveSpeed);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else if (rb.rotation <= -50)
            {
                Vector3 temp = rb.velocity + new Vector2(curveSpeed, -curveSpeed);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
            else
            {
                Vector3 temp = rb.velocity + new Vector2(curveSpeed, 0);
                rb.velocity = Vector2.zero;
                rb.AddForce(temp.normalized * speed, ForceMode2D.Impulse);
            }
        }
    }

    public void Fire(Rigidbody2D rigidbody)
    {
        //if (isGhost)
        //{
        //    speed = 200;
        //}

        Vector3 dir = (new Vector2(0, 10)).normalized;
        Vector2 velocity = new Vector2(dir.x, dir.y) * speed;
        rigidbody.AddForce(velocity, ForceMode2D.Impulse);

        

        canFire = false;
    }

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
    }

    private void ResetPos()
    {
        rb.velocity = new Vector2(0, 0);
        transform.position = startPos;
        canFire = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            //If bottom wall, reset
            if (collision.gameObject.name == "BottomWall")
            {
                //ResetPos();
            }
            //Otherwise change color
            else
            {
                ChangeColor();
            }
            
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
