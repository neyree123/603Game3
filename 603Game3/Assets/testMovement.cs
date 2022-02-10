using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 30, Space.World);
        transform.Translate(0, Time.deltaTime * 3, 0, Space.Self);
    }
}
