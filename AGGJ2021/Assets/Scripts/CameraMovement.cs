using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject player;
    public float cameraSpeed = 0.1f;
    Vector3 cameraTarget;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); // Collecting player object
        transform.position = player.transform.position + new Vector3(0, 10, 0); // Initial camera position
        transform.Rotate(90, 0, 0); // Looking straight down, this shouldn't change throughout the game
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraTarget = player.transform.position + new Vector3(0, 10, 0); // Directly above the player is always where we strive to be
        
        // The farther we are from the player, the faster the camera moves, the second argument is the maximum distance we want from the player:
        float speedModifier = Mathf.InverseLerp(0, 2, Vector3.Distance(transform.position, cameraTarget));
        
        // Moving camera to position
        transform.position = Vector3.MoveTowards(transform.position, cameraTarget, cameraSpeed * speedModifier);
    }
}
