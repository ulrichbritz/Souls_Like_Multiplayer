using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

namespace UB
{
    public class CharacterManager : NetworkBehaviour
    {        
        [Header("Scrips")]
        [HideInInspector] public CharacterStats characterStats;
        [HideInInspector] public CharacterAnimationManager animationManager;
        [HideInInspector] private EquipmentManager equipmentManager;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        public CharacterStats CharacterStats => characterStats;
        PlayerInventory playerInventory;
        TextBoard textBoard;


        [Header("Components")]
        [HideInInspector]
        public GameObject hitPoint;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterController characterController;

        [Header("Movement")]
        [HideInInspector] public Vector3 moveTarget;

        [Header("Stats")]
        public int Initiative;
        public List<GameObject> d20;

        [Header("Actions")]
        [HideInInspector]
        public List<CharacterManager> allTargets = new List<CharacterManager>();
        public List<CharacterManager> primaryAttackTargets = new List<CharacterManager>();
        public List<CharacterManager> secondaryAttackTargets = new List<CharacterManager>();
        [HideInInspector]
        public int currentTarget;

        [Header("Flags")]
        [HideInInspector] public bool isPerformingAction;
        [HideInInspector] public bool canRotate = true;
        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool applyRootMotion = false;
        [HideInInspector] public bool isSprinting = false;
        [HideInInspector] public bool isGrounded = false;

        protected virtual void Awake()
        {
            //components
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            
            //scripts
            characterStats = GetComponent<CharacterStats>();
            animationManager = GetComponent<CharacterAnimationManager>();
            playerInventory = GetComponent<PlayerInventory>();
            equipmentManager = GetComponent<EquipmentManager>();
            textBoard = TextBoard.instance;
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);

            //if this char is controlled by us, assign its network pos to its pos
            if (IsOwner)
            {
                //movement
                characterNetworkManager.networkPosition.Value = transform.position;
                //rotation
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            //if not, use their network transform
            else
            {               
                // Smoothly interpolate position
                transform.position = Vector3.SmoothDamp(transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);
                

                // Interpolate rotation
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {

        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                characterNetworkManager.isDead.Value = true;

                //reset any flags that need to be reset

                //if not grounded play arial death anim

                if (!manuallySelectDeathAnimation)
                {
                    animationManager.PlayTargetActionAnimation("Dead_01", true, true);
                }
            }

            //play death sfx

            yield return new WaitForSeconds(5f);

            //award players with xp

            //disable character
        }

        public virtual void ReviveCharacter()
        {

        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

            List<Collider> ignoreColliders = new List<Collider>();

            //add all damageable character colliders to list that will be used to ignore collisions
            foreach(var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            //add our character controller collider to list to ignore collisions
            ignoreColliders.Add(characterControllerCollider);

            //go through every collider in list to ignore collision with eachother
            foreach(var collider in ignoreColliders)
            {
                foreach(var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }

        public int GetOverallArmorCount()
        {
            int armorCount = 0;

            foreach(Equipment equipment in equipmentManager.CurrentEquipment)
            {
                if(equipment != null)
                    armorCount += equipment.armorModifier;
            }

            return armorCount;
        }

        public int GetOverallStrengthModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.strengthModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatStrengthModifier() + equipmentMod);
        }

        public void GetOverAllInitiativeModifier()
        {

        }

        public int GetOverallIntelligenceModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.intelligenceModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatIntelligenceModifier() + equipmentMod);
        }

        public int GetOverallAgilityModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.agilityModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatAgilityModifier() + equipmentMod);
        }

        public int GetOverallCharismaModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.charismaModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatCharismaModifier() + equipmentMod);
        }

        public int GetOverallWillpowerModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.willpowerModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatWillpowerModifier() + equipmentMod);
        }

    }
}
