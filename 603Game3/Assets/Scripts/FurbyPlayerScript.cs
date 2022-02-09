using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurbyPlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector3 myScreenPos;
    public bool canFire = true;
    public float speed = 5;
    public bool isGhost = false;

    //public Projection projection;

    private SpriteRenderer sr;
    private int currentColorNum = 0; //0 = Pink, 1 = Purple, 2 = Green, 3 = Blue
    public Color[] colors = new Color[4];

    private Vector3 startPos;

    //public bool testFurbyAsCircles;
    //private List<Transform> furbyBodies;
    //public float timeBetweenEachCircle = .5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        sr = GetComponent<SpriteRenderer>();
        sr.color = colors[currentColorNum];
        startPos = transform.position;
        //projection = GetComponent<Projection>();

        //if (testFurbyAsCircles)
        //{
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        furbyBodies.Add(transform.GetChild(i));
        //        Physics.IgnoreCollision(GetComponent<Collider>(), transform.GetChild(0).GetComponent<Collider>());
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (isGhost)
        {
            return;
        }
        

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
    }

    public void Fire(Rigidbody2D rigidbody)
    {
        //if (isGhost)
        //{
        //    speed = 200;
        //}

        Vector3 dir = (Input.mousePosition - myScreenPos).normalized;
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
                ResetPos();
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

    //public IEnumerator FireAdditionalCircles()
    //{
    //    foreach (Transform b in furbyBodies)
    //    {
    //        yield return new WaitForSeconds(timeBetweenEachCircle);
    //        Fire(b.GetComponent<Rigidbody2D>());
    //    }        
    //}
}
