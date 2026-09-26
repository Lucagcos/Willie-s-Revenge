using UnityEngine;
using System.Collections;
using TMPro;

public class GameManagerScript : MonoBehaviour
{

    public GameObject Projectile1;
    public GameObject Projectile2;

    public PlayerScript player;
    public int gameTurn;
    public Vector2 squareSize;
    public Vector2 squareCenter;

    public TMP_Text fightUI; 

    public bool rain; 
    bool gameGoing = true;

    public GameObject willie;

    SpriteRenderer willieSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        willieSprite = willie.GetComponent<SpriteRenderer>();
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //keeps track of the game turn to decide what attack to do 
    IEnumerator GameLoop()
    {
        while (gameGoing)
        {
            switch (gameTurn)
            {
                case 0:
                    //yield return StartCoroutine(PlayerAttack());
                    break;

                case 1:
                    yield return StartCoroutine(AttackOne());
                    break;

                case 2:
                    yield return StartCoroutine(AttackTwo());
                    break;

                case 3:
                    yield return StartCoroutine(AttackThree());
                    break;
                case 4:
                    yield return StartCoroutine(AttackFour());
                    break;

                case 5:
                    yield return StartCoroutine(AttackFive());
                    break;

                case 6:
                    player.moveMode = false;
                    yield return StartCoroutine(AttackSix());
                    break;

                default:
                    gameGoing = false;
                    break;
            }

            yield return StartCoroutine(PlayerTurn());
        }
        
    }
  
    GameObject SpawnOnPerimeter(GameObject projectile, bool rain)
    {
        Vector2 spawnPosition = GetRandomPerimeterPosition(rain);
        return Instantiate(projectile, spawnPosition, Quaternion.identity);
    }

    GameObject SpawnForShield(GameObject projectile, bool eight)
    {
        Vector2 spawnPosition = GetRandomOctalPosition(false);
        return Instantiate(projectile, spawnPosition, Quaternion.identity);
    }

    Vector2 GetRandomPerimeterPosition(bool rain)
    {
        // Calculate the boundaries based on center and size
        float halfWidth = squareSize.x / 2f;
        float halfHeight = squareSize.y / 2f;

        float left = squareCenter.x - halfWidth;
        float right = squareCenter.x + halfWidth;
        float bottom = squareCenter.y - halfHeight;
        float top = squareCenter.y + halfHeight;

        // Pick a random side: 0 = Top, 1 = Bottom, 2 = Left, 3 = Right
        int side = Random.Range(0, 4);
        Vector2 randomPoint = Vector2.zero;

        if (rain)
            side = 0; 

        // Pick a random coordinate along the chosen side
        switch (side)
        {
            case 0: // Top edge
                if (rain)
                {
                    randomPoint = new Vector2(Random.Range(-2.5f, 2f), top);
                }
                else 
                    randomPoint = new Vector2(Random.Range(left, right), top);
                break;
            case 1: // Bottom edge
                randomPoint = new Vector2(Random.Range(left, right), bottom);
                break;
            case 2: // Left edge
                randomPoint = new Vector2(left, Random.Range(bottom, top));
                break;
            case 3: // Right edge
                randomPoint = new Vector2(right, Random.Range(bottom, top));
                break;
        }

        return randomPoint;
    }

    Vector2 GetRandomOctalPosition(bool eight)
    {
        Vector2 randomPoint = Vector2.zero;
        int side = Random.Range(0, 4);

        if (eight)
            side = Random.Range(0, 8);

        switch (side)
        {
            case 0: // Top 
                randomPoint = new Vector2(0f,3.5f);
                break;
            case 1: // Bottom 
                randomPoint = new Vector2(0f, -5.5f);
                break;
            case 2: // Left 
                randomPoint = new Vector2(5f, -1.5f);

                break;
            case 3: // Right 
                randomPoint = new Vector2(5f, -1.5f);

                break;
            case 4: 

                break;
            case 5: 

                break;
            case 6:

                break;
            case 7: 

                break;
        }

        return randomPoint;
    }

    IEnumerator AttackOne()
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(1);

            rain = false;
            SpawnOnPerimeter(Projectile1, rain );
        }
    }

    IEnumerator AttackTwo()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(.3f);
            rain = true; 
            SpawnOnPerimeter(Projectile1, rain);
        }
    }

    IEnumerator AttackThree()
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(1);
            rain = false;
            SpawnOnPerimeter(Projectile1, rain );
        }
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(.3f);
            rain = true;
            SpawnOnPerimeter(Projectile1, rain);
        }

    }

    IEnumerator AttackFour()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(0.5f);
            rain = false;
            SpawnOnPerimeter(Projectile1, rain);
            yield return new WaitForSeconds(0.2f);
            rain = true;
            SpawnOnPerimeter(Projectile1, rain);
        }

    }

    IEnumerator AttackFive()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.7f);
            rain = false;
            SpawnOnPerimeter(Projectile1, rain); 
            SpawnOnPerimeter(Projectile1, rain);
            yield return new WaitForSeconds(0.7f);
            rain = true;
            SpawnOnPerimeter(Projectile1, rain);
            SpawnOnPerimeter(Projectile1, rain);
        }

    }

    IEnumerator AttackSix()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);
            rain = false;
            SpawnForShield(Projectile2,false);
         
        }

    }

    IEnumerator PlayerTurn()
    {
        while (!player.held)
        { 
            willieSprite.color = new Color(1f, 1f, 1f, 1f);   
            fightUI.color = Color.white;
            yield return null;
        }

        fightUI.color = new Color(1f, 1f, 1f, 0f);
        willieSprite.color = new Color(1f, 1f, 1f, .1f);
        gameTurn++;

        Debug.Log(player.held);
    }

}
