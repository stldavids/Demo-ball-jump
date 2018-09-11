using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {



	void OnCollisionEnter2D(Collision2D other) {  // Checks for 2D Collisions

		if (other.gameObject.tag.Equals ("Player")) {			// If the ball hit an object wih the tag "Player"
			GameController.instance.UpdateScore ();				// Calls UpdateScore() from GameController script (adds one to score and updates score display)
		} else if (other.gameObject.tag.Equals ("Ground")) {	// If the ball hits an object with the tag "Ground"
			GameController.instance.gameOver = true;			// Game/level over.  Set gameOver boolean to true so GameController's Ipdate() will catch it and call EndGame()
		}

		//Debug.Log("OnCollisionEnter2D");  					// Testing
	}


}
