using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushedItems : MonoBehaviour
{

   public PlayerMovement playerTrigger;

   public Collider itemTrigger;
  // public Collider itemColl;


    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            itemTrigger.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
        }
    }


}
