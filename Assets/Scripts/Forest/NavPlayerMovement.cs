using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotationSpeed = 60.0f;
    private Rigidbody playerRb = null;
    private float translationValue = 0;
    private float rotateValue = 0;

    public delegate void DropHive(Vector3 position);
    public static event DropHive DroppedHive;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        translationValue = translation;
        rotateValue = rotation;

        if (Input.GetKeyDown(KeyCode.Space))
            DroppedHive.Invoke(transform.position);
    }

    private void FixedUpdate()
    {
        // rotates the player
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += rotateValue * rotationSpeed * Time.deltaTime;
        playerRb.MoveRotation(Quaternion.Euler(rot));
        
        // simply moves the player by however much the player is pressing with respect
        // to the speed parameter. Does not affect gravity.
        Vector3 move = transform.forward * translationValue;
        playerRb.velocity = new Vector3(move.x * speed, playerRb.velocity.y, move.z * speed);
    }
}
