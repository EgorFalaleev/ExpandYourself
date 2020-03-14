using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // cached references
    Player player;

    // state variables
    private Vector2 circleScale;

    void Start()
    {
        // get the player object
        player = FindObjectOfType<Player>();

        circleScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        player.IncreaseSize(circleScale.x);
    }

    public Vector2 GetCircleScale()
    {
        return circleScale;
    }
}
