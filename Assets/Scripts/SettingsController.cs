using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    //
    // instance fields
    //

    // rotation speed
    [SerializeField] private Slider rotationSpeedSlider;

    // volume
    [SerializeField] private Slider volumeSlider;

    //
    // setter and getter methods
    //

    //
    // inherited methods
    //

    void Start()
    {
        rotationSpeedSlider.value = Settings.RotationSpeed;
        volumeSlider.value = Settings.Volume;
    }

    void Update()
    {
        Settings.RotationSpeed = rotationSpeedSlider.value;
        Settings.Volume = volumeSlider.value;
    }

    //
    // mutator methods
    //
}
