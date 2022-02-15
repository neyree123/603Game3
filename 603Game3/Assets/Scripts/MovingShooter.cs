using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShooter : MonoBehaviour
{
    [SerializeField] private float xMin; //Left bound of movement
    [SerializeField] private float xMax; //Right bound of movement
    [SerializeField] private float speed;
    [SerializeField] private float chargeTime = 3f;
    [SerializeField] private float timeBetweenBullets = 2f;
    private float chargeTimer = 0;
    private float attackTimer = 0;


    public GameObject bullet;
    private Transform bulletHolder;

    public GameObject chargeBarParent;
    public GameObject chargeBar;

    private Vector3 barLocalScale;

    // Start is called before the first frame update
    void Start()
    {
        bulletHolder = GameObject.Find("BulletHolder").transform;


        chargeBarParent = transform.GetChild(0).gameObject;
        chargeBar = chargeBarParent.transform.GetChild(1).gameObject;

        barLocalScale = chargeBar.transform.localScale;

        chargeBarParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ManagerActions.paused)
        {
            attackTimer += Time.deltaTime;

            if (Input.GetKey(KeyCode.D))
            {
                float moveDist = speed * Time.deltaTime;
                float xPos = transform.position.x + moveDist;
                if (xPos > xMax)
                {
                    xPos = xMax;
                }
                transform.position = new Vector2(xPos, transform.position.y);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                float moveDist = speed * Time.deltaTime;
                float xPos = transform.position.x - moveDist;
                if (xPos < xMin)
                {
                    xPos = xMin;
                }
                transform.position = new Vector2(xPos, transform.position.y);
            }

            if (Input.GetKey(KeyCode.W) && attackTimer > timeBetweenBullets)
            {
                chargeTimer += Time.deltaTime;
                Debug.Log(chargeTimer);

                if (!chargeBarParent.activeInHierarchy)
                {
                    chargeBarParent.SetActive(true);
                }

                if (chargeTimer < chargeTime)
                {
                    chargeBar.transform.localScale = new Vector3((chargeTimer / chargeTime) * barLocalScale.x, barLocalScale.y, barLocalScale.z);
                }
                else
                {
                    chargeBar.transform.localScale = barLocalScale;
                }


            }

            if (Input.GetKeyUp(KeyCode.W) && attackTimer > timeBetweenBullets)
            {
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity, bulletHolder);

                if (chargeTimer >= chargeTime)
                {
                    b.GetComponent<BulletScript>().isCharged = true;
                }

                chargeBarParent.SetActive(false);
                chargeTimer = 0;
                attackTimer = 0;
            }
        }
    }
}
