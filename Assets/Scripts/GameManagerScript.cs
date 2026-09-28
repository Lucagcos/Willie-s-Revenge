using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
public class GameManagerScript : MonoBehaviour
{

    public GameObject Projectile1;
    public GameObject Projectile2;

    public PlayerScript player;
    public GameObject willie;
    private SpriteRenderer willieSprite;

    public int gameTurn;
    public Vector2 squareSize;
    public Vector2 squareCenter;

    public TMP_Text fightUI; 

    public bool rain; 
    bool gameGoing = true;

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

                case 7:
                    yield return StartCoroutine(AttackSeven());
                    break;

                case 8:
                    yield return StartCoroutine(AttackEight());
                    break;

                case 9:
                    yield return StartCoroutine(AttackNine());
                    break;

                case 10:
                    yield return StartCoroutine(AttackTen());
                    break;

                default:
                    gameGoing = false;
                    break;
            }

            yield return StartCoroutine(PlayerTurn());
        }
        
    }
  
    GameObject SpawnOnPerimeter(GameObject projectile)
    {
        Vector2 spawnPosition = GetRandomPerimeterPosition();
        return Instantiate(projectile, spawnPosition, Quaternion.identity);
    }

    GameObject SpawnForShield(GameObject projectile, int direction)
    {
        Vector2 spawnPosition = GetOctalPosition(direction);
        return Instantiate(projectile, spawnPosition, Quaternion.identity);
    }

    Vector2 GetRandomPerimeterPosition()
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

    Vector2 GetOctalPosition(int side)
    {
        Vector2 spawnPoint = Vector2.zero;

        switch (side)
        {
            case 0: // Top
                spawnPoint = new Vector2(0f, 3.5f);
                break;

            case 1: // Bottom
                spawnPoint = new Vector2(0f, -5.5f);
                break;

            case 2: // Left
                spawnPoint = new Vector2(-5f, -1.5f);
                break;

            case 3: // Right
                spawnPoint = new Vector2(5f, -1.5f);
                break;

            case 4: // top left
                spawnPoint = new Vector2(-5f, 3.5f);
                break;

            case 5: // top right 
                spawnPoint = new Vector2(5f, 3.5f);
                break;

            case 6: // bottom left 
                spawnPoint = new Vector2(-5f, -5.5f);
                break;

            case 7: // bottom right 
                spawnPoint = new Vector2(5f, -5.5f);
                break;
        }

        return spawnPoint;
    }

    List<int> CreateDirectionList(int startDirection, int endDirection, int repeats)
    {
        List<int> directions = new List<int>();

        // Add every direction once per repeat,
        // plus one random direction
        for (int i = 0; i < repeats; i++)
        {
            for (int direction = startDirection; direction <= endDirection; direction++)
            {
                directions.Add(direction);
            }

            directions.Add(Random.Range(startDirection, endDirection + 1));
        }

        // Shuffle the list
        for (int i = 0; i < directions.Count; i++)
        {
            int randomIndex = Random.Range(i, directions.Count);

            int temp = directions[i];
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }

        return directions;
    }

    IEnumerator AttackOne()
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(1);

            rain = false;
            SpawnOnPerimeter(Projectile1);
        }
    }

    IEnumerator AttackTwo()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(.3f);
            rain = true; 
            SpawnOnPerimeter(Projectile1);
        }
    }

    IEnumerator AttackThree()
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(1);
            rain = false;
            SpawnOnPerimeter(Projectile1 );
        }
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(.3f);
            rain = true;
            SpawnOnPerimeter(Projectile1);
        }

    }

    IEnumerator AttackFour()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(0.5f);
            rain = false;
            SpawnOnPerimeter(Projectile1);
            yield return new WaitForSeconds(0.2f);
            rain = true;
            SpawnOnPerimeter(Projectile1);
        }

    }

    IEnumerator AttackFive()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.7f);
            rain = false;
            SpawnOnPerimeter(Projectile1); 
            SpawnOnPerimeter(Projectile1);
            yield return new WaitForSeconds(0.7f);
            rain = true;
            SpawnOnPerimeter(Projectile1);
            SpawnOnPerimeter(Projectile1);
        }

    }


    IEnumerator AttackSix()
    {
        yield return new WaitForSeconds(2f);

        rain = false;

        // Directions 0-3, each guaranteed twice
        List<int> directions = CreateDirectionList(0, 3, 2);

        foreach (int direction in directions)
        {
            yield return new WaitForSeconds(0.7f);
            SpawnForShield(Projectile2, direction);
        }
    }


    IEnumerator AttackSeven()
    {
        rain = false;

        // Directions 0-3, each guaranteed three times
        List<int> directions = CreateDirectionList(0, 3, 4);

        foreach (int direction in directions)
        {
            float wait = Random.Range(.3f, .5f);

            yield return new WaitForSeconds(wait);
            SpawnForShield(Projectile2, direction);
        }
    }


    IEnumerator AttackEight()
    {
        rain = false;

        // Directions 4-7, each guaranteed three times
        List<int> directions = CreateDirectionList(4, 7, 3);

        foreach (int direction in directions)
        {
            yield return new WaitForSeconds(0.6f);
            SpawnForShield(Projectile2, direction);
        }
    }


    IEnumerator AttackNine()
    {
        rain = false;

        // Directions 0-7, each guaranteed twice
        List<int> directions = CreateDirectionList(0, 7, 3);

        foreach (int direction in directions)
        {
            float wait = Random.Range(.4f, .7f);

            yield return new WaitForSeconds(wait);
            SpawnForShield(Projectile2, direction);
        }
    }


    IEnumerator AttackTen()
    {
        rain = false;

        // Directions 0-7, each guaranteed three times
        List<int> directions = CreateDirectionList(0, 7, 3);

        foreach (int direction in directions)
        {
            float wait = Random.Range(.3f, .6f);

            yield return new WaitForSeconds(wait);
            SpawnForShield(Projectile2, direction);
        }

        yield return new WaitForSeconds(2);

        // Return to normal movement mode
        player.moveMode = true;

        // Finish with previous attacks
        yield return StartCoroutine(AttackTwo());
        yield return StartCoroutine(AttackFour());
        yield return StartCoroutine(AttackFive());
    }

    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(2f);

        while (!player.held)
        { 
            willieSprite.color = new Color(1f, 1f, 1f, 1f);   
            fightUI.color = Color.white;
            yield return null;
        }

        fightUI.color = new Color(1f, 1f, 1f, 0f);
        willieSprite.color = new Color(1f, 1f, 1f, .1f);
        gameTurn++;

    }

}
