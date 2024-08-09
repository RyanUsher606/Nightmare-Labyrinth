using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class EnemyContact : MonoBehaviour
{
    private MazeRenderer mazeRenderer;
    private MazeGenerator mazeGenerator;

    private RawImage deathImage;

    private void Start()
    {
        mazeRenderer = FindObjectOfType<MazeRenderer>();
        mazeGenerator = FindObjectOfType<MazeGenerator>();

        deathImage = GameObject.FindWithTag("DeathImage").GetComponent<RawImage>();
    }

    IEnumerator deathRoutine()
    {

        deathImage.enabled = true;
        yield return new WaitForSeconds(5);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            StartCoroutine(deathRoutine());

            // Setting the target alpha value

            Time.timeScale = 0; // Pause the time
            LoadNextLevel();


            // Now, remove the image
            // curColor.a = 0.0f;
            // deathImage.color = curColor;
            // deathImage.enabled = false;
            deathImage.enabled = false;
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