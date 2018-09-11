using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerController playerController;
	public Animator animPlayer;
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;

	void Update () 	{
		horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;
		animPlayer.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown ("Jump")) {
			jump = true;
			animPlayer.SetBool("IsJumping", true);
		}

	}


	public void onLanding() {
		animPlayer.SetBool ("IsJumping", false);
	}



	void FixedUpdate () {
		playerController.Move (horizontalMove * Time.fixedDeltaTime, jump);	
		jump = false;
	}


}