using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour {

	[SerializeField] private float jumpForce = 700f;							// Amount of force added when the player jumps
	[Range (0, 0.3f)] [SerializeField] private float movementSmoothing = 0.05f;	// How much to smooth out the movement
	[SerializeField] private bool airControl = true;							// Can player steer while jumping
	[SerializeField] private LayerMask whatIsGround;							// Mask determining what is ground to the character
	[SerializeField] private Transform groundCheck;								// Position marking where to check if the player is grounded
	[SerializeField] private Transform ceilingCheck;							// Position marking where to check for ceilings

	const float groundedRadius = 0.2f;				// Radius of the overlap circle to determine if grounded
	private bool isGrounded;						// Whether or not the player is grounded
	const float ceilingRadius = 0.2f;				// Radius of the overlap circle to determine if the player can stand up.  Not needed, used if crouching is added
	private Rigidbody2D rbPlayer;					// Player's rigidbody
	private bool facingRight = true;				// Direction player is currently facing
	private Vector3 velocity = Vector3.zero;		// Velocity

	[Header("Events")]
	public UnityEvent OnLandEvent;					// Part of the 'fix' to show second part of jumping animation

	[System.Serializable]
	public class BoolEvent: UnityEvent<bool> { }	// For above


	void Awake() {
		rbPlayer = GetComponent<Rigidbody2D>();		// Player's rigidbody

		if (OnLandEvent == null)					// Part of the aboive 'fix'
			OnLandEvent = new UnityEvent ();
	}


	void FixedUpdate() {
		bool wasGrounded = isGrounded;				// Hold current status of isGrounded
		isGrounded = false;							// Flip it off

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This could be done using layers instead
		Collider2D[] colliders = Physics2D.OverlapCircleAll (groundCheck.position, groundedRadius, whatIsGround);	

		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				isGrounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke ();
			}
		}

	}


	public void Move(float move, bool jump) {
		// Crouching code would go here.   No crouching animation, and no need for crouching

		// Only control the player if grounded or airControl is turned on
		if (isGrounded || airControl) {


			Vector3 targetVelocity = new Vector2(move * 10f, rbPlayer.velocity.y);			// Move the player by finding the target velocity
			rbPlayer.velocity = Vector3.SmoothDamp(rbPlayer.velocity, targetVelocity, ref velocity, movementSmoothing);	// And then smooth it out and apply it to the player

			if (move > 0 && !facingRight) {			// If the input is moving the player right and the player is facing left
				Flip();								// Flip him
			}
			else if (move < 0 && facingRight) {		// Otherwise if the input is moving the player left and the player is facing right
				Flip();								// Flip him
			}
		}

		if (isGrounded && jump) {							// If the player should jump
			isGrounded = false;								// Flip isGrounded to false
			rbPlayer.AddForce(new Vector2(0f, jumpForce));	// Add a vertical force to the player
		}
	}



	void Flip()
	{
		facingRight = !facingRight;					// Switch the way the player is facing
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;							// Multiply the player's x local scale by -1
		transform.localScale = theScale;
	}


}