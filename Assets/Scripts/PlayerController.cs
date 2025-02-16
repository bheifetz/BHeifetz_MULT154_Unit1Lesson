using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rbPlayer;
    private Vector3 direction = Vector3.zero;
    private float forceMultiplier = 40.0f;
    public GameObject spawnPoint;
    private Dictionary<Item.VegetableType, int> inventory = new Dictionary<Item.VegetableType, int>();

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();

        //populate inventory dictionary with vegetable types and their counts (0 by default)
        foreach(Item.VegetableType type in System.Enum.GetValues(typeof(Item.VegetableType)))
        {
            inventory.Add(type, 0);
        }
    }

    void Update() //for non-physics-related functions
    {
        float xVelocity = Input.GetAxis("Horizontal");
        float zVelocity = Input.GetAxis("Vertical");

        direction = new Vector3(xVelocity, 0, zVelocity);
    }

    // FixedUpdate is called once per frame, along with Unity's physics engine
    void FixedUpdate() //for physics-related functions
    {
        rbPlayer.AddForce(direction * forceMultiplier, ForceMode.Impulse);

        if (transform.position.z > 38.5f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 38.5f);
        else if (transform.position.z < -38.5f)
            transform.position = new Vector3(transform.position.x, transform.position.y, -38.5f);
    }

    private void Respawn()
    {
        transform.position = spawnPoint.transform.position;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Item"))
        {
            Item item = collider.gameObject.GetComponent<Item>();
            AddItemToInventory(item);
            PrintInventory();
        }
    }

    private void PrintInventory()
    {
        string output = "";
        foreach(KeyValuePair<Item.VegetableType, int> pair in inventory)
        {
            output += string.Format("{0}: {1}; ", pair.Key, pair.Value);
        }
        Debug.Log(output);
    }

    private void AddItemToInventory(Item item)
    {
        inventory[item.typeOfVeggie]++;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Hazard"))
            Respawn();
    }
}
