using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;

    private List<GameObject> mazeCells = new List<GameObject>();
    public float CellSize = 1f;

    private void populateMaze()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze(); // Get our MazeGenerator Script.

        // Loops through every Cell in Maze.
        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                // Instantiate a new maze cell prefab
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);

                // Push object
                mazeCells.Add(newCell);

                // Get reference to the cell's MazeCellObject script.
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                // Determine which walls need to be active.
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                bool right = false;
                bool bottom = false;
                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if (y == 0) bottom = true;

                // Initialize the cell with the roof
                mazeCell.Init(top, bottom, right, left, true); // The last parameter sets the roof to active
            }
        }
    }

    private void Start()
    {
        populateMaze();
    }

    public void RegenerateMaze()
    {
        // Clearing off the old maze
        foreach (GameObject cell in mazeCells)
        {
            Destroy(cell);
        }

        populateMaze();
    }
}
