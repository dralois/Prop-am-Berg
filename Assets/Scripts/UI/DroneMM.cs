using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class DroneMM : MonoBehaviour
{
    public float maxHeight;
    public float minHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hoverHeight = (maxHeight + minHeight) / 2.0f;
        float hoverRange = maxHeight - minHeight;
        float hoverSpeed = 10.0f;

        this.transform.position = Vector3.up * (hoverHeight + Mathf.Cos(Time.time * hoverSpeed/10f) * hoverRange) + new Vector3(0f, 0, -2f);
    }

}
