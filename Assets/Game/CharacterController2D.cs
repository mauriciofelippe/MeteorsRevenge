using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game
{
	public class CharacterController2D : MonoBehaviour
	{
		[SerializeField] private float mJumpForce = 400f;							// Amount of force added when the player jumps.
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
		private Rigidbody2D _mRigidbody2D;
		private bool _mFacingRight = true;  // For determining which way the player is currently facing.
		private Vector3 _mVelocity = Vector3.zero;

		//If < .75 is Alive more Death for Fall
		public float timeOutOfGround = 0f;

		public float distanceFromGround;
		private float _lastGroundedPositionYaxis = 0f;
		
		[FormerlySerializedAs("OnLandEvent")]
		[Header("Events")]
		[Space]

		public UnityEvent onLandEvent;
		public UnityEvent onJumpEvent;

		public OneWayPlatform oneWayPlatform;

		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		// [FormerlySerializedAs("OnCrouchEvent")] public BoolEvent onCrouchEvent;
		// private bool _mWasCrouching = false;

		private void Awake()
		{
			_mRigidbody2D = GetComponent<Rigidbody2D>();

			if (onLandEvent == null)
				onLandEvent = new UnityEvent();

			// if (onCrouchEvent == null)
			// 	onCrouchEvent = new BoolEvent();
		}

		private bool lastGrounded;
		
		public void FixedUpdateMe()
		{
			bool wasGrounded = mGrounded;
			// _mGrounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			// var colliders = Physics2D.OverlapCircleAll(mGroundCheck.position, KGroundedRadius, mWhatIsGround);

			var collider2d = Physics2D.OverlapCircle(mGroundCheck.position, KGroundedRadius, mWhatIsGround);

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
				
				oneWayPlatform = collider2d.GetComponent<OneWayPlatform>();
				
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
			
			// foreach (var t in colliders)
			// {
			// 	if (t.gameObject != gameObject)
			// 	{
			// 		_mGrounded = true;
			// 		if (!wasGrounded)
			// 		{
			// 			landedCount++;
			// 			print("Landed: "+landedCount);
			// 			if (landedCount > 1)
			// 			{
			// 				landedCount = 0;
			// 				_timeOutOfGround = 0f;		
			// 				onLandEvent.Invoke();
			// 			}
			// 		}
			// 			
			// 	}
			// }

			if (mGrounded == false)
				timeOutOfGround += Time.fixedDeltaTime;
		}
		
		// private void FixedUpdate()
		// {
		// 	bool wasGrounded = _mGrounded;
		// 	_mGrounded = false;
		// 	
		// 	// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// 	// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		// 	var colliders = Physics2D.OverlapCircleAll(mGroundCheck.position, KGroundedRadius, mWhatIsGround);
		// 	foreach (var t in colliders)
		// 	{
		// 		if (t.gameObject != gameObject)
		// 		{
		// 			_mGrounded = true;
		// 			if (!wasGrounded)
		// 			{
		// 				landedCount++;
		// 				print("Landed: "+landedCount);
		// 				if (landedCount > 1)
		// 				{
		// 					landedCount = 0;
		// 					_timeOutOfGround = 0f;		
		// 					onLandEvent.Invoke();
		// 				}
		// 			}
		// 				
		// 		}
		// 	}
		// 	
		// 	if (_mGrounded == false)
		// 		_timeOutOfGround += Time.fixedDeltaTime;
		// }


		public void MoveDownOneWayPlatform()
		{
			if (oneWayPlatform != null)
			{
				oneWayPlatform.TurnPlatformOff();
			}
		}
		
		public void Move(float move, bool crouch, bool jump)
		{

			//only control the player if grounded or airControl is turned on
			if (mGrounded || mAirControl)
			{
				
				// Move the character by finding the target velocity
				var velocity = _mRigidbody2D.velocity;
				Vector3 targetVelocity = new Vector2(move * 10f, velocity.y);
				// And then smoothing it out and applying it to the character
				_mRigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _mVelocity, mMovementSmoothing);

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
			// If the player should jump...
			if (!mGrounded || !jump) return;
			// Add a vertical force to the player.
			mGrounded = false;
			onJumpEvent.Invoke();
			distanceFromGround += 1;
			_mRigidbody2D.AddForce(new Vector2(0f, mJumpForce));
		}


		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			_mFacingRight = !_mFacingRight;

			// Multiply the player's x local scale by -1.
			var transform1 = transform;
			var theScale = transform1.localScale;
			theScale.x *= -1;
			transform1.localScale = theScale;
		}
	}
}