using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
	void ToInstructions() {
		SceneManager.LoadScene ("Instructions");
	}

	void ToLevel1() {
		SceneManager.LoadScene ("EasyLevel");
	}

	void ToStart() {
		SceneManager.LoadScene ("Start");
	}
}

