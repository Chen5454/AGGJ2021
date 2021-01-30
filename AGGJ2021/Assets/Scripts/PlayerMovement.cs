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



    private void Start()
    {

        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
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


        if (heldByPlayer)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                speed = 3;
                pushedItems.transform.SetParent(gameObject.transform);
                anim.SetBool("IsPushing", true);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                speed = 5;
                pushedItems.transform.SetParent(null);
                anim.SetBool("IsPushing", false);
                

            }

        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
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


    private void OnCollisionEnter(Collision collision)
    {

        if(GameObject.Find("Worlds Manager").GetComponent<WorldsManager>().isInRealWorld)
        {

            if (collision.gameObject.tag == "Real World Pickup")
            {
                heldByPlayer = true;
                pushedItems = collision.gameObject;
            }
        }
        else
        {
            if (collision.gameObject.tag == "Lost World Pickup")
            {
                heldByPlayer = true;
                pushedItems = collision.gameObject;
            }
        }

    }



    private void OnCollisionExit(Collision collision)
    {

        if (GameObject.Find("Worlds Manager").GetComponent<WorldsManager>().isInRealWorld)
        {

            if (collision.gameObject.tag == "Real World Pickup")
            {
                heldByPlayer = false;
            }
        }
        else
        {
            if (collision.gameObject.tag == "Lost World Pickup")
            {
                heldByPlayer = false;
            }
        }
    }


}
