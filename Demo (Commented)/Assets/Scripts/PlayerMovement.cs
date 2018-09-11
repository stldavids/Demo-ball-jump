using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerController playerController;	// Player controller
	public Animator animPlayer;					// Animator
	public float runSpeed = 40f;				// Run speed, defaults to 40

	float horizontalMove = 0f;					// Hortizontal move, defaults to 0
	bool jump = false;							// jump boolean, defaults to false

	void Update () 	{
		horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;	// Get input
		animPlayer.SetFloat("Speed", Mathf.Abs(horizontalMove));		// Pass speed to animator

		if (Input.GetButtonDown ("Jump")) {								// If jump button is pressed
			jump = true;												// Set jump boolean to true
			animPlayer.SetBool("IsJumping", true);						// Pass true to animator to play jump animation
		}

	}


	// Transition 'fix' so down frame of jumping animation will be played
	public void onLanding() {
		animPlayer.SetBool ("IsJumping", false);
	}


	// Moved the characer
	void FixedUpdate () {
		playerController.Move (horizontalMove * Time.fixedDeltaTime, jump);	
		jump = false;
	}


}