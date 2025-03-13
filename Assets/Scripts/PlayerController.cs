using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rbPlayer;
    private Vector3 direction = Vector3.zero;
    private float forceMultiplier = 40.0f;
    public GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        Respawn();
    }

    void Update() //for non-physics-related functions
    {
        if(!IsLocalPlayer)
        {
            return;
        }

        float xVelocity = Input.GetAxis("Horizontal");
        float zVelocity = Input.GetAxis("Vertical");

        direction = new Vector3(xVelocity, 0, zVelocity);
    }

    // FixedUpdate is called once per frame, along with Unity's physics engine
    void FixedUpdate() //for physics-related functions
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if (IsServer)
            Move(direction);
        else
            MoveRpc(direction);
    }

    private void Move(Vector3 input)
    {
        rbPlayer.AddForce(input * forceMultiplier, ForceMode.Impulse);

        if (transform.position.z > 38.5f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 38.5f);
        else if (transform.position.z < -38.5f)
            transform.position = new Vector3(transform.position.x, transform.position.y, -38.5f);
    }

    [Rpc(SendTo.Server)]
    public void MoveRpc(Vector3 input)
    {
        Move(input);
    }

    private void Respawn()
    {
        int index = 0;
        while (Physics.CheckBox(spawnPoints[index].transform.position, new Vector3(1.5f, 1.5f, 1.5f)))
            index++;
        transform.position = spawnPoints[index].transform.position;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if (collider.CompareTag("Hazard"))
            Respawn();
    }
}
