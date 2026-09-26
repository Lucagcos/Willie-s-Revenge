using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public float speed;

    public bool held = false;

    public bool moveMode = true;

    Rigidbody2D _rbody;

    Vector2 moveDirection = Vector2.zero;

    public ShieldScript shield;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveMode)
        {
            _rbody.linearVelocity = moveDirection * speed;
        }
        else
        {
            _rbody.linearVelocity = Vector2.zero;

            Vector2 target = new Vector2(0, -1.5f);

            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, target) < 0.01f)
            {
                shield.gameObject.SetActive(true);
            }
        }
    }

    public void OnMove(InputValue value)
    {
        if (moveMode)
            moveDirection = value.Get<Vector2>();
        else
            shield.RotateShield(value); 
    }


    public void OnAction(InputValue value)
    {
        held = value.isPressed;
        //Debug.Log("OnAction: " + held);
    }

}
