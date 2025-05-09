using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's position
    public float speed = 3f;  // Movement speed
    public float DetectPlayerRange = 10;

    void Start()
    {
        // Find the player by tag (Make sure your player has the "Player" tag!)
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        float distance = Vector3.Distance(transform.position, player.position);
        if (player != null)
        {
            if (distance < DetectPlayerRange)
            {
                // Move towards the player's position
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
               
            }
        }
    }
}
