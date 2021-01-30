using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    CharacterController controller;
    public float speed;


    Rigidbody rb;
    GameObject pushedItems;
    public bool heldByPlayer = false;
    public float turnSmoothTime=0.1f;
    float turnSmoothVelocity;
    private Animator anim;
    public bool dropNow = false;



    private void Start()
    {

        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
        pushedItems = GameObject.Find("Player");
    }


    private void Update()
    {


        var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = input.normalized * speed;
        rb.velocity = velocity; // Using RigidBody velocity instead of generic Transform in order to prevent jitter when interacting with physics objects
        if (input.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);




            rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
            anim.SetBool("Walking",true);
           // anim.SetTrigger("DoWalk");
        }
        else
        {
            anim.SetBool("Walking",false);

        }


        //Debug.Log("space: " + Input.GetKey(KeyCode.Space) + " colliding: " + heldByPlayer + " transition: " + dropNow);
        if (Input.GetKey(KeyCode.Space) && heldByPlayer && !dropNow)
        {
            speed = 3;
            float fade = GameObject.Find("Worlds Manager").GetComponent<WorldsManager>().fade;
            if(pushedItems.tag == "Real World Pickup" && fade == 1)
            {
                pushedItems.transform.SetParent(gameObject.transform);
            }
            else if (pushedItems.tag == "Lost World Pickup" && fade == 0)
            {
                pushedItems.transform.SetParent(gameObject.transform);
            }
            else
            {
                pushedItems.transform.SetParent(null);
            }
            anim.SetBool("IsPushing", true);
        }

        else
        {
            speed = 5;
            pushedItems.transform.SetParent(null);
            anim.SetBool("IsPushing", false);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = 1;
            anim.SetBool("IsCrouch", true);

        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("IsCrouch", false);
            speed = 5;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(GameObject.Find("Worlds Manager").GetComponent<WorldsManager>().fade == 1)
        {
            if (collision.gameObject.tag == "Real World Pickup")
            {

                heldByPlayer = true;
                if (pushedItems != collision.gameObject)
                {
                    pushedItems.transform.SetParent(null);
                }
                pushedItems = collision.gameObject;
            }
        }
        else if (GameObject.Find("Worlds Manager").GetComponent<WorldsManager>().fade == 0)
        {
            if (collision.gameObject.tag == "Lost World Pickup")
            {
                heldByPlayer = true;
                if (pushedItems != collision.gameObject)
                {
                    pushedItems.transform.SetParent(null);
                }
                pushedItems = collision.gameObject;
            }
        }

    }
    private void OnTriggerExit(Collider collision)
    {
        if (GameObject.Find("Worlds Manager").GetComponent<WorldsManager>().isInRealWorld)
        {

            if (collision.gameObject.tag == "Real World Pickup")
            {
                heldByPlayer = false;
                pushedItems = GameObject.Find("Player");
            }
        }
        else
        {
            if (collision.gameObject.tag == "Lost World Pickup")
            {
                heldByPlayer = false;
                pushedItems = GameObject.Find("Player");
            }
        }
    }


}
