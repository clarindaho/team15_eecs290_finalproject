using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    void Exit()
    {
        Debug.Log("Start Scene Controller: Exit");

        // quit application
        Application.Quit();                                 // in standalone application
        UnityEditor.EditorApplication.isPlaying = false;    // in Unity Editor
    }

	void ToInstructions() {
        Debug.Log("Start Scene Controller: Instructions");

        SceneManager.LoadScene ("Instructions");
	}

    void ToStart()
    {
        Debug.Log("Start Scene Controller: Start");

        SceneManager.LoadScene("Start");
    }

    void ToLevel1() {
        Debug.Log("Start Scene Controller: Level 1");

        SceneManager.LoadScene ("EasyLevel");
	}
}

