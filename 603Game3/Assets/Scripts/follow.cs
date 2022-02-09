using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class follow : MonoBehaviour
{
    private List<Vector4> trail;
    public float targetTime;
    public float memory;
    public follow next;

    // Start is called before the first frame update
    void Start()
    {
        trail = new List<Vector4>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTrail();

        // move the head towards the next in the chain
        if (next != null)
        {
            int index = 0;
            while (index < next.trail.Count && next.trail[index].w > targetTime) { index++; }
            Vector3 newPos = (Vector3)next.trail[index];
            newPos.z = 0.2f;
            transform.position = newPos;
        }

    }


    void updateTrail()
    {
        // logs where this ball has been
        Vector4 data = new Vector4(transform.position.x, transform.position.y, transform.position.z, 0);
        for (int i = 0; i < trail.Count; i++)
        {
            trail[i] += new Vector4(0,0,0,Time.deltaTime);
        }
        trail.Add(data);

        // removes parts of the trail that exceed the memory
        while (trail[0].w > memory)
        {
            trail.RemoveAt(0);
        }
    }
}
