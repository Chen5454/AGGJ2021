using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = input.normalized * speed;
        transform.position += velocity * Time.deltaTime;
    }
}
