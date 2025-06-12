using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [Header("Target scene to load (e.g., CLUE0, CLUE1, CLUE2)")]
    [SerializeField] private string targetSceneName = "CLUE0";

    private static string previousSceneName;

    [ContextMenu("Go To Target Scene")]
    public void GoToTargetScene()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(targetSceneName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
    }
}