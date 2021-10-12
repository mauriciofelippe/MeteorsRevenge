using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private CharacterController2D controller2D;
        [SerializeField] private Animator animator;

        public static int Keys;
        
        private float _moveX;
        private const float Speed = 40f;

        private float distJump = 0;
        private const float MaxDistJump = 2.9f;

        private bool _jump = false;
        private static readonly int Walk = Animator.StringToHash("walk");
        private static readonly int Jump = Animator.StringToHash("jump");
        private static readonly int JumpTime = Animator.StringToHash("jumpTime");
        private static readonly int Ground = Animator.StringToHash("ground");
        private static readonly int DeadByFall = Animator.StringToHash("deadByFall");

        private void Start()
        {
            if (controller2D == null) controller2D = GetComponent<CharacterController2D>();

            controller2D.onLandEvent.AddListener(PlayerIsNotJumping);
            controller2D.onJumpEvent.AddListener(PlayerIsJumping);

            Keys = 0;
        }


        private void PlayerIsJumping()
        {
            animator.SetBool(Jump, true);
            distJump = transform.position.y;
        }

        private void PlayerIsNotJumping()
        {
        animator.SetBool(Jump, false);
        
        if(controller2D.distanceFromGround > MaxDistJump)
            animator.SetBool(DeadByFall, true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
            
            
            _moveX = Input.GetAxisRaw("Horizontal") * Speed;
            if (Input.GetButtonDown("Jump"))
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                 controller2D.MoveDownOneWayPlatform();   
                }
                else
                {
                    _jump = true;    
                }
            }
            animator.SetBool(Walk,Mathf.Abs(_moveX)>0);
            animator.SetFloat(JumpTime, controller2D.timeOutOfGround);
            animator.SetBool(Ground, controller2D.mGrounded);
        }

        private void FixedUpdate()
        {
            controller2D.Move(_moveX * Time.fixedDeltaTime,false,_jump);
            _jump = false;
            
            controller2D.FixedUpdateMe();
        }
    }
}
