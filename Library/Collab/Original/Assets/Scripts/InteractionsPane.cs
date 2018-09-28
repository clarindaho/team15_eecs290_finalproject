using UnityEngine;
using System.Collections;
using System;

public class InteractionsPane : MonoBehaviour
{
    //
    // inner classes
    //

    public class RotationAction
    {
        //
        // instance fields
        //
        private bool cwAction;
        private GameObject action;

        //
        // getter and setter methods
        //

        public bool CwAction
        {
            get { return cwAction; }
            private set { cwAction = value; }
        }

        public GameObject Action
        {
            get { return action; }
            private set { action = value; }
        }

        //
        // constructor methods
        //

        public RotationAction(bool cwAction, GameObject action)
        {
            CwAction = cwAction;
            Action = action;
        }
    }

    //
    // instance fields
    // 

    // rotate action UI buttons
    [SerializeField] private GameObject cwTestButtonTargetObject;
    [SerializeField] private GameObject ccwTestButtonTargetObject;
    [SerializeField] private string rotateButtonEnableMethod = "Enable";
    [SerializeField] private string rotateButtonDisableMethod = "Disable";

    // rotate action sprite prefabs
    [SerializeField] private GameObject ccwSpritePrefab;
    [SerializeField] private GameObject cwSpritePrefab;

    // original sprite position
    [SerializeField] private float xStartPos;
    [SerializeField] private float yStartPos;
    [SerializeField] private float zStartPos;

    // sprite offset
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    // calling the scene controller script
    [SerializeField] private GameObject sceneControllerTargetObject;
    [SerializeField] private string sceneControllerRotateRightMethod = "RotateRight";
    [SerializeField] private string sceneControllerRotateLeftMethod = "RotateLeft";
    [SerializeField] private string sceneControllerResetMethod = "Reset";

    // boolean to control response to buttons if code is running
    private bool isRunning = false;

    // time delays between each actions
    [SerializeField] private float actionStepDelay = 5f;
    [SerializeField] private float isRunningDelay = 8f;

    // moves in the interaction pane
    [SerializeField] private int movesAvailable;
    private IList actionArray = new ArrayList();

    // moves remaining GUI bar
    private Rect movesRemainingBar;
    private GUIStyle movesRemainingBarStyle = new GUIStyle();
    private string movesRemainingBarText;
    private int movesRemainingBarSize = 30;
    [SerializeField] private float movesRemainingBarPosX;
    [SerializeField] private float movesRemainingBarPosY;

    //
    // setter and getter methods
    //

    public bool IsRunning {
        get { return isRunning; }
        private set { isRunning = value;}
    }

    //
    // mutator methods
    //

    /// <summary>
    /// Adds to the array of actions the action button that was pressed
    /// </summary>
    /// <param name="cwButtonClicked">true if cw button was pressed, false if ccw button was pressed</param>
    public void ButtonClick(bool cwButtonClicked)
    {
        // respond if the interaction pane is not running
        if (!isRunning)
        {
            if (cwButtonClicked)
                AddCwAction();
            else
                AddCcwAction();
        }
    }

    /// <summary>
    /// Runs the array of actions in order
    /// </summary>
    public void Run()
    {
        // respond if the interaction pane is not running
        if (!isRunning)
        {
            if (actionArray.Count == movesAvailable)
            {
                // disable buttons from responding
                isRunning = true;
				ccwTestButtonTargetObject.SendMessage(rotateButtonDisableMethod);
				cwTestButtonTargetObject.SendMessage(rotateButtonDisableMethod);

                Debug.Log("Executing action order");

                int i = 0;
                foreach (RotationAction action in actionArray)
                {
                    float seconds = actionStepDelay * i++;

                    bool isRunning = true;
                    if (i == actionArray.Count) isRunning = false;

                    StartCoroutine(RunStep(action, seconds, isRunning));
                }
            }
            else
                Debug.Log("Not enough moves");
        }
    }

