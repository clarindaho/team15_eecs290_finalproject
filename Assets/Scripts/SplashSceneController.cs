using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashSceneController : MonoBehaviour {
    //
    // instance fields
    //

    [SerializeField] private float splashScreenTime = 5f;

    [SerializeField] private GameObject titleScreen;
    [SerializeField] private float titleScreenTime = 2f;

    [SerializeField] private GameObject plotScreen;

    private AudioSource audioSource;

    //
    // setter and getter methods
    //


    //
    // inherited methods
    //

    void Start () {
        ShowTitle();
        StartCoroutine(ShowPlot());
        StartCoroutine(DelayStart());
	}

    //
    // mutator methods
    //

    private void GetMusicTime()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        Settings.PauseTime = audioSource.time;
    }

    private void ShowTitle()
    {
        // enable title screen
        titleScreen.SetActive(true);

        // disable plot screen
        plotScreen.SetActive(false);
    }

    private IEnumerator ShowPlot()
    {
        yield return new WaitForSeconds(titleScreenTime);

        // disable title screen
        titleScreen.SetActive(false);

        // enable plot screen
        plotScreen.SetActive(true);
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(splashScreenTime);
        GetMusicTime();
        SceneManager.LoadScene("Start");
    }
}
