using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FurbyController : MonoBehaviour
{
    private follow tail;
    private Rigidbody2D rb;

    public float ySpeed;
    public float maxXSpeed;
    public float xLerp;

    public float maxHealth = 10;
    public float health;

    public GameObject healthWaves;
    public float yBounds = 9;
    public float healthUISpeed;
    private float startY;
    private float targetY = .1f;

    public Image healthBar;


    // Start is called before the first frame update
    void Start()
    {
        tail = GetComponent<follow>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, ySpeed); // launch upwards

        health = maxHealth;

        startY = healthWaves.transform.position.y;
        targetY = startY;
        //healthBar.fillMethod = Image.FillMethod.Horizontal;
        //healthBar.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //Shooter Wins
            Debug.Log("Shooter Win");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }

        targetY = startY - ((maxHealth - health) * (yBounds / maxHealth));

        Debug.Log(targetY);

        if (healthWaves.transform.position.y > targetY)
        {
            healthWaves.transform.position -= new Vector3(0, healthUISpeed, 0);
            healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
        }

        //healthWaves.transform.position = new Vector3(healthWaves.transform.position.x, startY - ((maxHealth - health) * (yBounds / maxHealth)));

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            health--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            tail.newColor = (tail.newColor + 1) % 4;
            WallColorChangeScript.instance.ChangeWallColor();
        }
        else if (collision.transform.tag == "Floor")
        {
            ySpeed *= -1;
            tail.newColor = (tail.newColor + 1) % 4;
            WallColorChangeScript.instance.ChangeWallColor();
        }
        else if (collision.transform.tag == "Ball")
        {
            ySpeed *= -1;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Top")
        {
            //TriggerWin
            Debug.Log("Furby Win");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
