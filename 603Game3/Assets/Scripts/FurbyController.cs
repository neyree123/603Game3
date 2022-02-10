using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurbyController : MonoBehaviour
{
    private follow tail;
    private Rigidbody2D rb;

    public float ySpeed;
    public float maxXSpeed;
    public float xLerp;


    // Start is called before the first frame update
    void Start()
    {
        tail = GetComponent<follow>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, ySpeed); // launch upwards
    }

    // Update is called once per frame
    void Update()
    {
        float xDir = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            xDir -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            xDir += 1f;
        }

        float xVel = rb.velocity.x;
        xVel = Mathf.Lerp(xVel, xDir * maxXSpeed, xLerp * Time.deltaTime);
        rb.velocity = new Vector2(xVel, ySpeed);

        transform.up = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            tail.newColor = (tail.newColor + 1) % 4;
        }
        if (collision.transform.tag == "Floor")
        {
            ySpeed *= -1;
            tail.newColor = (tail.newColor + 1) % 4;
        }
        if (collision.transform.tag == "Ball")
        {
            ySpeed *= -1;
        }
    }
}
