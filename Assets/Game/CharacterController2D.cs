using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game
{
	public class CharacterController2D : MonoBehaviour
	{
		[SerializeField] internal float speed = 10;
		[SerializeField] internal float mJumpForce = 400f;							// Amount of force added when the player jumps.
		// [Range(0, 1)] [SerializeField] private float mCrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
		[Range(0, .3f)] [SerializeField] private float mMovementSmoothing = .05f;	// How much to smooth out the movement
		[SerializeField] private bool mAirControl = false;							// Whether or not a player can steer while jumping;
		[SerializeField] private LayerMask mWhatIsGround;							// A mask determining what is ground to the character
		[SerializeField] private Transform mGroundCheck;							// A position marking where to check if the player is grounded.
		// [SerializeField] private Transform mCeilingCheck;							// A position marking where to check for ceilings
		// [SerializeField] private Collider2D mCrouchDisableCollider;				// A collider that will be disabled when crouching

		private const float KGroundedRadius = .3f; // Radius of the overlap circle to determine if grounded
		public bool mGrounded;            // Whether or not the player is grounded.
		private const float KCeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
		[SerializeField] internal Rigidbody2D rigidbody2D;
		private bool _mFacingRight = true;  // For determining which way the player is currently facing.
		private Vector3 _mVelocity = Vector3.zero;

		//If < .75 is Alive more Death for Fall
		public float timeOutOfGround = 0f;

		public float distanceFromGround;
		private float _lastGroundedPositionYaxis = 0f;
		
		[FormerlySerializedAs("OnLandEvent")]
		[Header("Events")]
		
		public OneWayPlatform oneWayPlatform;

		public bool isOnWayPlatform;
		
		[Space]

		public UnityEvent onLandEvent;
		public UnityEvent onJumpEvent;

	
		
		
		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		// [FormerlySerializedAs("OnCrouchEvent")] public BoolEvent onCrouchEvent;
		// private bool _mWasCrouching = false;

		private void Awake()
		{
			rigidbody2D = GetComponent<Rigidbody2D>();

			if (onLandEvent == null)
				onLandEvent = new UnityEvent();

			// if (onCrouchEvent == null)
			// 	onCrouchEvent = new BoolEvent();
		}

		private bool lastGrounded;

		// private void OnDrawGizmos()
		// {
		// 	Gizmos.color = Color.red;
		// 	Gizmos.DrawWireSphere(mGroundCheck.position,KGroundedRadius);
		// }

		private Collider2D collider2d;
		
		public void FixedUpdateMe()
		{
			bool wasGrounded = mGrounded;
			// _mGrounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			// var colliders = Physics2D.OverlapCircleAll(mGroundCheck.position, KGroundedRadius, mWhatIsGround);

			collider2d = Physics2D.OverlapCircle(mGroundCheck.position, KGroundedRadius, mWhatIsGround);

			
			if (collider2d != null)
			{
				if (wasGrounded == false && timeOutOfGround > 0.1f)
				{
					timeOutOfGround = 0f;
					distanceFromGround = _lastGroundedPositionYaxis - transform.position.y;
					// print("Distance: "+distanceFromGround+" Pos1: "+_lastGroundedPositionYaxis+" Pos2: "+transform.position.y);
					onLandEvent.Invoke();
				}
				mGrounded = true;

				isOnWayPlatform = collider2d.CompareTag("OneWay");

				// oneWayPlatform = collider2d.GetComponent<OneWayPlatform>();
				
				lastGrounded = mGrounded;
			}
			else
			{
				distanceFromGround = 0f;
				mGrounded = false;
			}

			if (mGrounded == false && mGrounded != lastGrounded)
			{
				lastGrounded = mGrounded;
				_lastGroundedPositionYaxis = transform.position.y;
			}
			
			if (mGrounded == false)
				timeOutOfGround += Time.fixedDeltaTime;
		}
	
		public void MoveDownOneWayPlatform()
		{
			if (isOnWayPlatform)
			{
				collider2d.GetComponent<OneWayPlatform>().TurnPlatformOff();
			}
		}
		
		public void Move(float move, bool crouch, bool jump, bool jumpHeld)
		{

			//only control the player if grounded or airControl is turned on
			if (mGrounded || mAirControl)
			{
				
				// Move the character by finding the target velocity
				// var velocity = rigidbody2D.velocity;
				// Vector3 targetVelocity = new Vector2(move * 10f, velocity.y);
				// And then smoothing it out and applying it to the character
				//rigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _mVelocity, mMovementSmoothing);


				
				
				
				rigidbody2D.AddForce(Vector2.right * (move * speed), ForceMode2D.Force);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !_mFacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && _mFacingRight)
				{
					// ... flip the player.
					Flip();
				}
			}
			
			
			if(Mathf.Abs(rigidbody2D.velocity.y) <= 0)
				rigidbody2D.drag = Mathf.Abs(move) < 0.1f ? 40 : 10;
			else
			{
				rigidbody2D.drag = 10;
			}
			
			// If the player should jump...
			if (mGrounded && jump && rigidbody2D.velocity.y <= 0)
			{
				mGrounded = false;
				onJumpEvent.Invoke();
				distanceFromGround += 1;
				rigidbody2D.AddForce(Vector2.up*mJumpForce,ForceMode2D.Impulse);
				// Add a vertical force to the player.
			}
			
			if(jumpHeld && timeOutOfGround < 0.2f)
				rigidbody2D.AddForce(Vector2.up * (mJumpForce * 10));
		}

		public void StopBody()
		{
			rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
			
		}

		public void StartBody()
		{
			rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}


		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			_mFacingRight = !_mFacingRight;

			// Multiply the player's instance local scale by -1.
			var transform1 = transform;
			var theScale = transform1.localScale;
			theScale.x *= -1;
			transform1.localScale = theScale;
		}
	}
}