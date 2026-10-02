using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    public GameObject Projectile1;
    public GameObject Projectile2;

    public PlayerScript player;
    public GameObject willie;
    private SpriteRenderer willieSprite;

    public SpriteRenderer DialogueSprite;

    public Vector2 squareSize;
    public Vector2 squareCenter;

    public TMP_Text fightUI;

    public TMP_Text roundUI;

    public TMP_Text win;

    public TMP_Text dialogue;
    AudioSource _audioSource;
    public AudioClip shieldBlock;

    public int gameTurn;
    public bool rain; 
    bool gameGoing = true;
    bool gameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        win.color = new Color(1f, 1f, 1f, 0f);
        DialogueSprite.color = new Color(1f, 1f, 1f, 0f);
        dialogue.color = new Color(0f, 0f, 0f, 1f);
        willieSprite = willie.GetComponent<SpriteRenderer>();
        UpdateRound();
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health <= 0 && !gameOver)
        {
            gameOver = true;
            StopAllCoroutines();
            StartCoroutine(GameLoseSequence());
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GameExit();
        }
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
                    gameGoing = false; 
                    break;

                default:
                    break;
            }
            
            //player turn 
            yield return new WaitForSeconds(2f);
            player.AddHealth();
            yield return StartCoroutine(PlayerTurn());
        }

        //victoroy effects and return to main menu
        StartCoroutine(RotateWillie());
        yield return new WaitForSeconds(5);
        GameExit(); 
    }

    //set the spawn point for projectiles to an imaginary square perimeter around the playable area 
    GameObject SpawnOnPerimeter(GameObject projectile)
    {
        Vector2 spawnPosition = GetRandomPerimeterPosition();
        return Instantiate(projectile, spawnPosition, Quaternion.identity);
    }

    //spawn projectiles in a total of 8 possible spots
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
                    //ensure rain projectiles are inside the playable area 
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

    //helper method for shield projectiles. This is for making a list so we can have pseudo random spawning 
    List<int> CreateDirectionList(int startDirection, int endDirection, int repeats)
    {
        List<int> directions = new List<int>();

        // Add every direction once per repeat, plus one random direction
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
            yield return new WaitForSeconds(0.6f);
            rain = false;
            SpawnOnPerimeter(Projectile1);
            yield return new WaitForSeconds(0.3f);
            rain = true;
            SpawnOnPerimeter(Projectile1);
        }

    }

    IEnumerator AttackFive()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.75f);
            rain = false;
            SpawnOnPerimeter(Projectile1); 
            SpawnOnPerimeter(Projectile1);
            yield return new WaitForSeconds(0.75f);
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
        DialogueSprite.color = new Color(1f, 1f, 1f, 1f);
        dialogue.text = "RAHHHHHH!";

        yield return new WaitForSeconds(2); 

        // Finish with previous attacks
        yield return StartCoroutine(AttackTwo());
        yield return StartCoroutine(AttackFour());
        yield return StartCoroutine(AttackFive());
    }

    IEnumerator PlayerTurn()
    {
        //turn on and off certain ui elements 
        DialogueSprite.color = new Color(1f, 1f, 1f, 1f);
        dialogue.text = RandomDialogue();
        while (!player.held)
        {
            
            willieSprite.color = new Color(1f, 1f, 1f, 1f);   
            fightUI.color = Color.white;
            yield return null;
        }

        UpdateRound();
        fightUI.color = new Color(1f, 1f, 1f, 0f);
        willieSprite.color = new Color(1f, 1f, 1f, .1f);
        DialogueSprite.color = new Color(1f, 1f, 1f, 0f);
        gameTurn++;

    }

    void UpdateRound()
    {
        if (gameTurn >= 10)
        {
            roundUI.text = "Round: 10/10";
        }
        else
        {
            if (gameTurn != 0)
            {
                roundUI.text ="Round: " + (gameTurn + 1).ToString() + "/??";
            }
            else
            {
                roundUI.text = "Round: 1/??";
            }
        }
            
    }

    IEnumerator GameLoseSequence()
    {
        //clear the screen and reset variables
        ClearProjectiles();

        gameGoing = false;

        gameTurn = 0;
        rain = false;

        player.health = player.healthMax;
        player.UpdateHealthText();

        player.moveMode = true;
        player.held = false;

        willieSprite.color = new Color(1f, 1f, 1f, 1f);
        //fightUI.color = Color.white;

        UpdateRound();

        gameOver = false;
        gameGoing = true;

        
        roundUI.color = new Color(1, 1, 1, 0);
        DialogueSprite.color = new Color(1f, 1f, 1f, 1f);
        dialogue.color = new Color(0, 0, 0, 1);
        dialogue.text = "HAHA TOO BAD!";
        yield return new WaitForSeconds(3f);
        DialogueSprite.color = new Color(1f, 1f, 1f, 0f);

        //send back to main menu
        GameExit(); 
        //StartCoroutine(GameLoop());
    }

    void ClearProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }
    }

    //smoothly rotate willie sprite
    IEnumerator RotateWillie()
    {
        win.color = new Color(1f, 1f, 1f, 1f);
        willieSprite.color = new Color(1, 1, 1, 1); 
        Quaternion startRotation = willie.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, 90);

        float duration = 0.6f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            willie.transform.rotation = Quaternion.Lerp(startRotation , endRotation , elapsedTime / duration );

            yield return null;
        }

        willie.transform.rotation = endRotation;
    }

    public void GameExit()
    {
        SceneManager.LoadScene("GameStart");
    }

    public string RandomDialogue()
    {

        int option = Random.Range(0, 8);

        if (gameTurn == 0)
        {
            return "Let's go!\nArrow Keys to Move and Space \nto Continue";
        }
        if (gameTurn == 5)
        {
            return "Alright bet...\nArrow Keys to\nBlock a Direction.";
        }
        if (gameTurn == 7)
        {
            return "Hey real quick,\ndon't do two\nkeys at once.";
        }
        if (!gameGoing)
        {
            return "NOOO AHHH \n>:(";
        }

        switch (option)
        {
            case (0):
                return "GRRRR!";
            case (1):
                return "I'll show ya!";
            case (2):
                return "Now it's on. \nYou're going down.";
            case (3):
                return "Impressive...\nLet's see \nwhat you got!";
            case (4):
                return "Ok lil bro.";
            case (5):
                return "You're done for!";
            case (6):
                return "You can't \nhandle this!";
            case (7):
                return "Ja-rona!";
            default:
                return "Def";
        }
    }

    public void PlayBlockSound()
    {
        _audioSource.PlayOneShot(shieldBlock,20f);
    }
}
