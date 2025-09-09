using UnityEngine;
using System.Collections.Generic;

public class Display : MonoBehaviour
{
    /*
    

    // Update is called once per frame
    void Update()
    {
        
    } 
    */

    public int rows = 8; // num of rows
    public int cols = 8; //num of cols
    public float cellSize = 1.0f; //size of each cell
    public List<Gem> gemPrefabs; //list<GameObject>
    public Gem gemPrefab1; // reference to the gem prefab 
    public Gem gemPrefab2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBoard(); //when the game starts it fills the board
    }

    void SpawnBoard()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vector3 pos = new Vector3(c * cellSize, r * cellSize, 0f); //spawn a new gem in the grid at position (x,y,z) x=c*cellSize,y=r*cellSize,z=0f cuz we in 2d
                int randomNumber = Random.Range(1, 3);
                if (randomNumber == 1)
                {
                    Instantiate(gemPrefab1, pos, Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(gemPrefab2, pos, Quaternion.identity, transform);
                }
                
            }
        }

    }

}
