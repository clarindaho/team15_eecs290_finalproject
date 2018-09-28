using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings {

    //
    // instance fields
    //

    // default slider values
    private static float rotationSpeed = 0.75f;
    private static float volume = 0.5f;

    // clip pause time
    private static float pauseTime = 0f;

    // required instructions
    private static bool viewedInstructions = false;

    // settings scene exit button target
    private static string exitButtonTarget = "Start";

    //
    // setter and getter methods
    //

    public static float RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }

    public static float Volume
    {
        get { return volume; }
        set { volume = value; }
    }

    public static float PauseTime
    {
        get { return pauseTime; }
        set { pauseTime = value; }
    }

    public static bool ViewedInstructions
    {
        get { return viewedInstructions; }
        set { viewedInstructions = true; }
    }

    public static string ExitButtonTarget
    {
        get { return exitButtonTarget; }
        set { exitButtonTarget = value; }
    }
}
