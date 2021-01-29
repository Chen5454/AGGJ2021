using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed;
    //Rigidbody rb;
    public GameObject pushedItems;
     public bool heldByPlayer = false;
   public float turnSmoothTime=0.1f;
    float turnSmoothVelocity;
    private void Start()
    {
       // rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
       //  Vector3 velocity = input.normalized * speed;
        if (input.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
          //  rb.velocity = velocity; // Using RigidBody velocity instead of generic Transform in order to prevent jitter when interacting with physics objects

            transform.rotation = Quaternion.Euler(0f, angle,0f);
            controller.Move(input * speed * Time.deltaTime);
        }


        if (heldByPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {

                pushedItems.transform.SetParent(gameObject.transform);

            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                pushedItems.transform.SetParent(null);
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
