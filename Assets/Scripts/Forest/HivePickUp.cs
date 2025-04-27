using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HivePickUp : MonoBehaviour
{
    public delegate void PickUpHive();
    public static event PickUpHive HivePickedUp;
    public static bool pickedUp = false;
    public static bool dropped = false;

    private void Start()
    {
        NavPlayerMovement.DroppedHive += OnHiveDrop;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider) && !pickedUp && !dropped)
        {
            HivePickedUp?.Invoke();
            gameObject.SetActive(false);
            pickedUp = true;
        }
    }

    void OnHiveDrop(Vector3 position)
    {
        if (pickedUp == false)
            return;
        transform.position = position;
        gameObject.SetActive(true);
        dropped = true;
        pickedUp = false;
    }
}
