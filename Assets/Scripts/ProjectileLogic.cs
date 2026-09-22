using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    GameObject player;
    Vector2 playerPosition;

    public float speed;

    private Rigidbody2D _rbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _rbody = GetComponent<Rigidbody2D>();
        Launch(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Launch()
    {
        playerPosition = player.transform.position;

        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;

        _rbody.linearVelocity = direction * speed;

        // Rotate triangle so its point faces its travel direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
