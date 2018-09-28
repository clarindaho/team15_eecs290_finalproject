using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

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

    // audio controller
    [SerializeField] private GameObject audioControllerTargetObject;
    [SerializeField] private string audioControllerRunMethod = "Run";
    [SerializeField] private string audioControllerClearMethod = "Clear";

    // rotate action UI buttons
    [SerializeField] private GameObject cwTestButtonTargetObject;
    [SerializeField] private GameObject ccwTestButtonTargetObject;
    [SerializeField] private string rotateButtonEnableMethod = "Enable";
    [SerializeField] private string rotateButtonDisableMethod = "Disable";

    private Color originalColor;
    public Color highlightColor = Color.red;

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
    [SerializeField] private string sceneControllerActivateTriggerMethod = "ActivateTrigger";
    [SerializeField] private string sceneControllerDeactivateTriggerMethod = "DeactivateTrigger";
    [SerializeField] private string sceneControllerRotateRightMethod = "RotateRight";
    [SerializeField] private string sceneControllerRotateLeftMethod = "RotateLeft";
	[SerializeField] private string sceneControllerResetMethod = "Reset";
	[SerializeField] private string sceneControllerResetTestCountMethod = "ResetTestCount";

    // boolean to control response to buttons if code is running
    private int runCount = 0;
    private IList isRunning = new ArrayList();

    // time delays between each actions
    [SerializeField] private static float baseActionStepDelay = 5f;
    private float actionStepDelay = baseActionStepDelay;

    // moves in the interaction pane
    [SerializeField] private int movesAvailable;
    private IList actionArray = new ArrayList();

	// goal node trigger (if coin was collected)
	private bool canTrigger = false;

    // moves remaining GUI
    [SerializeField] private float movesRemainingPosX;
	[SerializeField] private float movesRemainingPosY;
	[SerializeField] private float movesRemainingPosZ;
	[SerializeField] private float movesRemainingXOffset;
	[SerializeField] private GameObject digitPrefab;
	[SerializeField] private Sprite[] numberSprites = new Sprite[10];
	private IList movesRemainingArray = new ArrayList();

    //
    // setter and getter methods
    //

    public bool IsRunning
    {
        get { return (bool)isRunning[runCount]; }
        private set { isRunning[runCount] = value; }
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
        if (!IsRunning)
        {
            if (cwButtonClicked)
            {
                AddCwAction();
                Debug.Log("Interaction Pane: Add clockwise rotation sprite");
            }
            else
            {
                AddCcwAction();
                Debug.Log("Interaction Pane: Add counterclockwise rotation sprite");
            }

			// update moves remaining
			UpdateMovesRemaining();
        }
        else
            Debug.Log("Interaction Pane ERROR: Interaction pane is already running");
    }

    /// <summary>
    /// Runs the array of actions in order
    /// </summary>
    public void Run()
    {
        // respond if the interaction pane is not running
        if (!IsRunning)
        {
            // check if the user used all the moves available
            if (actionArray.Count == movesAvailable)
            {
                // increment run count
                runCount++;
                isRunning.Add(true);

                // play running music
                audioControllerTargetObject.SendMessage(audioControllerRunMethod);

                // disable test buttons
                ccwTestButtonTargetObject.SendMessage(rotateButtonDisableMethod);
                cwTestButtonTargetObject.SendMessage(rotateButtonDisableMethod);

                // reset the puzzle scene
                sceneControllerTargetObject.SendMessage(sceneControllerResetMethod);

                // execute the action order in the interactions pane
                Debug.Log("Interaction Pane: Execute action order");
                StartCoroutine(RunStep(runCount, actionArray, 0));
            }
            else
                Debug.Log("Interaction Pane ERROR: Not enough moves");
        }
        else
            Debug.Log("Interaction Pane ERROR: Interactions pane is already running");

    }

    /// <summary>
    /// Clear the array of actions
    /// </summary>
    public void Clear()
    {
        Debug.Log("Interaction Pane: Clear interaction pane");

        // change running state
        IsRunning = false;

        // play background music
        audioControllerTargetObject.SendMessage(audioControllerClearMethod);

        // destroy the current sprites in the action array
        foreach (RotationAction action in actionArray)
        {
            action.Action.SetActive(false);
            Destroy(action.Action);
        }

        // clear the action array
        actionArray.Clear();

		// update moves remaining
		UpdateMovesRemaining();

        // deactivate trigger of goal node
		sceneControllerTargetObject.SendMessage(sceneControllerDeactivateTriggerMethod);

		// reset the test move count
		sceneControllerTargetObject.SendMessage(sceneControllerResetTestCountMethod);

        // reset the scene
        if (sceneControllerTargetObject != null)
            sceneControllerTargetObject.SendMessage(sceneControllerResetMethod);

        // enable test buttons
        if (ccwTestButtonTargetObject != null && cwTestButtonTargetObject != null)
        {
            ccwTestButtonTargetObject.SendMessage(rotateButtonEnableMethod);
            cwTestButtonTargetObject.SendMessage(rotateButtonEnableMethod);
        }
    }

    public void Start()
    {
        // set isRunning
        isRunning.Add(false);

		// update moves remaining
		UpdateMovesRemaining();

        // get original color of rotation sprites
        SpriteRenderer rotationSprite = cwSpritePrefab.GetComponent<SpriteRenderer>();
        originalColor = rotationSprite.color;
    }

    public void Update()
    {
        // update time delay between each action
		actionStepDelay = 7f - (Settings.RotationSpeed * baseActionStepDelay);
    }

    //
    // helper methods
    //

    private IEnumerator RunStep(int currentRunCount, IList actionArray, int actionOrder)
    {
        // check if the interactions pane has not been resetted
        if ((bool)isRunning[currentRunCount])
        {
            if (actionOrder < actionArray.Count)
            {
				// if the action is the last action
				if (actionOrder == (actionArray.Count - 1)) {
					Debug.Log ("Interaction Pane: Check if coin has been collected");

					// if coin collected
					if (canTrigger) {
						// activate trigger of goal node
						sceneControllerTargetObject.SendMessage(sceneControllerActivateTriggerMethod);
						canTrigger = false;
					}
				}

                // unhighlight the action sprite that was previously executed
                if (actionOrder > 0)
                {
                    SpriteRenderer previousAction = ((RotationAction) actionArray[actionOrder - 1]).Action.GetComponent<SpriteRenderer>();
                    previousAction.color = originalColor;
                }

                // highlight the action sprite that is currently being executed
                SpriteRenderer currentAction = ((RotationAction) actionArray[actionOrder]).Action.GetComponent<SpriteRenderer>();
                currentAction.color = highlightColor;


                // call the corresponding rotate action in the scene controller
                if (((RotationAction) actionArray[actionOrder]).CwAction)
                {
                    sceneControllerTargetObject.SendMessage(sceneControllerRotateRightMethod);
                    Debug.Log("Interaction Pane: Rotate right");
                }
                else
                {
                    sceneControllerTargetObject.SendMessage(sceneControllerRotateLeftMethod);
                    Debug.Log("Interaction Pane: Rotate left");
                }

                // wait number of seconds
                yield return new WaitForSeconds(actionStepDelay);

                // if the action is the last action
                if (actionOrder == (actionArray.Count - 1))
                {
                    // change running state
                    isRunning[currentRunCount] = false;

                    // unhighlight the action sprite that was previously executed
                    currentAction.color = originalColor;

                    // play background music
                    audioControllerTargetObject.SendMessage(audioControllerClearMethod);

                    // enable test buttons
                    if (ccwTestButtonTargetObject != null && cwTestButtonTargetObject != null)
                    {
                        ccwTestButtonTargetObject.SendMessage(rotateButtonEnableMethod);
                        cwTestButtonTargetObject.SendMessage(rotateButtonEnableMethod);
                    }       
                }
                else
                {
                    // start next action
                    StartCoroutine(RunStep(currentRunCount, actionArray, (actionOrder + 1)));
                }
            }
        }
    }

    private void UpdateMovesRemaining()
    {
		// destroy the current number sprites in the moves remaining array
		foreach (GameObject numSprite in movesRemainingArray) {
			numSprite.SetActive (false);
			Destroy (numSprite);
		}

		// clear the moves remaining array
		movesRemainingArray.Clear();

		// determine the number of moves remaining
		int movesRemaining = Math.Max((movesAvailable - actionArray.Count), 0);

		// update the GUI
		string movesRemainingString = "" + movesRemaining;
		foreach (char num in movesRemainingString) {
			// instantiate new digit sprite
			int digit = num - '0';
			GameObject digitSprite = Instantiate (digitPrefab) as GameObject;
			digitSprite.GetComponent<SpriteRenderer>().sprite = numberSprites[digit];

			// set the position
			float xPos = movesRemainingPosX + (movesRemainingArray.Count * movesRemainingXOffset);
			digitSprite.transform.localPosition = new Vector3(xPos, movesRemainingPosY, movesRemainingPosZ);

			// set the sprite active
			digitSprite.SetActive(true);

			// add to moves remaining array
			movesRemainingArray.Add(digitSprite);
		}
    }

    private void AddCwAction()
    {
        if (actionArray.Count < movesAvailable)
        {
            // instantiate new cw sprite
            GameObject cwAction = Instantiate(cwSpritePrefab) as GameObject;

            // set the position
            float posX = xStartPos + ((int)(actionArray.Count / 5)) * xOffset;
            float posY = yStartPos - (actionArray.Count % 5) * yOffset;
			cwAction.transform.localPosition = new Vector3(posX, posY, zStartPos);

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
            float posX = xStartPos + ((int)(actionArray.Count / 5)) * xOffset;
            float posY = yStartPos - (actionArray.Count % 5) * yOffset;
            float posZ = zStartPos;
            ccwAction.transform.localPosition = new Vector3(posX, posY, posZ);

            // set the sprite active
            ccwAction.SetActive(true);

            // add to action array
            actionArray.Add(new RotationAction(false, ccwAction));
        }
    }

	void CanTrigger() {
		Debug.Log("Interaction Pane: Coin(s) collected");
		canTrigger = true;
	}
}