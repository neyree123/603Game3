using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class follow : MonoBehaviour
{
    private List<Vector4> trail;

    public GameObject body;

    public float targetTime;
    public float memory;

    public follow next;
    public follow last;

    public int newTails;
    public int removeTails;

    private float addWaiter;

    // Start is called before the first frame update
    void Start()
    {
        trail = new List<Vector4>();
        addWaiter = -1;
    }

    // Update is called once per frame
    void Update()
    {
        updateTrail();
        moveForward();

        if (newTails > 0 && addWaiter < 0)
        {
            addWaiter = targetTime;
            newTails--;
            addTail();
        }
        if (addWaiter >= 0)
        {
            addWaiter -= Time.deltaTime;
        }

        if (removeTails > 0)
        {
            removeTails--;
            removeTail();
        }
    }

    public void moveForward()
    {
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

    follow getTail()
    {
        // get the last tail segment
        follow tail = this;
        while (tail.last != null) { tail = tail.last; }
        return tail;
    }

    public void removeTail()
    {
        // get the last tail segment
        follow tail = getTail();

        // if prevents head from being destroyed
        if (tail.next != null)
        {
            tail.next.last = null;
            Destroy(tail.gameObject);
        }
    }

    public void addTail()
    {
        // get the last tail segment
        follow tail = getTail();

        // make a new tail
        GameObject newTail = Instantiate(body);
        newTail.transform.position = tail.transform.position;

        // set up the script
        follow newTailScript = newTail.GetComponent<follow>();
        newTailScript.targetTime = targetTime;
        newTailScript.memory = memory;
        newTailScript.next = tail;
        tail.last = newTailScript;
    }
}
