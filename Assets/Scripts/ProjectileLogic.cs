using UnityEngine;
using System.Collections;


public class ProjectileLogic : MonoBehaviour
{
    private GameObject player;
    private Vector2 playerPosition;
    private PlayerScript playerScript;
    private Rigidbody2D _rbody;
    private GameManagerScript manager;
    public float speed;
    public float rainSpeed;
   
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManagerScript>();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>(); 
        _rbody = GetComponent<Rigidbody2D>();

        LaunchHandler(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LaunchHandler()
    {
        if (manager.rain)
            LaunchDown();
        else
            LaunchAtPlayer(); 
    }

    void LaunchAtPlayer()
    {

        playerPosition = player.transform.position;

        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;

        _rbody.linearVelocity = direction * speed;

        // Rotate triangle so its point faces its travel direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

    }

    void LaunchDown()
    {
        Vector2 direction = Vector2.down;

        _rbody.linearVelocity = direction * rainSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerScript.StartCoroutine(playerScript.FlashPlayer());
            Destroy(gameObject);
        }

        if (collision.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

   
}
