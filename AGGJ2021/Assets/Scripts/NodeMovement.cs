﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class NodeMovement : MonoBehaviour
{
    public NodePath path;

    public float MoveSpeed;


    float Timer;
    Vector3 currNode;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateNodePos();
    }

    public void updateNodePos()
    {
        currNode = path.getCurrNode();

        Timer += Time.deltaTime * MoveSpeed;

        if ((transform.position - currNode).magnitude > 0.01)
        {
            Vector3 currPos = transform.position;
            Vector3 step = (currNode - currPos).normalized * MoveSpeed * Time.deltaTime;

            transform.position += step;

            
        }
        else
        {
            transform.position = currNode;
            Timer = 0;
            path.MoveToNextNode();


            Vector3 diff = (path.getCurrNode() - transform.position).normalized;

            float angle = Vector3.SignedAngle(Vector3.forward, diff, Vector3.up);
            //angle += 90;

            transform.rotation = Quaternion.Euler(0, angle, 0);

        }
    }

    
}
