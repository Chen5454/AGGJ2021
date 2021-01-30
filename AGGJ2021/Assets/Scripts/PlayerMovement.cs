using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

   public CharacterController controller;
    public float speed;


    GameObject pushedItems;
    public bool heldByPlayer = false;
    public float turnSmoothTime=0.1f;
    float turnSmoothVelocity;
    private Animator anim;
    public bool dropNow = false;



    private void Start()
    {

        anim = GetComponent<Animator>();
        pushedItems = GameObject.Find("Player");
    }


    private void Update()
    {


        var direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * speed * Time.deltaTime);
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
                dropNow = !dropNow;
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
