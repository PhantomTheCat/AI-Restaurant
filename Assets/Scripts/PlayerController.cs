using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Player
{
    /// <summary>
    /// Controls the player character's movement and camera using input from the player.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(FootstepController))]
    public class PlayerController : MonoBehaviour
    {
        //
        //Properties
        //
        [Header("Movement Stats")]
        [SerializeField] private float playerSpeed = 6f;
        [SerializeField] private float gravityMagnitude = -9.8f;
        [SerializeField] private float sprintModifier = 1.5f;

        [Header("Sensitivity")]
        [SerializeField] private float sensitivityHorizontal = 30f;
        [SerializeField] private float sensitivityVertical = 30f;

        [Header("View Constraints")]
        [SerializeField] private float minimumVert = -45.0f;
        [SerializeField] private float maximumVert = 45.0f;

        [Header("Input")]
        [SerializeField] private float verticalRotation = 0;
        [SerializeField] private float horizontalRotation = 0;

        [Header("Camera")]
        [SerializeField] private Camera playerCamera;

        //Input Actions for the new input system
        private InputAction zAxisMove;
        private InputAction xAxisMove;
        private InputAction sprint;
        private InputAction mouseLook;
        private PlayerControls controlMappings;

        //Components
        private CharacterController characterController;
        private FootstepController footstepController;

        //Tracking Variables
        private bool isSprinting = false;
        private bool cameraLocked = false;
        private Vector2 mouseDelta;


        //
        // Methods
        //
        protected void Awake()
        {
            //Component initialization
            controlMappings = new PlayerControls();
            characterController = GetComponent<CharacterController>();
            footstepController = GetComponent<FootstepController>();

            //Setting up the input actions
            zAxisMove = controlMappings.Player.ForwardAndBack;
            xAxisMove = controlMappings.Player.LeftAndRight;
            sprint = controlMappings.Player.Sprint;
            mouseLook = controlMappings.Player.Look;

            //Locking the cursor to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected void OnEnable()
        {
            //Enabling the controls for play
            zAxisMove.Enable();
            xAxisMove.Enable();
            sprint.Enable();
            mouseLook.Enable();
        }

        protected void OnDisable()
        {
            //Disabling the controls when this component is (Stops looking for them)
            zAxisMove.Disable();
            xAxisMove.Disable();
            sprint.Disable();
            mouseLook.Disable();
        }

        protected void FixedUpdate()
        {
            //Handling movement
            HandleMovement();
        }

        protected void Update()
        {
            //Controlling rotation for player and camera
            if (!cameraLocked)
            {
                //Getting the mouse delta from the input system
                mouseDelta = mouseLook.ReadValue<Vector2>();

                //(Player focuses on horizontal rotation because
                //they can't rotate up or otherwise will go upwards)
                PlayerMouseRotation();

                //Camera focuses on vertical rotation as player
                //can't be rotated up (restriction)
                CameraMouseRotation();
            }
        }

        /// <summary>
        /// Controlling the movement of the player character.
        /// </summary>
        protected void HandleMovement()
        {
            //Getting their input as floats
            float xAxisInput = xAxisMove.ReadValue<float>() * playerSpeed;
            float zAxisInput = zAxisMove.ReadValue<float>() * playerSpeed;

            //Seeing if we are moving
            if (xAxisInput != 0 || zAxisInput != 0)
            {
                //Seeing if sprinting or not
                if (sprint.ReadValue<float>() > 0)
                {
                    xAxisInput *= sprintModifier;
                    zAxisInput *= sprintModifier;
                    isSprinting = true;
                }

                //Applying the footstep sound if movement is above 0
                footstepController.ChangeMovingState(true, isSprinting);

                //Translating input into Vector3/axis along the x and z axis respectfully
                Vector3 movement = new Vector3(xAxisInput, 0, zAxisInput);

                //Applying gravity
                movement.y = gravityMagnitude;

                //Moving character
                movement *= Time.deltaTime;
                movement = transform.TransformDirection(movement);
                characterController.Move(movement);

            }
            else
            {
                //Else stop the footstep sounds
                footstepController.ChangeMovingState(false, isSprinting);
            }

            //Setting the sprinting bool to default
            isSprinting = false;
        }

        /// <summary>
        /// Controlling vertical rotation on the camera
        /// </summary>
        private void CameraMouseRotation()
        {
            //Getting the vertical rotation (Used for camera)
            verticalRotation -= (mouseDelta.y * sensitivityVertical) * Time.deltaTime;
            verticalRotation = Mathf.Clamp(verticalRotation, minimumVert, maximumVert);

            //Transforming camera rotation
            playerCamera.transform.localEulerAngles = new Vector3(verticalRotation, 0, 0);
        }

        /// <summary>
        /// Controlling horizontal rotation of player on the player.
        /// </summary>
        private void PlayerMouseRotation()
        {
            //Getting the horizontal rotation (Used for player)
            float delta = (mouseDelta.x * sensitivityHorizontal) * Time.deltaTime;
            horizontalRotation = transform.localEulerAngles.y + delta;

            //Transforming player rotation
            this.transform.localEulerAngles = new Vector3(0, horizontalRotation, 0);
        }

        /// <summary>
        /// Changing the lock of the camera movement to the other state
        /// </summary>
        public void ChangeLockCameraMovement()
        {
            //Putting the camera lock on the
            //opposite setting of what it was
            if (cameraLocked)
            {
                cameraLocked = false;
            }
            else
            {
                cameraLocked = true;
            }
        }
    }
}
