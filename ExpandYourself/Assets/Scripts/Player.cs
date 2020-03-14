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

    void Start()
    {
        // get components
        myRigidbody = GetComponent<Rigidbody2D>();

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
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
            mousePosition.x = Mathf.Clamp(mousePosition.x, -screenBounds.x, screenBounds.x);
            mousePosition.y = Mathf.Clamp(mousePosition.y, -screenBounds.y, screenBounds.y);

            // smoothly move player from his position to mouse position
            transform.position = Vector2.Lerp(transform.position, mousePosition, mouseMovementSpeed);
        }
        else
        {
            // get input axis
            float xMovement = Input.GetAxis("Horizontal");
            float yMovement = Input.GetAxis("Vertical");

            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -screenBounds.x, screenBounds.x),
                                             Mathf.Clamp(transform.position.y, -screenBounds.y, screenBounds.y));

            // change player's velocity depending on the input
            myRigidbody.velocity = new Vector2(xMovement * moveSpeed, yMovement * moveSpeed);
        }
    }

    public void IncreaseSize(float sizeIncreasingValue)
    {
        transform.localScale = new Vector2(transform.localScale.x + sizeIncreasingValue / transform.localScale.x,
                                           transform.localScale.y + sizeIncreasingValue / transform.localScale.y);
    }
}
