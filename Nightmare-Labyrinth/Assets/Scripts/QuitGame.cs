using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    public Button quitGame;

    public void Start()
    {
        quitGame.gameObject.SetActive(false);
        quitGame.onClick.AddListener(QuitGame);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
            // If we are running in the editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // If we are running in a build
        Application.Quit();
#endif
    }
}