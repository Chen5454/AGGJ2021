using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSChecker : MonoBehaviour
{
    public Transform Target;

    public float coneAngle;

    public float MaxMagnitude;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HasConeOfSight();
        }
    }

    public bool HasConeOfSight()
    {
        Debug.Log("orig position: " + transform.position);
        Debug.Log("target position: " + Target.position);

        Vector3 dist = transform.position - Target.position;

        Debug.Log("distance: " + dist);

        Vector3 forward = transform.rotation * Vector3.forward;

        Debug.Log("forward: " + forward);

        Debug.Log("dist magnitude: " + dist.magnitude);

        if (dist.magnitude <= MaxMagnitude)
        {
            float angle = Vector3.SignedAngle(dist, forward,Vector3.up);

            angle += 180;
            if (angle >= 270)
            {
                angle = Math.Abs(angle - 360);
            }

            Debug.Log("angle: " + angle);

            if (angle <= coneAngle)
            {
                return true;
            }
        }


        return false;
    }


    public bool HasLineOfSight()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);

        //int layerMask = PhaseThroughWalls ? ~(1 << 11) : ~1;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
