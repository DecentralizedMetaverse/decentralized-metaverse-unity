using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TC.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Controller : MonoBehaviour
    {
        public event Action UpdateHandler;

        [SerializeField] Animator anim;
        [SerializeField] float speedMove = 10f;
        [SerializeField] float multiplyDash = 3.0f;
        [SerializeField] float jumpPower = 10f;

        Rigidbody rb;
        int layerMask = 0;
        const float sizeCollider = 0.125f;
        Transform mainCamera;

        readonly int animSpeed = Animator.StringToHash("moveSpeed");

        protected void Awake()
        {
            layerMask = LayerMask.GetMask("Default");
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (mainCamera == null)
            {
                mainCamera = UnityEngine.Camera.main.transform;
            }
        }

        public void FixedUpdate()
        {
            //// �ړ�
            //data.direction.Normalize();
            //Move(data.direction * Runner.DeltaTime, data.dash);

            //// �W�����v
            //if (data.jump) Jump();

            //UpdateHandler?.Invoke();
        }

        void Move(Vector2 moveVector, bool dash)
        {
            if (dash) moveVector *= multiplyDash;

            var force = speedMove - rb.velocity.magnitude;
            var move = CalcMovementFromCamera(moveVector);

            if (moveVector != Vector2.zero)
            {
                // ���͂�����ꍇ
                ChangeAngleFromInput(move); // ������ς���
            }

            move *= force;
            rb.AddForce(move);
            SetAnimMoveSpeed(dash, move);
        }

        /// <summary>
        /// Animation��moveSpeed�̒l��ύX����
        /// TODO: �֐�����������Ƃ������O�ɕς���
        /// </summary>
        /// <param name="dash"></param>
        /// <param name="move"></param>
        private void SetAnimMoveSpeed(bool dash, Vector3 move)
        {
            if (move == Vector3.zero)
            {
                anim.SetFloat(animSpeed, 0);
            }
            else if (!dash)
            {
                anim.SetFloat(animSpeed, 0.6666f);
            }
            else
            {
                anim.SetFloat(animSpeed, 1.0f);
            }
        }

        void Jump()
        {
            if (!Physics.CheckSphere(transform.position, sizeCollider, layerMask)) return;
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        }

        // -----------------------------------------------------------

        /// <summary>
        /// �J�����̊p�x����ړ��x�N�g�����v�Z
        /// </summary>
        Vector3 CalcMovementFromCamera(Vector2 moveInput)
        {
            if (mainCamera == null) return Vector3.zero;

            var forward = mainCamera.forward;
            forward.Normalize();
            forward.y = 0;
            return (mainCamera.right * moveInput.x) + (forward * moveInput.y);
        }

        /// <summary>
        /// �ړ��x�N�g�������Ƃɂ��āA�L�����N�^�[�̌�����ς���
        /// Change the character's orientation
        /// </summary>
        /// <param name="angle"></param>
        public void ChangeAngleFromInput(Vector3 angle, float speed = 10f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(angle), Time.deltaTime * speed);
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, sizeCollider);
        }
#endif
    }
}