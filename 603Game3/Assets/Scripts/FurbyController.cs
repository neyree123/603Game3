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
    public LayerMask wallMask;
    public LayerMask topMask;
    public LayerMask floorMask;
    public LayerMask ballMask;
    public LayerMask bulletMask;
    public LayerMask laserMask;

    private bool hitByLaserRecently;
    public AudioClip[] wallHits;
    public AudioClip bulletHit;
    private AudioSource source;

    public float speed = 5;
    public float curveSpeed = .01f;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        tail = GetComponent<follow>();
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(0, ySpeed); // launch upwards

        health = maxHealth;

        startY = healthWaves.transform.position.y;
        targetY = startY;
        //healthBar.fillMethod = Image.FillMethod.Horizontal;
        //healthBar.fillOrigin = (int)Image.OriginHorizontal.Left;

        Vector3 dir = (new Vector2(0, 10)).normalized;
        Vector2 velocity = new Vector2(dir.x, dir.y) * speed;
        rb.AddForce(velocity, ForceMode2D.Impulse);

        //rb.AddForce(new Vector2(0, speed), ForceMode2D.Impulse);
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

        //Handles Wave Health Bar Change
        targetY = startY - ((maxHealth - health) * (yBounds / maxHealth));

        if (healthWaves.transform.position.y > targetY)
        {
            healthWaves.transform.position -= new Vector3(0, healthUISpeed, 0);
            healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
        }

        //healthWaves.transform.position = new Vector3(healthWaves.transform.position.x, startY - ((maxHealth - health) * (yBounds / maxHealth)));

        //float xDir = 0f;
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    xDir -= 1f;
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    xDir += 1f;
        //}

        //Rotate towards direction of movement
        Vector2 moveDir = rb.velocity;
        if (moveDir != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
            transform.rotation = toRotation;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
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
        else if (Input.GetKey(KeyCode.RightArrow))
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

        //float xVel = rb.velocity.x;
        //xVel = Mathf.Lerp(xVel, xDir * maxXSpeed, xLerp * Time.deltaTime);
        //rb.velocity = new Vector2(xVel, ySpeed);
        //
        //transform.up = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (wallMask == (wallMask | (1 << collision.gameObject.layer)))
        {
            tail.newColor = (tail.newColor + 1) % 4;
            WallColorChangeScript.instance.ChangeWallColor();
            //wall-hit audio 
            //wallhit1-001
            source.clip = wallHits[Random.Range(0, wallHits.Length)];
            source.Play();

        }
        else if (floorMask == (floorMask | (1 << collision.gameObject.layer)))
        {
            //ySpeed *= -1;
            tail.newColor = (tail.newColor + 1) % 4;
            WallColorChangeScript.instance.ChangeWallColor();
            //wall-hit audio 
            //wallhit1-001
            source.clip = wallHits[Random.Range(0, wallHits.Length)];
            source.Play();
        }
        else if (ballMask == (ballMask | (1 << collision.gameObject.layer)))
        {
            //ySpeed *= -1;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if (topMask == (topMask | (1 << collision.gameObject.layer)))
        //{
        //    //TriggerWin
        //    Debug.Log("Furby Win");
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
        if (bulletMask == (bulletMask | (1 << collision.gameObject.layer)))
        {
            source.clip = bulletHit;
            source.Play();
            health--;
            Destroy(collision.gameObject);
            //play bullet hit sound 
            //Furby_Bubble_Hit1
        }
        else if (laserMask == (laserMask | (1 << collision.gameObject.layer)))
        {
            if (!hitByLaserRecently)
            {
                health--;
                StartCoroutine(laserDamageCooldown());
                collision.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator laserDamageCooldown()
    {
        hitByLaserRecently = true;
        yield return new WaitForSeconds(1f);
        hitByLaserRecently = false;
    }
}
