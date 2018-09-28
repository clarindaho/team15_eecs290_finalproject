using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
	/**
	 * Fields for maxAngle, clickability of buttons, and whether or not rotation is left or right
	 */
	private float angleSoFar = 0;
	private bool canClick = true;
	private bool left;
	private bool right;
	private bool isReset = false;

	/* Should add a goalAccessible boolean */

	[SerializeField] private GameObject centerOfMap;
	[SerializeField] private GameObject cubeLoc;
	[SerializeField] private GameObject puzzleLoc;
	[SerializeField] private GameObject goalLoc;
	[SerializeField] private GameObject coinLoc;
	[SerializeField] private GameObject puzzlePrefab;
	[SerializeField] private GameObject cubePrefab;
	[SerializeField] private GameObject goalPrefab;
	[SerializeField] private GameObject coinPrefab;

	[SerializeField] private GameObject ccwButtonTargetObject;
	[SerializeField] private GameObject cwButtonTargetObject;
	[SerializeField] private GameObject cwTestButtonTargetObject;
	[SerializeField] private GameObject ccwTestButtonTargetObject;
	[SerializeField] private GameObject runButtonTargetObject;
	[SerializeField] private GameObject resetButtonTargetObject;

	[SerializeField] private GameObject levelCompleteButton;
	[SerializeField] private GameObject nextLevelButtonLoc;


	// how many test moves we allow for the scene
	public int testMovesAllowed = 3;

	[SerializeField] private float movesRemainingPosX = -1.9f;
	[SerializeField] private float movesRemainingPosY = -2.6f;
	[SerializeField] private GameObject digitPrefab;
	[SerializeField] private Sprite[] numberSprites = new Sprite[10];
	private GameObject digitSprite;
	public int testMovesLeft = 3;
	public int oldTestMovesLeft = 3;

	private GameObject currentPuzzle;
	private GameObject currentCube;
	private GameObject currentGoal;
	private GameObject currentCoin;

	// how many times we have pressed test button
	private int testCount = 0;

	public GameObject interactionPane; 

	private bool gameOver = false;

	[SerializeField] private static float baseSpeed = 50f;
	private float speed = baseSpeed;

	// Use this for initialization
	void Start () {
		// Switch to 1600 x 900 fullscreen
		Screen.SetResolution(1600, 900, true);

		// Instantiate currentPuzzle as new puzzle prefab
		currentPuzzle = Instantiate (puzzlePrefab) as GameObject;
		currentPuzzle.transform.position = puzzleLoc.transform.position;

		// Instantiate currentCube
		currentCube = Instantiate (cubePrefab) as GameObject;
		currentCube.transform.position = cubeLoc.transform.position;

		// Instantiate currentGoal
		currentGoal = Instantiate (goalPrefab) as GameObject;
		currentGoal.transform.position = goalLoc.transform.position;

		// Instantiate currentCoin
		currentCoin = Instantiate (coinPrefab) as GameObject;
		currentCoin.transform.position = coinLoc.transform.position;
		Debug.Log ("Instantiation done");

		// instantiate new digit sprite
		int digit = testMovesLeft;
		digitSprite = Instantiate (digitPrefab) as GameObject;
		Debug.Log (digit);
		digitSprite.GetComponent<SpriteRenderer>().sprite = numberSprites[digit];

		// set the position
		digitSprite.transform.localPosition = new Vector3(movesRemainingPosX, movesRemainingPosY, 0);

		// set the sprite active
		digitSprite.SetActive(true);

	}

	void RotateLeft(){
		if (canClick) {
			this.left = true;
			this.right = false;
			angleSoFar = 0;
			canClick = false;
		}
	}

	void RotateRight(){
		if (canClick) {
			this.left = false;
			this.right = true;
			angleSoFar = 0;
			canClick = false;
		}
	}

	void RotateLeftTest(){
		if (canClick && testCount < testMovesAllowed) {
			testCount++;
			testMovesLeft--;
			this.left = true;
			this.right = false;
			angleSoFar = 0;
			canClick = false;
		}
	}

	void RotateRightTest(){
		if (canClick && testCount < testMovesAllowed) {
			testCount++;
			testMovesLeft--;
			this.left = false;
			this.right = true;
			angleSoFar = 0;
			canClick = false;
		}
	}

	private IEnumerator TurnOnCanClick() {
		yield return new WaitForSeconds (1f);
		canClick = true;
	}

	// Update is called once per frame
	void Update () {
		if (testMovesLeft != oldTestMovesLeft) {
			oldTestMovesLeft = testMovesLeft;
			Debug.Log ("Change TEST DIGIT");
			UpdateTestNumber ();
		}
		if (isReset) {
			Reset ();
			isReset = false;
		} else {
			speed = 140f - (1 - Settings.RotationSpeed) * baseSpeed;
			if (!gameOver) {
				if (left && angleSoFar < 90) {
					float rot = speed * Time.deltaTime;
					currentCube.GetComponent<Rigidbody2D> ().gravityScale = 0;

					if (rot + angleSoFar <= 90) {
						Debug.Log ("rotating left");
						angleSoFar += rot;
					} else {
						rot = 90 - angleSoFar;

						Debug.Log ("rotating left");
						angleSoFar = 90;
						canClick = false;

						StartCoroutine (TurnOnCanClick ());
					}

					currentPuzzle.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, 1), rot);
					currentCube.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, 1), rot);
					currentGoal.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, 1), rot);
					if(currentCoin != null)
						currentCoin.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, 1), rot);

				} else if (right && angleSoFar < 90) {
					float rot = speed * Time.deltaTime;
					currentCube.GetComponent<Rigidbody2D> ().gravityScale = 0;
					if (rot + angleSoFar <= 90) {
						Debug.Log ("rotating right");
						angleSoFar += rot;
					} else {
						rot = 90 - angleSoFar;

						Debug.Log ("rotating right");
						angleSoFar = 90;
						canClick = false;

						StartCoroutine (TurnOnCanClick ());
					}

					currentPuzzle.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, -1), rot);
					currentCube.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, -1), rot);
					currentGoal.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, -1), rot);
					if(currentCoin != null)
						currentCoin.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, -1), rot);
				} else if (currentCube != null) {
						currentCube.GetComponent<Rigidbody2D> ().gravityScale = 1;
				}
			} else {
				LevelOver ();
			}
		}
	}

	void UpdateTestNumber() {
		// instantiate new digit sprite
		int digit = testMovesLeft;
		Destroy (digitSprite);
		digitSprite = Instantiate (digitPrefab) as GameObject;
		digitSprite.GetComponent<SpriteRenderer>().sprite = numberSprites[digit];

		// set the position
		digitSprite.transform.localPosition = new Vector3(movesRemainingPosX, movesRemainingPosY, 0);

		// set the sprite active
		digitSprite.SetActive(true);
	}

	void ActivateTrigger() {
		currentGoal.GetComponent<BoxCollider2D> ().isTrigger = true;
	}

	void DeactivateTrigger() {
		currentGoal.GetComponent<BoxCollider2D> ().isTrigger = false;
	}

	void Reset() {
		if (!gameOver && canClick) {
			Debug.Log ("Resetting");

			DeactivateTrigger ();

			Destroy (currentPuzzle);
			Destroy (currentCube);
			Destroy (currentGoal);
			Destroy (currentCoin);

			// Instantiate currentPuzzle as new puzzle prefab
			currentPuzzle = Instantiate (puzzlePrefab) as GameObject;
			currentPuzzle.transform.position = puzzleLoc.transform.position;

			// Instantiate currentCube
			currentCube = Instantiate (cubePrefab) as GameObject;
			currentCube.transform.position = cubeLoc.transform.position;

			// Instantiate currentGoal
			currentGoal = Instantiate (goalPrefab) as GameObject;
			currentGoal.transform.position = goalLoc.transform.position;

			// Instantiate currentCoin
			currentCoin = Instantiate (coinPrefab) as GameObject;
			currentCoin.transform.position = coinLoc.transform.position;

			Debug.Log ("Resetting done");
		}
	}

	void ResetTestCount() {
		testCount = 0;
		testMovesLeft = testMovesAllowed;

		// instantiate new digit sprite
		int digit = testMovesAllowed;
		Destroy (digitSprite);
		digitSprite = Instantiate (digitPrefab) as GameObject;
		digitSprite.GetComponent<SpriteRenderer>().sprite = numberSprites[digit];

		// set the position
		digitSprite.transform.localPosition = new Vector3(movesRemainingPosX, movesRemainingPosY, 0);

		// set the sprite active
		digitSprite.SetActive(true);
	}

	void DestroyCoin() {
		Debug.Log ("Destroying");

		Destroy (currentCoin);

		currentCoin = null;
	}

	void Destroy() {
		Debug.Log ("Destroying");

		Destroy (currentPuzzle);
		Destroy (currentCube);
		Destroy (currentGoal);

		Destroy(ccwButtonTargetObject);
		Destroy(cwButtonTargetObject);
		Destroy(ccwTestButtonTargetObject);
		Destroy(cwTestButtonTargetObject);
		Destroy(runButtonTargetObject);
		Destroy(resetButtonTargetObject);

		currentPuzzle = null;
		currentCube = null;
		currentGoal = null;

		canClick = false;
		gameOver = true;
	}

	void LevelOver() {
		Debug.Log ("Level beaten");

		// To stop update from continually calling this method
		gameOver = false;

		// Instantiate nextLevelButton
		GameObject nextLevel = Instantiate (levelCompleteButton) as GameObject;
		nextLevel.transform.position = nextLevelButtonLoc.transform.position;
	}

	void ToStart() {
		SceneManager.LoadScene ("Start");
	}

	void ToLevel2() {
		SceneManager.LoadScene ("MediumLevel");
	}

	void ToLevel3() {
		SceneManager.LoadScene ("HardLevel");
	}
		
	void ToGameOver() {
		SceneManager.LoadScene ("GameOver");
	}
}