using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float scalePerFrameDifferenceFactor = 0.0005f;
    [SerializeField] float scaleToLose = 0.2f;
    [SerializeField] float bonusSize = 3f;
    [SerializeField] float bonusSizeIncreasingValue = 0.25f;
    [SerializeField] AudioClip loseSound;
    public bool tutorialMode = false;

    // cached references
    private SceneLoader sceneLoader;
    private GameSession gameSession;
    private PolygonCollider2D playerCollider;
    private Rigidbody2D playerRigidBody;
    private TrailRenderer trailRenderer;

    // state variables
    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;
    private float movementSpeed;
    private float movementSpeedPickupInfluence;
    private bool defeated = false;
    private bool dragging;
    private bool changeSpeedTaken = false;
    private bool isShrinking = true;

    private void Start()
    {
        // get components and references
        sceneLoader = FindObjectOfType<SceneLoader>();
        gameSession = FindObjectOfType<GameSession>();
        playerCollider = GetComponent<PolygonCollider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        // get screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // set player sprite depending on his choice in unlocks 
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/{PlayerPrefs.GetString("PlayerSprite", "Starting Starship")}");

        UpdatePlayerBounds();
        SetTrailColor();

        // turn on tutorial mode
        if (FindObjectOfType<TutorialManager>()) tutorialMode = true;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // touch movement
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            dragging = true;
            if (tutorialMode) FindObjectOfType<TutorialManager>().playerHasMoved = true;

            // convert touch position to world coordinates
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 touchPosition2D = new Vector2(touchPosition.x, touchPosition.y);

            // clamp touches inside game screen
            touchPosition.x = Mathf.Clamp(touchPosition.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
            touchPosition.y = Mathf.Clamp(touchPosition.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);

            RotateToTouchPosition(touchPosition);

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

    private void RotateToTouchPosition(Vector2 touchPos)
    {
        // find the direction vector 
        Vector2 direction = new Vector2(touchPos.x - transform.position.x, touchPos.y - transform.position.y);

        // "look" at the touch
        transform.up = direction;
    }

    public void IncreaseSize(float sizeIncreasingValue)
    {
        StartCoroutine(IncreaseSizeCoroutine(sizeIncreasingValue));
    }

    private IEnumerator IncreaseSizeCoroutine(float sizeIncreasingValue)
    {
        isShrinking = false;

        float scaleRelation = sizeIncreasingValue / transform.localScale.x;
        float increaseFactor = 10f;

        // increase size by different value depending on the scale relation between pickup and player
        if (scaleRelation >= 5)
        {
            increaseFactor = 10f;
        }
        else if (scaleRelation >= 1)
        {
            increaseFactor = 3f;
        }
        else
        {
            increaseFactor = 1.5f;
        }

        Vector2 currentScale = transform.localScale;
        float targetScale = transform.localScale.x + scaleRelation / increaseFactor;

        // if player reaches bonus size, don't increase more 
        if (targetScale >= bonusSize)
        {
            targetScale = bonusSize;
            gameSession.ProcessBonusSizeReached();
            bonusSize += bonusSizeIncreasingValue;
        }

        while (transform.localScale.x < targetScale - 0.01f)
        {
            currentScale = Vector2.Lerp(currentScale, new Vector2(targetScale, targetScale), 0.1f);
            transform.localScale = currentScale;
            UpdatePlayerBounds();
            HandleMoveSpeed();
            yield return null;
        }

        isShrinking = true;
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
        if (!isShrinking) return;

        if (!defeated)
        {
            if (!tutorialMode) transform.localScale = new Vector2(transform.localScale.x - scalePerFrameDifferenceFactor * Time.deltaTime,
                                               transform.localScale.y - scalePerFrameDifferenceFactor * Time.deltaTime);

            UpdatePlayerBounds();
            HandleMoveSpeed();

            // lose if size is too small
            if (transform.localScale.x < scaleToLose) Defeat();
        }
    }

    private void HandleMoveSpeed()
    {
        // if taken pickup that changes speed add its influence
        if (changeSpeedTaken) movementSpeed = movementSpeedPickupInfluence;

        // speed-size relation
        else movementSpeed = (Mathf.Exp(2.5f - transform.localScale.x) + 1);
    }

    private void Defeat()
    {
        defeated = true;

        // disable player collider and image
        GetComponent<SpriteRenderer>().enabled = false;
        playerCollider.enabled = false;

        StartCoroutine(PlaySoundThenDefeat());
    }

    private IEnumerator PlaySoundThenDefeat()
    {
        // play sound then wait 1 sec before loading game over scene
        AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, PlayerPrefs.GetFloat("VolumeOnOff", 0.5f));
        yield return new WaitForSeconds(1f);

        // reset game session (score)
        gameSession.ResetGameSession();
        sceneLoader.LoadGameOverScene();

        Destroy(gameObject);
    }

    public void TeleportAfterPortal()
    {
        transform.position = new Vector2(Random.Range(screenBounds.x - transform.localScale.x, -screenBounds.x + transform.localScale.x),
                                         Random.Range(screenBounds.y - transform.localScale.y, -screenBounds.y + transform.localScale.y));
    }

    public void AcceleratePlayerShrinking(float value)
    {
        scalePerFrameDifferenceFactor += value;
    }

    public void HandleSpeedPickup(bool effectState, float value)
    {
        changeSpeedTaken = effectState;
        movementSpeedPickupInfluence = value;
    }

    private void SetTrailColor()
    {
        Color trailColor = trailRenderer.endColor;

        switch(PlayerPrefs.GetInt("TrailColor", 0))
        {
            case 1:
                trailColor = new Color(0.17f, 0.27f, 0.31f);
                break;
            case 2:
                trailColor = new Color(0.96f, 0.85f, 0.577f);
                break;
            case 3:
                trailColor = new Color(0.23f, 0.44f, 0.56f);
                break;
            case 4:
                trailColor = new Color(0.91f, 0.7f, 0.435f);
                break;
            case 5:
                trailColor = new Color(0.93f, 0.9f, 0.8f);
                break;
            case 6:
                trailColor = new Color(0.31f, 0.365f, 0.26f);
                break;
            case 7:
                trailColor = Color.black;
                break;
            default:
                trailColor = new Color(0.92f, 0.9f, 0.8f);
                break;
        }
    }

    private void OnMouseDrag()
    {
        // convert mouse position from screen space to world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // clamp mouse inside game screen
        mousePosition.x = Mathf.Clamp(mousePosition.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
        mousePosition.y = Mathf.Clamp(mousePosition.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);

        RotateToMousePosition(mousePosition);

        // move player to the mouse position
        Vector3 normalizedPosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, movementSpeed * Time.deltaTime);

        if (tutorialMode) FindObjectOfType<TutorialManager>().playerHasMoved = true;
    }

    private void RotateToMousePosition(Vector2 mousePos)
    {
        float distanceBetweenMouseAndPlayer = Mathf.Sqrt(Mathf.Pow(mousePos.x - transform.position.x, 2)
                                                       + Mathf.Pow(mousePos.y - transform.position.y, 2));

        // rotate only if mouse is not overlapping the player
        if (distanceBetweenMouseAndPlayer > playerWidth && distanceBetweenMouseAndPlayer > playerHeight)
        {
            // find the direction vector
            Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

            // "look" at the mouse
            transform.up = direction;
        }
    }
}