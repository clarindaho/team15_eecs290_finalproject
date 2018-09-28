using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    //
    // instance fields
    //

    private AudioSource audioSource;

    private void GetMusicTime()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        Settings.PauseTime = audioSource.time;
    }

	void ToInstructions() {
        Debug.Log("Start Scene Controller: Instructions");

        GetMusicTime();
        SceneManager.LoadScene ("Instructions");
	}

    void ToSettings()
    {
        Debug.Log("Start Scene Controller: Settings");

        GetMusicTime();
        SceneManager.LoadScene("Settings");
    }

    void Exit()
    {
        Debug.Log("Start Scene Controller: Exit");

        // quit application
        Application.Quit();                                 // in standalone application
        //UnityEditor.EditorApplication.isPlaying = false;    // in Unity Editor
    }

    void ToStart()
    {
        Debug.Log("Start Scene Controller: Start");

        GetMusicTime();
        SceneManager.LoadScene("Start");
    }

    void ToRequiredInstructions()
    {
        if (!Settings.ViewedInstructions)
        {
            Debug.Log("Start Scene Controller: Required Instructions");
            Settings.ViewedInstructions = true;

            GetMusicTime();
            SceneManager.LoadScene("RequiredInstructions");
        }
        else
            ToLevel1();
    }

    void ToLevel1() {
        Debug.Log("Start Scene Controller: Level 1");

        GetMusicTime();
        SceneManager.LoadScene ("EasyLevel");
	}

    void FromEasy()
    {
        Settings.ExitButtonTarget = "EasyLevel";
        ToSettings();
    }

    void FromMedium()
    {
        Settings.ExitButtonTarget = "MediumLevel";
        ToSettings();
    }

    void FromHard()
    {
        Settings.ExitButtonTarget = "HardLevel";
        ToSettings();
    }

    void ExitSettings()
    {
        // Debug.Log(string.Format("Start Scene Controller: {}", Settings.ExitButtonTarget));
        string tempTarget = Settings.ExitButtonTarget;
        Settings.ExitButtonTarget = "Start";

        GetMusicTime();
        SceneManager.LoadScene(tempTarget);
    }
}

