using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyContact : MonoBehaviour
{
    private MazeRenderer mazeRenderer;
    private MazeGenerator mazeGenerator;

    private RawImage deathImage;
    private AudioSource audioSource;
    public AudioClip deathSound; // Assign the death sound clip in the Inspector

    private void Start()
    {
        mazeRenderer = FindObjectOfType<MazeRenderer>();
        mazeGenerator = FindObjectOfType<MazeGenerator>();

        deathImage = GameObject.FindWithTag("DeathImage").GetComponent<RawImage>();

        // Add an AudioSource component if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the death sound
            if (deathSound != null && audioSource != null)
            {
                audioSource.clip = deathSound;
                audioSource.Play();
            }

            // Display the death image
            deathImage.enabled = true;

            yield return new WaitForSeconds(3.5f);

            Time.timeScale = 0; // Pause the time
            LoadNextLevel();

            // Remove the image after reloading the level
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
