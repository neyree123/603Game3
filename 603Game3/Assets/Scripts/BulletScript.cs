using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, speed);
    }

    // Update is called once per frame
    /*void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }*/
}
