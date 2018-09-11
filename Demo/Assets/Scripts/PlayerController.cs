using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour {

	[SerializeField] private float jumpForce = 700f;
	[Range (0, 0.3f)] [SerializeField] private float movementSmoothing = 0.05f;
	[SerializeField] private bool airControl = true;
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform ceilingCheck;

	const float groundedRadius = 0.2f;
	private bool isGrounded;
	const float ceilingRadius = 0.2f;
	private Rigidbody2D rbPlayer;
	private bool facingRight = true;
	private Vector3 velocity = Vector3.zero;

	[Header("Events")]
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent: UnityEvent<bool> { }


	void Awake() {
		rbPlayer = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent ();
	}


	void FixedUpdate() {
		bool wasGrounded = isGrounded;
		isGrounded = false;

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
		if (isGrounded || airControl) {

			Vector3 targetVelocity = new Vector2(move * 10f, rbPlayer.velocity.y);
			rbPlayer.velocity = Vector3.SmoothDamp(rbPlayer.velocity, targetVelocity, ref velocity, movementSmoothing);

			if (move > 0 && !facingRight) {
				Flip();
			}
			else if (move < 0 && facingRight) {
				Flip();
			}
		}

		if (isGrounded && jump) {
			isGrounded = false;
			rbPlayer.AddForce(new Vector2(0f, jumpForce));
		}
	}


	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


}