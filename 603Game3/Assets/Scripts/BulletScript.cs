using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [HideInInspector] public bool isCharged = false;
    public LayerMask topZoneMask;



    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (topZoneMask == (topZoneMask | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    /*void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }*/
}
