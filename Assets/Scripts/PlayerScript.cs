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
    public int health;
    public int healthMax;
    public bool flash;

    Rigidbody2D _rbody;

    Vector2 moveDirection = Vector2.zero;

    public ShieldScript shield;

    SpriteRenderer sprite;

    public TMP_Text healthUI;

    public GameManagerScript manager;

    AudioSource _audioSource;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        healthMax = health;
        _rbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        //have shield off when it is not in use
        if (moveMode)
        {
            _rbody.linearVelocity = moveDirection * speed;
            shield.gameObject.SetActive(false);
        }

        //turn on the shield mode and move the player to the center 
        else
        {
            _rbody.linearVelocity = Vector2.zero;

            Vector2 target = new Vector2(0, -1.5f);

            transform.position = Vector2.MoveTowards( transform.position, target, speed * Time.deltaTime);

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

    //have effects when player takes dmg and update health 
    public IEnumerator FlashPlayer()
    {
        UpdateHealthText();
        flash = true;
        _audioSource.Play();
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
        if (health > 0)
            healthUI.text = "Health: " + health.ToString() + "/" + healthMax.ToString();
        else
            healthUI.color = new Color(1, 1, 1, 0); 
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
