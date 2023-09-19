using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace UB
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] bool respawnCharacter = false;

        [Header("Components")]
        //scripts
        CharacterStats charStats;
        [HideInInspector] public PlayerAnimationManager playerAnimationManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        

        //Roaming
        [Header("Player Roaming Scripts")]
        PlayerInputManager inputHandler;
        PlayerLocomotionManager playerLocomotionManager;

        [Header("Movement")]
        [SerializeField] LayerMask movementMask;

        [Header("Interactable")]
        [SerializeField] float interactRange = 2f;
        [SerializeField] LayerMask interactableLayers;
        [HideInInspector] public Interactable interactableObject = null;

        //values
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Interacting")]
        public bool isInteracting = false;

        public bool uiFlag = false;


        protected override void Awake()
        {
            DontDestroyOnLoad(this);

            base.Awake();

            //scripts
            charStats = GetComponent<CharacterStats>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();

        }

        protected override void Start()
        {
            base.Start();

            //scripts
            inputHandler = PlayerInputManager.instance;
        }

        protected override void Update()
        {
            base.Update();

            //only control this gameobject if we own it
            if (!IsOwner)
                return;

            //handle movement
            playerLocomotionManager.HandleAllMovement();

            //regen stamina
            playerStatsManager.RegenerateStamina();

            DebugMenu();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;

            base.LateUpdate();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            //if this player obj is local client player
            if (IsOwner)
            {
                PlayerInputManager.instance.playerManager = this;
                PlayerInputManager.instance.playerLocomotionManager = playerLocomotionManager;
                CameraController.instance.playerManager = this;
                WorldSaveGameManager.instance.playerManager = this;

                //update the total health or stamina when stat linked to either changes
                playerNetworkManager.strength.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.agility.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

                //updates resource bars when stat changes
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                //PlayerInteractUI.instance.playerManager = this;
                // UIManager.instance.playerManager = this;
            }

            //putting outside of if isOwner because we want to do some stuff for everyone

            //STATS
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

            //EQUIPMENT
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

            // Upon connecting, if client but not owner of server, reload character data to newly instantiated char
            // we dont run this if we are server as we already have ours loaded
            if(IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
            }

            // check for players that are alive, if 0 respawn characters

            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            if (IsOwner)
            {
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

                //player respawn fx
                playerAnimationManager.PlayTargetActionAnimation("Empty", false);
            }
        }

        #region Save and Load
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            //current scene
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            //name
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            //position
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            //stats
            currentCharacterData.strength = playerNetworkManager.strength.Value;
            currentCharacterData.agility = playerNetworkManager.agility.Value;

            //resources
            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            //name
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;

            //position
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;

            //stats
            playerNetworkManager.strength.Value = currentCharacterData.strength;
            playerNetworkManager.agility.Value = currentCharacterData.agility;

            //resources
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnStrength(playerNetworkManager.strength.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnAgility(playerNetworkManager.agility.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        }

        #endregion

        #region Roaming Functions
        //interaction
        public void CheckForInteractable()
        {
            List<Interactable> interactableList = new List<Interactable>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliders)
            {
                if(collider.TryGetComponent(out Interactable interactable))
                {
                    interactableList.Add(interactable);
                }
            }

            Interactable nearestInteractable = null;
            if (interactableList.Count > 0)
            {
                foreach (Interactable interactable in interactableList)
                {
                    if (nearestInteractable == null)
                    {
                        nearestInteractable = interactable;
                    }
                    else if ((interactable.transform.position - transform.position).sqrMagnitude < (nearestInteractable.transform.position - transform.position).sqrMagnitude)
                    {
                        nearestInteractable = interactable;
                    }
                }

                FaceTarget(nearestInteractable.gameObject.transform);
                isInteracting = true;
                MouseControls.instance.ShowCursor();
                nearestInteractable.Interact(transform);
            }
     
        }

        public Interactable GetInteractableObject()
        {
            List<Interactable> interactableList = new List<Interactable>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Interactable interactable))
                {
                    interactableList.Add(interactable);
                }
            }

            Interactable nearestInteractable = null;
            if (interactableList.Count > 0)
            {
                foreach (Interactable interactable in interactableList)
                {
                    if (nearestInteractable == null)
                    {
                        nearestInteractable = interactable;
                    }
                    else if((interactable.transform.position - transform.position).sqrMagnitude < (nearestInteractable.transform.position - transform.position).sqrMagnitude)
                    {
                        nearestInteractable = interactable;
                    }
                }               
            }

            return nearestInteractable;
        }

        public void InteractWithInteractable()
        {
            interactableObject.Interact();
        }

        void FaceTarget(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = lookRotation; //Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        #endregion
        
        //debug, delete later
        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }
        }

    }
}

