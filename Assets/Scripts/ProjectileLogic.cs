using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    GameObject player;
    Vector2 playerPosition;

    private GameManagerScript manager;

    public float speed;
    public float rainSpeed;

    private Rigidbody2D _rbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManagerScript>();
        }

        player = GameObject.FindGameObjectWithTag("Player");
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

    void ShieldLaunch4D()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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
