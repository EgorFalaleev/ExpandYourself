using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float mouseMovementSpeed = 1f;
    [SerializeField] bool mouseMovement;
    [SerializeField] float leftBound = -8.3f;
    [SerializeField] float rightBound = 8.3f;
    [SerializeField] float bottomBound = -4.4f;
    [SerializeField] float upperBound = 4.4f;

    // cached references
    Rigidbody2D myRigidbody;

    void Start()
    {
        // get components
        myRigidbody = GetComponent<Rigidbody2D>();
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
            mousePosition.x = Mathf.Clamp(mousePosition.x, leftBound, rightBound);
            mousePosition.y = Mathf.Clamp(mousePosition.y, bottomBound, upperBound);

            // smoothly move player from his position to mouse position
            transform.position = Vector2.Lerp(transform.position, mousePosition, mouseMovementSpeed);
        }
        else
        {
            // get input axis
            float xMovement = Input.GetAxis("Horizontal");
            float yMovement = Input.GetAxis("Vertical");

            transform.position = new Vector2(Mathf.Clamp(transform.position.x, leftBound, rightBound), Mathf.Clamp(transform.position.y, bottomBound, upperBound));

            // change player's velocity depending on the input
            myRigidbody.velocity = new Vector2(xMovement * moveSpeed, yMovement * moveSpeed);
        }
    }

    public void IncreaseSize(float sizeIncreasingValue)
    {
        transform.localScale = new Vector2(transform.localScale.x + sizeIncreasingValue, transform.localScale.y + sizeIncreasingValue);
        UpdateBounds(sizeIncreasingValue);
    }

    // when player becomes bigger, screen bounds should be updated to prevent him from going off screen
    public void UpdateBounds(float sizeIncreasingValue)
    {
        leftBound += sizeIncreasingValue / 4;
        rightBound -= sizeIncreasingValue / 4;
        upperBound -= sizeIncreasingValue / 4;
        bottomBound += sizeIncreasingValue / 4;
    }
}
