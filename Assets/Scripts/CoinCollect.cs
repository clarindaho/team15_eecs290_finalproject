using System.Collections;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
	[SerializeField] private GameObject sceneController;
	[SerializeField] private GameObject interactionsPane;

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Coin Collected");
		interactionsPane.SendMessage ("CanTrigger");
		sceneController.SendMessage ("DestroyCoin");
	}


}
