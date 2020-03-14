using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float scaleToDestroy = 0.01f;
    [SerializeField] float scalePerFrameDivideFactor = 1000f;

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

    private void Update()
    {
        Shrink();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        player.IncreaseSize(circleScale.x);
    }

    private void Shrink()
    {
        var scaleDifferenceX = transform.localScale.x / scalePerFrameDivideFactor;
        var scaleDifferenceY = transform.localScale.y / scalePerFrameDivideFactor;

        transform.localScale = new Vector2(transform.localScale.x - scaleDifferenceX, transform.localScale.y - scaleDifferenceY);
        circleScale = transform.localScale;

        // destroy pickup if it becomes too small
        if (transform.localScale.x < scaleToDestroy || transform.localScale.y < scaleToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
