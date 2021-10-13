using System.Collections;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] internal CharacterController2D controller2D;
        [SerializeField] internal Animator animator;

        public static int Keys;

        internal float _moveX;
        internal const float Speed = 40f;

        private float distJump = 0;
        private const float MaxDistJump = 2.9f;

        internal bool _jump = false;
        internal readonly int Walk = Animator.StringToHash("walk");
        internal readonly int Jump = Animator.StringToHash("jump");
        internal readonly int JumpTime = Animator.StringToHash("jumpTime");
        internal readonly int Ground = Animator.StringToHash("ground");
        internal readonly int DeadByFall = Animator.StringToHash("deadByFall");

        internal readonly StateMachine StateMachine = new StateMachine();


        private void OnEnable()
        {
            StateMachine.ChangeState(new Walking(this, controller2D));
        }

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
                StateMachine.ChangeState(new Dead(this, default));
        }

        private void Update()
        {
            StateMachine.DirectUpdate();
        }

        private void FixedUpdate()
        {
            controller2D.Move(_moveX * Time.fixedDeltaTime,false,_jump);
            _jump = false;
            
            controller2D.FixedUpdateMe();
        }
    }

    public class Walking : CommandState
    {

        private readonly PlayerController player;
        private readonly CharacterController2D controller2D;

        public Walking(PlayerController player, CharacterController2D controller2D)
        {
            this.player = player;
            this.controller2D = controller2D;
        }

        public override void Enter()
        {
            
        }

        public override void Execute()
        {
            player._moveX = Input.GetAxisRaw("Horizontal") * PlayerController.Speed;
            
            if (Input.GetButtonDown("Jump"))
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    controller2D.MoveDownOneWayPlatform();
                }
                else
                {
                    player._jump = true;    
                }
            }
            
            player.animator.SetBool(player.Walk,Mathf.Abs(player._moveX)>0);
            player.animator.SetBool(player.Ground, player.controller2D.mGrounded);
            player.animator.SetFloat(player.JumpTime, player.controller2D.timeOutOfGround);
        }

        public override void Exit()
        {
            
        }
    }
    public class Dead : CommandState
    {
        private readonly PlayerController player;
        private readonly ItemType item;

        public Dead(PlayerController player, ItemType item)
        {
            this.player = player;
            this.item = item;
        }

        public override void Enter()
        {
            player.StartCoroutine(StopBody());
            
            switch (item)
            {
                case ItemType.Snake:
                    break;
                case ItemType.Skull:
                    break;
                case ItemType.Spider:
                    break;
                case ItemType.Lava:
                    break;
                case ItemType.FireChainWall:
                    break;
                default: //distance
                    player.animator.SetBool(player.DeadByFall, true);
                    break;
            }
        }

        private IEnumerator StopBody()
        {
            yield return new WaitForSecondsRealtime(.2f);
            player.controller2D.StopBody();   
        }

        public override void Execute()
        {
            //
        }

        public override void Exit()
        {
            player.controller2D.StartBody();
        }
    }
    
}
