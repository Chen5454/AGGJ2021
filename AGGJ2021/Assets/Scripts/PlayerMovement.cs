using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    public GameObject pushedItems;
    bool heldByPlayer = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = input.normalized * speed;
        rb.velocity = velocity; // Using RigidBody velocity instead of generic Transform in order to prevent jitter when interacting with physics objects
        rb.rotation = Quaternion.LookRotation(input);


        if (heldByPlayer)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                pushedItems.transform.position = gameObject.transform.position;

            }
          
        }
    }


    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "PChair")
        {

            heldByPlayer = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PChair")
        {

            heldByPlayer = false;

        }
    }
}
