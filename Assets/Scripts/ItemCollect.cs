using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ItemCollect : NetworkBehaviour
{
    public delegate void CollectItem(Item.VegetableType item);
    private Collider collidingItem;
    
    private Dictionary<Item.VegetableType, int> inventory = new Dictionary<Item.VegetableType, int>();
    public static event CollectItem ItemCollected;

    private void Start()
    {
        //populate inventory dictionary with vegetable types and their counts (0 by default)
        foreach (Item.VegetableType type in System.Enum.GetValues(typeof(Item.VegetableType)))
        {
            inventory.Add(type, 0);
        }
    }

    void Update()
    {
        if(collidingItem != null && Input.GetKeyDown(KeyCode.Space))
        {
            Item item = collidingItem.gameObject.GetComponent<Item>();
            AddItemToInventory(item);
            ItemCollected?.Invoke(item.typeOfVeggie);
            PrintInventory();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!IsServer)
        {
            return;
        }

        if (collider.CompareTag("Item"))
        {
            collidingItem = collider;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (!IsServer)
        {
            return;
        }

        if (collider.CompareTag("Item"))
        {
            collidingItem = null;
        }
    }

    private void PrintInventory()
    {
        string output = "";
        foreach (KeyValuePair<Item.VegetableType, int> pair in inventory)
        {
            output += string.Format("{0}: {1}; ", pair.Key, pair.Value);
        }
        Debug.Log(output);
    }

    private void AddItemToInventory(Item item)
    {
        inventory[item.typeOfVeggie]++;
    }

}
