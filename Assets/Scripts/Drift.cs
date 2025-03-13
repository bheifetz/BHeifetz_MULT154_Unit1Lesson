using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Drift : NetworkBehaviour
{
    private float speed = 10.0f;
    public enum DriftDirection
    {
        LEFT = -1,
        RIGHT = 1
    }
    public DriftDirection driftDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
            return;
        
        transform.Translate(Vector3.right * speed * Time.deltaTime * (int)driftDirection);
        if (Mathf.Abs(transform.position.x) > 80)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                NetworkObject player = transform.GetChild(i).gameObject.GetComponent<NetworkObject>();
                player.TryRemoveParent();
            }
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!IsServer)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            NetworkObject player = collision.gameObject.GetComponent<NetworkObject>();
            player.TrySetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!IsServer)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            NetworkObject player = collision.gameObject.GetComponent<NetworkObject>();
            player.TryRemoveParent();
        }
    }
}
