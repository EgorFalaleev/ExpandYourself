using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Range (0,1)]
    [SerializeField] float touchMovementSpeed = 1f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float scalePerFrameDifferenceFactor = 0.0005f;
    [SerializeField] float scaleToLose = 0.2f;

    // cached references
    Rigidbody2D myRigidbody;
    SceneLoader sceneLoader;

    // state variables
    Vector2 screenBounds;
    float playerWidth;
    float playerHeight;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();

        // get components
        myRigidbody = GetComponent<Rigidbody2D>();
        
        // get screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        UpdatePlayerBounds();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // get input axis
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth),
                                         Mathf.Clamp(transform.position.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight));

        // change player's velocity depending on the input
        myRigidbody.velocity = new Vector2(xMovement * moveSpeed * Time.deltaTime, yMovement * moveSpeed * Time.deltaTime);

        Shrink();

        // touch movement
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // convert touch position to world coordinates
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            transform.position = Vector2.Lerp(transform.position, touchPosition, touchMovementSpeed * Time.deltaTime);
        }
    }

    public void IncreaseSize(float sizeIncreasingValue)
    {
        float scaleRelation = sizeIncreasingValue / transform.localScale.x;

        if (scaleRelation >= 10)
        {
            transform.localScale = new Vector2(transform.localScale.x + scaleRelation / 10,
                                               transform.localScale.y + scaleRelation / 10);
        }
        else if (scaleRelation < 10 && scaleRelation >= 1)
        {
            transform.localScale = new Vector2(transform.localScale.x + scaleRelation / 3f,
                                               transform.localScale.y + scaleRelation / 3f);
        }
        else
        {
            transform.localScale = new Vector2(transform.localScale.x + scaleRelation,
                                               transform.localScale.y + scaleRelation);
        }
        UpdatePlayerBounds();
        HandleMoveSpeed();
    }

    private void UpdatePlayerBounds()
    {
        // update player bounds
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    private void Shrink()
    {
        transform.localScale = new Vector2(transform.localScale.x - scalePerFrameDifferenceFactor * Time.deltaTime,
                                           transform.localScale.y - scalePerFrameDifferenceFactor * Time.deltaTime);

        UpdatePlayerBounds();
        HandleMoveSpeed();

        // lose if size is too small
        if (transform.localScale.x < scaleToLose) Defeat();
    }

    private void HandleMoveSpeed()
    {
        // slow the player down
        moveSpeed = Mathf.Exp(2.5f - transform.localScale.x) + 1;
        touchMovementSpeed = Mathf.Exp(-transform.localScale.x) / 10f;
        if (touchMovementSpeed > 1) touchMovementSpeed = 1;
    }

    private void Defeat()
    {
        Destroy(gameObject);

        // reset game session (score)
        FindObjectOfType<GameSession>().ResetGameSession();

        sceneLoader.LoadGameOverScene();
    }
}
