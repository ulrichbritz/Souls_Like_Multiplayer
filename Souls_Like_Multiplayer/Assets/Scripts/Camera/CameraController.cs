using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        [Header("Components")]
        //scripts
        public PlayerManager playerManager;

        //components
        public Transform cameraSystemObj;
        public Transform cameraPivotObj;
        public Transform camObj;

        [Header("Roaming Camera Settings")]
        //roaming
        [SerializeField] private float cameraSmoothSpeed = 1;
        [SerializeField] private float upAndDownRotationSpeed = 220f;
        [SerializeField] private float leftAndRightRotationSpeed = 220f;
        [SerializeField] private float minPivot = -30f; //lowest able to look down
        [SerializeField] private float maxPivot = 60f; //highest able to look up

        [SerializeField] Vector3 roamingCameraSystemTransform;
        [SerializeField] Vector3 roamingCameraPivotTransform;
        [SerializeField] Vector3 roamingCamTransform;

        [SerializeField] float cameraCollisionRaduis = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Roaming Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; //used for collision, moves cam to this pos
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPos;
        private float targetCameraZPos;


        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {

        }

        private void LateUpdate()
        {
            if (playerManager != null)
            {
                HandleRoamingFollowPlayer();
                HandleRoamingRotation();
            }

            HandleCameraCollisions();
           
        }


        private void HandleCameraCollisions()
        {
            targetCameraZPos = cameraZPos;

            RaycastHit hit;
            Vector3 direction = camObj.transform.position - cameraPivotObj.transform.position;
            direction.Normalize();

            //check if object infront of cam
            if (Physics.SphereCast(cameraPivotObj.position, cameraCollisionRaduis, direction, out hit, Mathf.Abs(targetCameraZPos), 0))
            {
                //if there is , get distance from it
                float distanceFromHitObj = Vector3.Distance(cameraPivotObj.position, hit.point);
                targetCameraZPos = -(distanceFromHitObj - cameraCollisionRaduis);
            }

            //if target pos is less that collision raduis, subtract our collision raduis making it snap back
            if(Mathf.Abs(targetCameraZPos) < cameraCollisionRaduis)
            {
                targetCameraZPos = -cameraCollisionRaduis;
            }

            //apply final pos using lerp over time
            cameraObjectPosition.z = Mathf.Lerp(camObj.transform.localPosition.z, targetCameraZPos, 0.2f);
            camObj.transform.localPosition = cameraObjectPosition;
        }

        private void HandleRoamingFollowPlayer()
        {
            Vector3 targetCameraPos = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPos;
        }

        private void HandleRoamingRotation()
        {
            //locked on rotations

            //normal rotations
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minPivot, maxPivot);


            Vector3 cameraRotation;
            Quaternion targetRotation;

            //rotate this gameobject (cameraSystem) left and right
            cameraRotation = Vector3.zero;
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //rotate the pivot gameobject up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotObj.localRotation = targetRotation;
        }


    }


}
