using UnityEngine;

public class AudioController : MonoBehaviour {
    //
    // instance fields
    //

    // music clips
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip runningMusic;

    private AudioSource audioSource;

    //
    // setter and getter methods
    //

    //
    // mutator methods
    //

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        // stop any audio
        audioSource.Stop();

        // play background music
        audioSource.clip = backgroundMusic;
        audioSource.time = Settings.PauseTime;
        audioSource.Play();
    }

    public void Update()
    {
        if (audioSource != null)
            audioSource.volume = Settings.Volume;
    }

    public void Run()
    {
        // stop background music
        Settings.PauseTime = 0f;
        audioSource.Stop();

        // play running music
        audioSource.clip = runningMusic;
        audioSource.time = 0f;
        audioSource.Play();
    }

    public void Clear()
    {
        if (audioSource.clip == runningMusic) {
            // stop running music
            audioSource.Stop();

            // resume playing background music
            audioSource.clip = backgroundMusic;
            audioSource.time = Settings.PauseTime;
            audioSource.Play();
        }
    }
}
