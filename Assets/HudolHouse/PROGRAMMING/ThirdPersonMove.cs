using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace HudolHouse
{
    public class ThirdPersonMove : MonoBehaviour
    {
        //Components
        private CharacterController controller;
        private Animator anim;

        //movement variables
        private Vector3 playerVelocity;

        private bool groundedPlayer;
        private bool isJumping;
        public float turnSpeed;
        private float jumpHeight = 1.0f;
        public float jumpHorizontalSpeed;
        private float gravityValue = -9.81f;

        //Camera variables
        Vector3 camRight;
        Vector3 camForward;

        public float turnWhileAimingThreshold;
        public float turnWhileAimingSpeed;
        public bool shouldTurn;
        private void Start()
        {
            controller = gameObject.GetComponentInParent<CharacterController>();
            anim = GetComponent<Animator>();

        }

        void Update()
        {
            //check for grounding and reset gravity
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            //determine input direction based on camera and inputs.
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            camRight = Camera.main.transform.right;
            camForward = Vector3.Cross(camRight, Vector3.up);



            //check if you are moving
            if (h != 0 || z != 0)
            {
                //gameObject.transform.forward = camForward;
                if (!Input.GetMouseButton(2) && !Input.GetMouseButton(1))
                {
                    RotateToFaceCamera(false);
                }
                
                anim.SetBool("IsMoving", true);
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }
            if (Input.GetMouseButton(1))
            {
                RotateOnMouse();
            }
            Vector3 moveInput = new Vector3(h, 0, z);
            moveInput.Normalize();





            if (!Input.GetKey(KeyCode.LeftShift))
            {
                moveInput *= 0.5f;
            }

            //animate
            anim.SetFloat("Velx", moveInput.x, 0.05f, Time.deltaTime);
            anim.SetFloat("Velz", moveInput.z, 0.05f, Time.deltaTime);

            if (groundedPlayer)
            {
                //anim.SetLayerWeight(1, 0.5f);
                anim.SetBool("Grounded", true);
                anim.SetBool("Jumping", false);
                isJumping = false;
                anim.SetBool("Falling", false);
            }
            else
            {
                anim.SetBool("Grounded", false);
            }
            // ProcessJump
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                //anim.SetLayerWeight(1, 1f);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                isJumping = true;
                anim.SetBool("Jumping", true);
            }
            if (isJumping && playerVelocity.y < 0 || playerVelocity.y < -4)
            {
                anim.SetBool("Falling", true);
                if(Physics.Raycast(transform.position, -transform.up, 1))
                {
                    anim.SetBool("Grounded",  true);
                }
            }
            playerVelocity.y += gravityValue * Time.deltaTime;
            if (isJumping)
            {
                Vector3 velocity = ((controller.transform.forward * z) + (controller.transform.right * h)) * jumpHorizontalSpeed;
                velocity.y = playerVelocity.y;

                controller.Move(velocity * Time.deltaTime);
            }

        }
        private void OnAnimatorMove()
        {
            if (!isJumping)
            {
                Vector3 animatedMovement = anim.deltaPosition;
                animatedMovement.y = playerVelocity.y * Time.deltaTime;
                controller.Move(animatedMovement);
            }

        }
        private void RotateToFaceCamera(bool aiming)
        {
            Quaternion desiredLook;
            if (aiming)
            {
                Vector3 correctedTarget = ThirdPersonCameraController.AimTarget;
                correctedTarget.y = controller.transform.position.y;
                desiredLook = Quaternion.LookRotation(correctedTarget - controller.transform.position);
            }
            else
            {
                desiredLook = Quaternion.LookRotation(camForward);
            }

            Quaternion newRotation = Quaternion.Lerp(controller.transform.rotation, desiredLook, turnSpeed * Time.deltaTime);
            controller.transform.rotation = newRotation;
        }
        private void RotateOnMouse()
        {
            if (Input.mousePosition.x >= Screen.width * 0.8f)
            {
                controller.transform.Rotate(0, 10 * turnWhileAimingSpeed * Time.deltaTime, 0);
                
            }
            else if (Input.mousePosition.x <= Screen.width * 0.2f) 
            {
                controller.transform.Rotate(0, 10 * turnWhileAimingSpeed * Time.deltaTime * -1, 0);
            }
        }
    }
}

