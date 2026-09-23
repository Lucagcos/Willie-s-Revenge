using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{

    public GameObject Projectile1; 
    int gameTurn = 0;
    public Vector2 squareSize;
    public Vector2 squareCenter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameLoop(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //keeps track of the game turn to decide what attack to do 
    void gameLoop()
    {
            switch (gameTurn)
            {
                case 0:
                StartCoroutine(AttackOne()); 
                    break; 

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                default: 
                    break;
            }

    }
  
    GameObject SpawnOnPerimeter()
    {
        Vector2 spawnPosition = GetRandomPerimeterPosition();
        return Instantiate(Projectile1, spawnPosition, Quaternion.identity);
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

        // Pick a random coordinate along the chosen side
        switch (side)
        {
            case 0: // Top edge
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

    IEnumerator AttackOne()
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(1);

            SpawnOnPerimeter();
        }

        gameTurn++;
    }

}