    /// <summary>
    /// Clear the array of actions
    /// </summary>
    public void Clear()
    {
        // respond if the interaction pane is not running
        if (!isRunning)
        {
            // destroy the current sprites in the action array
            foreach (RotationAction action in actionArray)
            {
                action.Action.SetActive(false);
                Destroy(action.Action);
            }

            // clear the action array
            actionArray.Clear();

            // reset the scene
            if (sceneControllerTargetObject != null)
                sceneControllerTargetObject.SendMessage(sceneControllerResetMethod);
        }
    }

    public void Start()
    {
        // format the text on the moves remaining GUI label
        movesRemainingBarStyle.fontSize = 14;
        movesRemainingBarStyle.fontStyle = FontStyle.Bold;
        movesRemainingBarStyle.normal.textColor = Color.red;
        movesRemainingBarStyle.alignment = TextAnchor.MiddleCenter;

        // create the rectangle frame of the moves remaining GUI label
        Vector3 position = transform.position;
        position.x -= movesRemainingBarPosX;
        position.y -= movesRemainingBarPosY;
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(position);
        movesRemainingBar = new Rect(cameraPosition.x, cameraPosition.y, movesRemainingBarSize, movesRemainingBarStyle.lineHeight);
    }

    public void Update()
    {
    }

    public void OnGUI()
    {
        CreateMovesRemainingBar();
    }

    //
    // helper methods
    //

    private IEnumerator RunStep(RotationAction action, float seconds, bool isRunning)
    {
        // wait number of seconds
        yield return new WaitForSeconds(seconds);

        // call the corresponding rotate action in the scene controller
        if (action.CwAction)
        {
            sceneControllerTargetObject.SendMessage(sceneControllerRotateRightMethod);
            Debug.Log("Rotating right");
        }
        else
        {
            sceneControllerTargetObject.SendMessage(sceneControllerRotateLeftMethod);
            Debug.Log("Rotating left");
        }

        // change running state
        StartCoroutine(ChangeRunningState(isRunning));
    }

    private IEnumerator ChangeRunningState(bool isRunning)
    {
        // wait number of seconds
        yield return new WaitForSeconds(isRunningDelay);

        // change running state
        this.isRunning = isRunning;
        if (!isRunning)
        {
			ccwTestButtonTargetObject.SendMessage(rotateButtonEnableMethod);
			cwTestButtonTargetObject.SendMessage(rotateButtonEnableMethod);
        }
    }

    private void CreateMovesRemainingBar()
    {
        movesRemainingBarText = string.Format("Moves Remaining: {0}", Math.Max((movesAvailable - actionArray.Count), 0));
        GUI.Label(movesRemainingBar, movesRemainingBarText, movesRemainingBarStyle);
    }

    private void AddCwAction()
    {
        if (actionArray.Count < movesAvailable)
        {
            // instantiate new cw sprite
            GameObject cwAction = Instantiate(cwSpritePrefab) as GameObject;

            // set the position
            float posX = xStartPos;
            if (actionArray.Count >= 5) posX = xStartPos + xOffset;

            float posY = yStartPos - (actionArray.Count * yOffset);
            if (actionArray.Count >= 5) posY = yStartPos - ((actionArray.Count - 5) * yOffset);

            float posZ = zStartPos;
            cwAction.transform.localPosition = new Vector3(posX, posY, posZ);

            // set the sprite active
            cwAction.SetActive(true);

            // add to action array
            actionArray.Add(new RotationAction(true, cwAction));
        }
    }

    private void AddCcwAction()
    {
        if (actionArray.Count < movesAvailable)
        {
            // instantiate new ccw sprite
            GameObject ccwAction = Instantiate(ccwSpritePrefab) as GameObject;

            // set the position
            float posX = xStartPos;
            if (actionArray.Count >= 5) posX = xStartPos + xOffset;

            float posY = yStartPos - (actionArray.Count * yOffset);
            if (actionArray.Count >= 5) posY = yStartPos - ((actionArray.Count - 5) * yOffset);

            float posZ = zStartPos;
            ccwAction.transform.localPosition = new Vector3(posX, posY, posZ);

            // set the sprite active
            ccwAction.SetActive(true);

            // add to action array
            actionArray.Add(new RotationAction(false, ccwAction));
        }
    }
}