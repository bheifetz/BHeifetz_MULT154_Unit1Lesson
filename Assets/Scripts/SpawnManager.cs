using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnManager : NetworkBehaviour
{
    public GameObject[] lilyPads;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        InvokeRepeating("SpawnLilyPad", 0.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnLilyPad()
    {
        foreach(GameObject lilyPad in lilyPads)
        {
            NetworkObject lilyPadObject = Instantiate(lilyPad).GetComponent<NetworkObject>();
            lilyPadObject.Spawn();
        }
    }
}
