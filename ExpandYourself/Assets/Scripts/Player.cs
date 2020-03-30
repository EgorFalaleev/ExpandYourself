﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float scalePerFrameDifferenceFactor = 0.0005f;
    [SerializeField] float scaleToLose = 0.2f;

    // cached references
    SceneLoader sceneLoader;
    BoxCollider2D playerCollider;

    // state variables
    Vector2 screenBounds;
    float playerWidth;
    float playerHeight;
    float movementSpeed;
    bool dragging;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        playerCollider = GetComponent<BoxCollider2D>();

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
        // touch movement
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // convert touch position to world coordinates
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 touchPosition2D = new Vector2(touchPosition.x, touchPosition.y);

            // throw a ray from the touch position
            RaycastHit2D touchHit = Physics2D.Raycast(touchPosition2D, Vector2.zero);
           
            // if the ray collides with player, activate the dragging state
            if (touchHit.collider == playerCollider)
            {
                dragging = true;
            }
           
            // while dragging state is active move player to the touch position
            if (dragging)
            {
                Vector3 normalizedPosition = new Vector3(touchPosition.x, touchPosition.y, transform.position.z);
                transform.position = Vector2.MoveTowards(transform.position, normalizedPosition, movementSpeed * Time.deltaTime);
            }

            // when player releases finger deactivate dragging
            if (touch.phase == TouchPhase.Ended) dragging = false;
        }

        Shrink();
    }

    public void IncreaseSize(float sizeIncreasingValue)
    {
        float scaleRelation = sizeIncreasingValue / transform.localScale.x;

        // increase size by different value depending on the scale relation between pickup and player
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

    public void DecreaseSize(float valueToDecrease)
    {
        transform.localScale = new Vector2(transform.localScale.x - valueToDecrease, transform.localScale.y - valueToDecrease);

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
        // speed-size relation
        movementSpeed = (Mathf.Exp(2.5f - transform.localScale.x) + 1);
    }

    private void Defeat()
    {
        Destroy(gameObject);

        // reset game session (score)
        FindObjectOfType<GameSession>().ResetGameSession();

        sceneLoader.LoadGameOverScene();
    }

    public void AcceleratePlayerShrinking(float value)
    {
        scalePerFrameDifferenceFactor += value;
    }

    // mouse movement
    private void OnMouseDrag()
    {
        // convert mouse position from screen space to world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // move player to the mouse position
        Vector3 normalizedPosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, movementSpeed * Time.deltaTime);
    }
}
