using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerScript : MonoBehaviour
{

    public float speed;

    public bool held = false;

    public bool moveMode = true;

    Rigidbody2D _rbody;

    Vector2 moveDirection = Vector2.zero;

    public ShieldScript shield;

    SpriteRenderer sprite;

    public int health;

    public int healthMax;

    public TMP_Text healthUI;

    public bool flash;

    public GameManagerScript manager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthMax = health;
        _rbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveMode)
        {
            _rbody.linearVelocity = moveDirection * speed;
            shield.gameObject.SetActive(false);
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

    public IEnumerator FlashPlayer()
    {
        UpdateHealthText();

        flash = true;

        for (int i = 0; i < 4; i++)
        {
            sprite.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(0.1f);

            sprite.color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(0.1f);
        }

        flash = false;

    }

    public void UpdateHealthText()
    { 
        healthUI.text = "Health: " + health.ToString() + "/" + healthMax.ToString();
    }

    public void AddHealth()
    {
        if (health + 1 <= healthMax)
        {
            health++;
        }
        UpdateHealthText();
    }

}
