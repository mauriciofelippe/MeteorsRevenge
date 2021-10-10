using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private CharacterController2D controller2D;
        [SerializeField] private Animator animator;
        
        private float _moveX;
        private const float Speed = 40f;
        private bool _jump = false;
        private static readonly int Walk = Animator.StringToHash("walk");

        private void Start()
        {
            if(controller2D == null) controller2D = GetComponent<CharacterController2D>();
        }

        private void Update()
        {
            _moveX = Input.GetAxisRaw("Horizontal") * Speed;
            if (Input.GetButtonDown("Jump")) _jump = true;
            animator.SetBool(Walk,Mathf.Abs(_moveX)>0);
            
        }

        private void FixedUpdate()
        {
            controller2D.Move(_moveX * Time.fixedDeltaTime,false,_jump);
            _jump = false;
        }
    }
}
