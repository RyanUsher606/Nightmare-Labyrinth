using UnityEngine;

public class EnemyCollisionTrigger : MonoBehaviour
{
    // Reference to the NextLevelButton script
    private NextLevelButton nextLevelScript;

    private void Start()
    {
        // Find the NextLevelButton script in the scene
        nextLevelScript = FindObjectOfType<NextLevelButton>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the method to load the next level
            if (nextLevelScript != null)
            {
                nextLevelScript.LoadNextLevel();
            }
            else
            {
                Debug.LogError("NextLevelButton script not found!");
            }
        }
    }
}
