using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    // Fetching the required objects to load the next level
    private MazeRenderer mazeRenderer;
    private MazeGenerator mazeGenerator;

    public Button NextLevel;
    public Button QuitGame;

    public void Start()
    {
        // Fetching the objects when initialized
        mazeRenderer = FindObjectOfType<MazeRenderer>();
        mazeGenerator = FindObjectOfType<MazeGenerator>();

        NextLevel.gameObject.SetActive(false);
        NextLevel.interactable = true;

        NextLevel.onClick.AddListener(LoadNextLevel);
    }

    public void LoadNextLevel()
    {
        print("I got called!!");
        print(mazeRenderer);
        print(mazeGenerator);

        if (mazeRenderer != null && mazeGenerator != null)
        {
            print("Got here!!");
            // Get rid of components and then regenerate
            DeletePlayerAndEnemies();
            mazeRenderer.RegenerateMaze();

            // Make the buttons go away
            NextLevel.gameObject.SetActive(false);
            QuitGame.gameObject.SetActive(false);

            // Resuming normal time now
            Time.timeScale = 1;

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