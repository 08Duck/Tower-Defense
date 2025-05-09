using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testplayerhealth : MonoBehaviour
{
    private Health healthy; // Reference to the Health script

    void Start()
    {
        // Find the Health component on the same GameObject
        healthy = GetComponent<Health>();

        // If the script is on another GameObject, use this instead:
        // healthy = GameObject.Find("Player").GetComponent<Health>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (healthy != null)
            {
                healthy.TakeDamage(10);
            }
            else
            {
                Debug.LogError("Health script not found! Make sure this script is on the same GameObject as Health.");
            }
        }
    }
}


