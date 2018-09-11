using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {



	void OnCollisionEnter2D(Collision2D other) {

		if (other.gameObject.tag.Equals ("Player")) {
			GameController.instance.UpdateScore ();
		} else if (other.gameObject.tag.Equals ("Ground")) {
			GameController.instance.gameOver = true;
		}

		//Debug.Log("OnCollisionEnter2D");
	}


}
