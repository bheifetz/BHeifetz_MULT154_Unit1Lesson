using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] lilyPads;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnLilyPad", 0.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnLilyPad()
    {
        foreach(GameObject lilyPad in lilyPads)
            Instantiate(lilyPad);
    }
}
