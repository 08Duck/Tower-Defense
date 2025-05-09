using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTODooRx : MonoBehaviour
{
    public Transform DoorY;
    public Transform Player;
    private bool isPlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Player.position = DoorY.position;
        }
    }
}
