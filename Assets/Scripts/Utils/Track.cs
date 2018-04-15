using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    public GameObject target;
    public GameObject target2;
    public float distance = 15;
    public float lerpFactor = 0.1f;

    public bool isSmooth = true;

    // Use this for initialization
    void Start() { }


    // Update is called once per frame
    void Update()
    {
        if (!isSmooth)
        {
            transform.position = computeTargetPosition();
        }
    }

    void FixedUpdate()
    {
        if (isSmooth)
        {
            transform.position = Vector3.Lerp(transform.position, computeTargetPosition(), lerpFactor);
        }
    }

    private Vector3 computeTargetPosition()
    {
        if (target2 == null)
        {
            return target.transform.position + (Vector3.back * distance);
        }
        else
        {
            Vector2 pos1 = target.transform.position;
            Vector2 pos2 = target2.transform.position;

            Vector2 average = (pos1 + pos2) / 2;

            return new Vector3(average.x, average.y, -distance);
        }
    }
}
