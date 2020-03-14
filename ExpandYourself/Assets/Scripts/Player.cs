using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float mouseMovementSpeed = 1f;
    [SerializeField] bool mouseMovement;

    // cached references
    Rigidbody2D myRigidbody;

    // state variables
    Vector2 screenBounds;
    float playerWidth;
    float playerHeight;

    void Start()
    {
        // get components
        myRigidbody = GetComponent<Rigidbody2D>();
        
        // get screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // set player sizes
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (mouseMovement)
        {
            // get mouse position
            Vector2 mousePosition = Input.mousePosition;

            // convert screen mouse position to world space
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // make the block move inside a screen
            mousePosition.x = Mathf.Clamp(mousePosition.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
            mousePosition.y = Mathf.Clamp(mousePosition.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);

            // smoothly move player from his position to mouse position
            transform.position = Vector2.Lerp(transform.position, mousePosition, mouseMovementSpeed);
        }
        else
        {
            // get input axis
            float xMovement = Input.GetAxis("Horizontal");
            float yMovement = Input.GetAxis("Vertical");

            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth),
                                             Mathf.Clamp(transform.position.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight));

            // change player's velocity depending on the input
            myRigidbody.velocity = new Vector2(xMovement * moveSpeed, yMovement * moveSpeed);
        }
    }

    public void IncreaseSize(float sizeIncreasingValue)
    {
        transform.localScale = new Vector2(transform.localScale.x + sizeIncreasingValue / transform.localScale.x,
                                           transform.localScale.y + sizeIncreasingValue / transform.localScale.y);

        // update player bounds
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }
}
