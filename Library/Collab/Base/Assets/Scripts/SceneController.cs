using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
	/**
	 * Fields for maxAngle, clickability of buttons, and whether or not rotation is left or right
	 */
	private float maxAngle = 90f;
	private bool canClick = true;
	private bool left;
	private bool right;

	/* Should add a goalAccessible boolean */

	[SerializeField] private GameObject centerOfMap;
	[SerializeField] private GameObject cubeLoc;
	[SerializeField] private GameObject puzzleLoc;
	[SerializeField] private GameObject goalLoc;
	[SerializeField] private GameObject puzzlePrefab;
	[SerializeField] private GameObject cubePrefab;
	[SerializeField] private GameObject goalPrefab;

	[SerializeField] private GameObject ccwButtonTargetObject;
	[SerializeField] private GameObject cwButtonTargetObject;
	[SerializeField] private GameObject cwTestButtonTargetObject;
	[SerializeField] private GameObject ccwTestButtonTargetObject;
	[SerializeField] private GameObject runButtonTargetObject;
	[SerializeField] private GameObject resetButtonTargetObject;

	public GameObject currentPuzzle;
	public GameObject currentCube;
	public GameObject currentGoal;

	public GameObject interactionPane; 

	private bool gameOver = false;

	public static float speed = 30f; 
	public Slider speedSlider; 

	// Use this for initialization
	void Start () {
		// Instantiate currentPuzzle as new puzzle prefab
		currentPuzzle = Instantiate (puzzlePrefab) as GameObject;
		currentPuzzle.transform.position = puzzleLoc.transform.position;

		// Instantiate currentCube
		currentCube = Instantiate (cubePrefab) as GameObject;
		currentCube.transform.position = cubeLoc.transform.position;

		// Instantiate currentGoal
		currentGoal = Instantiate (goalPrefab) as GameObject;
		currentGoal.transform.position = goalLoc.transform.position;

		Debug.Log ("Instantiation done");

	}

	void RotateLeft(){
		if (canClick) {
			this.left = true;
			this.right = false;
			maxAngle = 90f;
			canClick = false;
		}
	}

	void RotateRight(){
		if (canClick) {
			this.left = false;
			this.right = true;
			maxAngle = 90f;
			canClick = false;
		}
	}

	private IEnumerator TurnOnCanClick() {
		yield return new WaitForSeconds (1f);
		canClick = true;
	}

	// Update is called once per frame
	void Update () {
		speed = 100 * speedSlider.value;
		if (!gameOver) {
			if (left && maxAngle >= 0) {
				currentCube.GetComponent<Rigidbody2D> ().gravityScale = 0;

				float rot = speed * Time.deltaTime;

				currentPuzzle.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, 1), rot);
				currentCube.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, 1), rot);
				currentGoal.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, 1), rot);

				Debug.Log ("rotating left");
				maxAngle -= rot;
			} else if (right && maxAngle >= 0) {
				currentCube.GetComponent<Rigidbody2D> ().gravityScale = 0;

				float rot = speed * Time.deltaTime;

				currentPuzzle.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, -1), rot);
				currentCube.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, -1), rot);
				currentGoal.transform.RotateAround (centerOfMap.transform.position, new Vector3 (0, 0, -1), rot);

				Debug.Log ("rotating right");
				maxAngle -= rot;
			} else if (!canClick) {
				currentCube.GetComponent<Rigidbody2D> ().gravityScale = 1;
				StartCoroutine (TurnOnCanClick ());
			} else {
				if (currentCube != null)
					currentCube.GetComponent<Rigidbody2D> ().gravityScale = 1;
			}
		} else {
			GameOver ();
		}
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

			// Instantiate currentPuzzle as new puzzle prefab
			currentPuzzle = Instantiate (puzzlePrefab) as GameObject;
			currentPuzzle.transform.position = puzzleLoc.transform.position;

			// Instantiate currentCube
			currentCube = Instantiate (cubePrefab) as GameObject;
			currentCube.transform.position = cubeLoc.transform.position;

			// Instantiate currentGoal
			currentGoal = Instantiate (goalPrefab) as GameObject;
			currentGoal.transform.position = goalLoc.transform.position;

			Debug.Log ("Resetting done");
		}
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

	void GameOver() {
		SceneManager.LoadScene ("GameOver");
	}



}