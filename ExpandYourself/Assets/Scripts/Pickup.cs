using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float sizeIncreasingValue = 0.5f;
    
    // cached references
    Player player;

    void Start()
    {
        // get the player object
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        player.IncreaseSize(sizeIncreasingValue);
        player.UpdateBounds(sizeIncreasingValue);
    }
}
