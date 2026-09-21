using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public float speed;

    Rigidbody2D _rbody;

    Vector2 moveDirection = Vector2.zero;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rbody.linearVelocity = moveDirection * speed;
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

}
