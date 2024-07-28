using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)]
    public int mazeWidth = 5, mazeHeight = 5; // Dimensions of the maze
    public int startX, startY; // Position where maze generation will start.
    public MazeCell[,] maze; // This is where array of maze cells will be stored.

    private Vector2Int currentCell; // Maze cell we are currently at.

    public GameObject examplePlayer;
    public GameObject enemyPrefab; // Enemy prefab to instantiate
    public NavMeshSurface navMeshSurface; // NavMeshSurface for baking navigation mesh

    private GameObject instantiatedPlayer; // Reference to the instantiated player

    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }

        CarvePath(startX, startY); // Carve out the maze path starting from the initial position
        InstantiatePlayer(); // Instantiate the player at a random position
        StartCoroutine(BakeNavMeshAndInstantiateEnemies()); // Bake the NavMesh and instantiate enemies after baking

        return maze;
    }

    private void InstantiatePlayer()
    {
        Vector2Int randomPosition = GetRandomPosition();
        Vector3 playerPosition = new Vector3(randomPosition.x, 0, randomPosition.y);
        instantiatedPlayer = Instantiate(examplePlayer, playerPosition, Quaternion.identity);
        instantiatedPlayer.name = "Player"; // Ensure the name matches the one in EnemyAi script
    }

    private IEnumerator BakeNavMeshAndInstantiateEnemies()
    {
        // Wait for the end of the frame to ensure all objects are positioned correctly
        yield return new WaitForEndOfFrame();

        // Bake the NavMesh
        navMeshSurface.BuildNavMesh();

        // Wait for the NavMesh to be fully built
        yield return new WaitUntil(() => navMeshSurface.navMeshData != null);

        // Instantiate the enemies
        InstantiateEnemies();
    }

    private void InstantiateEnemies()
    {
        // Try a few times to find a valid position on the NavMesh
        for (int i = 0; i < 10; i++)
        {
            Vector2Int randomPosition = GetRandomPosition();
            Vector3 enemyPosition = new Vector3(randomPosition.x, 0, randomPosition.y);

            // Debug the positions being checked
            Debug.DrawLine(enemyPosition + Vector3.up * 2, enemyPosition, Color.red, 10.0f);

            // Check if the position is on the NavMesh
            if (UnityEngine.AI.NavMesh.SamplePosition(enemyPosition, out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);

                // Assign the player object to the enemy's script
                EnemyAi enemyAI = enemy.GetComponent<EnemyAi>();
                if (enemyAI != null)
                {
                    enemyAI.player = instantiatedPlayer.transform;
                }
                return;
            }
        }

        Debug.LogError("Failed to find a valid position on the NavMesh for enemy instantiation");
    }

    private Vector2Int GetRandomPosition()
    {
        int randomX = Random.Range(0, mazeWidth);
        int randomY = Random.Range(0, mazeHeight);
        return new Vector2Int(randomX, randomY);
    }

    private List<Direction> directions = new List<Direction> {
        Direction.Up, Direction.Down, Direction.Left, Direction.Right,
    };

    private List<Direction> GetRandomDirections()
    {
        List<Direction> dir = new List<Direction>(directions); // Copy of List above because we don't want to edit above list.
        List<Direction> rndDir = new List<Direction>(); // Randomized directions of maze list.

        // Loop until rndDir is empty
        while (dir.Count > 0)
        {
            int rnd = Random.Range(0, dir.Count);
            rndDir.Add(dir[rnd]);
            dir.RemoveAt(rnd);
        }
        return rndDir;
    }

    private bool IsCellValid(int x, int y)
    {
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited) return false; // If cell outside of map or visited, it is not valid.
        else return true;
    }

    private Vector2Int CheckNeighbours()
    {
        List<Direction> rndDir = GetRandomDirections();

        for (int i = 0; i < rndDir.Count; i++)
        {
            Vector2Int neighbour = currentCell; // Set neighbour to current cell

            // Modify neighbour based on random directions we're currently trying
            switch (rndDir[i])
            {
                case Direction.Up:
                    neighbour.y++;
                    break;
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break;
                case Direction.Left:
                    neighbour.x--;
                    break;
            }

            if (IsCellValid(neighbour.x, neighbour.y)) return neighbour; // Checks to see if neighbour we tried is valid if not we rerun again.
        }

        return currentCell; // If no valid neighbour was found, we return currentCell
    }

    private void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        // We can go in one direction at a time.
        if (primaryCell.x > secondaryCell.x) // Primary Cells left wall
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        }
        else if (primaryCell.x < secondaryCell.x) // Secondary Cell Left Wall
        {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        }
        else if (primaryCell.y < secondaryCell.y) // Primary Cell Top Wall
        {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        }
        else if (primaryCell.y > secondaryCell.y) // Secondary Cell Top Wall
        {
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
    }

    private void CarvePath(int x, int y)
    {
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1) // Check to see if start position is inside map, if not default to 0
        {
            x = y = 0;
        }

        currentCell = new Vector2Int(x, y); // Set current cell position to the starting points we passed in the function.

        List<Vector2Int> path = new List<Vector2Int>(); // Keeps track of current path.

        // Loop occurs until dead end is encountered.
        bool deadEnd = false;
        while (!deadEnd)
        {
            Vector2Int nextCell = CheckNeighbours(); // Gets next cell.

            // If no valid neighbours set deadend to true.
            if (nextCell == currentCell)
            {
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i]; // Set current cell to next step back.
                    path.RemoveAt(i); // Removes step from path.
                    nextCell = CheckNeighbours(); // Checks to see if any neighbours valid

                    if (nextCell != currentCell) break; // If valid neighbour is found, it breaks out of loop
                }

                if (nextCell == currentCell)
                {
                    deadEnd = true;
                }
            }
            else
            {
                BreakWalls(currentCell, nextCell); // Set wall flags on these two cells
                maze[currentCell.x, currentCell.y].visited = true; // Set cells to visited
                currentCell = nextCell; // Current cell is then changed to neighbour we are now moving to
                path.Add(currentCell); // Cell is added to path
            }
        }
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public class MazeCell
{
    public bool visited;
    public int x, y;

    public bool topWall;
    public bool leftWall;

    public Vector2Int position
    {
        get
        {
            return new Vector2Int(x, y);
        }
    }

    public MazeCell(int x, int y)
    {
        // Coordinates of the cell 
        this.x = x;
        this.y = y;

        // Check if algorithm has visited cell.
        visited = false;

        // All walls of cell will need to be present until removed by algorithm.
        topWall = leftWall = true;
    }
}
