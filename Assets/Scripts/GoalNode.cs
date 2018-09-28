using UnityEngine;
using System.Collections;

public class GoalNode : MonoBehaviour
{
	[SerializeField] private GameObject sceneController;
	
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("GAME OVER");
		sceneController.SendMessage ("Destroy");
	}


}

