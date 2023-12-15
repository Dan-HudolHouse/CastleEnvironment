using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace HudolHouse
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        public CinemachineFreeLook camera1, camera2;

        public static Vector3 AimTarget;
        public LayerMask aimTargetLayers;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                camera1.Priority = 10;
                camera2.Priority = 20;
                
            }
            if (Input.GetMouseButtonUp(1))
            {
                camera1.Priority = 20;
                camera2.Priority = 10;
            }
            //GetAimTarget();
        }
        void GetAimTarget()
        {
            Vector3 screencenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray screencast = Camera.main.ScreenPointToRay(screencenter);
            RaycastHit hit;
            if (Physics.Raycast(screencast, out hit, 40, aimTargetLayers))
            {
                AimTarget = hit.point;
            }
            else
            {
                AimTarget = Camera.main.transform.position + Camera.main.transform.forward * 40f;
            }
        }
    }
}
