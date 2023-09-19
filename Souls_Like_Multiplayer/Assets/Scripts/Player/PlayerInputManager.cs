using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UB
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        PlayerControls playerControls;

        [Header("Scripts")]
        [HideInInspector] public PlayerManager playerManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

        [Header("Movement Inputs")]
        Vector2 movementInput;
        [SerializeField] float horizontalInput;
        [HideInInspector] public float HorizontalInput => horizontalInput;
        [SerializeField] float verticalInput;
        [HideInInspector] public float VerticalInput => verticalInput;
        public float moveAmount;

        [Header("Camera Input")]
        Vector2 cameraMovementInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;


        [Header("Action Inputs")]
        bool sprintInput = false;
        bool jumpInput = false;
        bool dodgeInput = false;
        bool interactInput = false;
        bool leftClickInput = false;
        bool rightClickInput = false;

        [Header("UI Actions")]
        bool toggleInventoryInput = false;
        bool toggleEquipmentInput = false;     

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
               Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;

            if(playerControls != null)
            {
                playerControls.Disable();
            }           
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //loading into world scene and enabling our player controls
            if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if(playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            //unload controls if in menu scene etc
            else
            {
                instance.enabled = false;

                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        public void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                //movement
                playerControls.PlayerMovement.Movement.performed += inputAction => movementInput = inputAction.ReadValue<Vector2>();               
                playerControls.PlayerCamera.Movement.performed += inputAction => cameraMovementInput = inputAction.ReadValue<Vector2>();
                //playerControls.PlayerMovement.Movement.performed += inputAction => moveInput = true;

                //actions
                playerControls.PlayerActions.Sprint.performed += inputAction => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += inputAction => sprintInput = false;
                playerControls.PlayerActions.Interact.performed += inputAction => interactInput = true;
                playerControls.PlayerActions.Jump.performed += inputAction => jumpInput = true;
                playerControls.PlayerActions.Dodge.performed += inputAction => dodgeInput = true;
                playerControls.PlayerActions.LeftClick.performed += inputAction => leftClickInput = true;
                playerControls.PlayerActions.RightClick.performed += inputAction => rightClickInput = true;

                //ui actions
                playerControls.PlayerActions.ToggleInventory.performed += inputAction => toggleInventoryInput = true;
                playerControls.PlayerActions.ToggleEquipment.performed += inputAction => toggleEquipmentInput = true;
            }
            
            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;            
        }

        private void OnApplicationFocus(bool focus)     //can only control one window at time
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            if (playerManager.isInteracting)
                return;

            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            //movement
            HandlePlayerMovementInput();
            HandleCameraMovementInput();

            //actions
            HandleSprintInput();
            HandleJumpInput();
            HandleInteractInput();
            HandleDodgeInput();
            HandleLeftClickInput();
            HandleRightClickInput();

            //ui actions
            HandleToggleInventoryInput();
            HandleToggleEquipmentInput();
        }

        //movement
        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            //returns absolute number (always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + (Mathf.Abs(horizontalInput)));

            if(moveAmount <= 0.5f && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            if (playerManager == null)
                return;

            //animation
            //pass zero because not locked on
            playerManager.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount, playerManager.playerNetworkManager.isSprinting.Value);
        }

        private void HandleCameraMovementInput()
        {
            if (playerManager.uiFlag)
                return;

            cameraVerticalInput = cameraMovementInput.y;
            cameraHorizontalInput = cameraMovementInput.x;
        }

        //actions
        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                //handle sprinting
                playerLocomotionManager.HandleSprinting();
            }
            else
            {
                playerManager.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                //if ui return

                playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleInteractInput()
        {
            if (interactInput)
            {
                interactInput = false;

                if (playerManager.interactableObject != null)
                {
                    playerManager.InteractWithInteractable();
                }
                playerManager.CheckForInteractable();
            }
        }

        private void HandleLeftClickInput()
        {
            if (leftClickInput)
            {
                leftClickInput = false;

                //if ui open return

                playerManager.playerNetworkManager.SetCharacterActionHand(false);

                //if we are two handing use the two handed action?

                playerManager.playerCombatManager.PerformWeaponBasedAction(playerManager.playerInventoryManager.currentLeftHandWeapon.oh_Left_Click_Action, playerManager.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        private void HandleRightClickInput()
        {
            if (rightClickInput)
            {
                rightClickInput = false;

                //if ui open return

                playerManager.playerNetworkManager.SetCharacterActionHand(true);

                // if we are two handing use the two handed action?

                playerManager.playerCombatManager.PerformWeaponBasedAction(playerManager.playerInventoryManager.currentRightHandWeapon.oh_rightClick_Action, playerManager.playerInventoryManager.currentRightHandWeapon);
            }
        }

        //ui 
        private void HandleToggleInventoryInput()
        {
            if (toggleInventoryInput)
            {
                toggleInventoryInput = false;

                UIManager.instance.ToggleInventory();
            }      
        }

        private void HandleToggleEquipmentInput()
        {
            if (toggleEquipmentInput)
            {
                toggleEquipmentInput = false;

                UIManager.instance.ToggleEquipment();
            }
        }


    }
}

