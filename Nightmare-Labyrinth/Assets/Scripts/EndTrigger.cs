using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    private MazeRenderer mazeRenderer;
    private MazeGenerator mazeGenerator;

    private void Start()
    {
        mazeRenderer = FindObjectOfType<MazeRenderer>();
        mazeGenerator = FindObjectOfType<MazeGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0; // Pause the time
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        if (mazeRenderer != null && mazeGenerator != null)
        {
            DeletePlayerAndEnemies();
            mazeRenderer.RegenerateMaze();
            Time.timeScale = 1; // Resume normal time

        }
        else
        {
            Debug.LogError("MazeRenderer or MazeGenerator not found!");
        }
    }

    private void DeletePlayerAndEnemies()
    {
        // Delete player
        GameObject player = GameObject.Find("PlayerObj");
        if (player != null)
        {
            Destroy(player);
        }

        // Delete all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}