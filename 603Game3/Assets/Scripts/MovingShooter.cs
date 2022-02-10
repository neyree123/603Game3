using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShooter : MonoBehaviour
{
    [SerializeField] private float xMin; //Left bound of movement
    [SerializeField] private float xMax; //Right bound of movement
    [SerializeField] private float speed;
    public GameObject bullet;
    private Transform bulletHolder;
    // Start is called before the first frame update
    void Start()
    {
        bulletHolder = GameObject.Find("BulletHolder").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            float moveDist = speed * Time.deltaTime;
            float xPos = transform.position.x + moveDist;
            if(xPos > xMax)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, transform.position, Quaternion.identity, bulletHolder);
        }
    }
}
