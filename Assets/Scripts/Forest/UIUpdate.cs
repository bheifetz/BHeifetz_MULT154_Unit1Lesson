using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdate : MonoBehaviour
{
    TextMeshProUGUI textGUI;

    // Start is called before the first frame update
    void Start()
    {
        textGUI = GetComponent<TextMeshProUGUI>();
        HivePickUp.HivePickedUp += HiveOnPlayer;
        NavPlayerMovement.DroppedHive += HiveOffPlayer;
    }

    void HiveOnPlayer()
    {
        textGUI.text = "Holding Hive";
    }

    void HiveOffPlayer(Vector3 v)
    {
        textGUI.text = string.Empty;
    }
}
