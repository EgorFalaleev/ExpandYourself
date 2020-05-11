using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // configuration parameters
    [SerializeField] protected float scaleToDestroy = 0.01f;
    [SerializeField] protected float scalePerFrameDifferenceFactor = 0.001f;
    [SerializeField] private int pointsPerPickup = 1;
    [SerializeField] private float valueToDecreaseIfNotCollected = 0.2f;
    [SerializeField] protected GameObject collectedVFX;

    // cached references
    protected Player player;
    protected GameSession gameSession;

    // state variables
    protected Vector2 circleScale;

    void Start()
    {
        // get the player object
        player = FindObjectOfType<Player>();
        gameSession = FindObjectOfType<GameSession>();

        // set pickup scale
        transform.localScale = FindObjectOfType<PickupGenerator>().GetPickupScale();
        circleScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    private void Update()
    {
        Shrink();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // create particles
        GameObject normalPickupParticles = Instantiate(collectedVFX, transform.position, transform.rotation);
        Destroy(normalPickupParticles, 1f);

        gameSession.AddToScore(pointsPerPickup);
        player.IncreaseSize(circleScale.x);
        Destroy(gameObject);
    }

    protected virtual void Shrink()
    {
        transform.localScale = new Vector2(transform.localScale.x - scalePerFrameDifferenceFactor * Time.deltaTime,
                                           transform.localScale.y - scalePerFrameDifferenceFactor * Time.deltaTime);
        circleScale = transform.localScale;

        // destroy pickup if it becomes too small
        if (transform.localScale.x < scaleToDestroy || transform.localScale.y < scaleToDestroy)
        {
            Destroy(gameObject);
            player.DecreaseSize(valueToDecreaseIfNotCollected);
            gameSession.ResetNumberOfPickupsCollected();
        }
    }

    public void AcceleratePickupShrinking(float value)
    {
        scalePerFrameDifferenceFactor += value;
    }
}
