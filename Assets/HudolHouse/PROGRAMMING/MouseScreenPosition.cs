using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScreenPosition : MonoBehaviour
{
    public static Vector3 AimTarget;
    public LayerMask aimTargetLayers;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.None;
            GetAimTarget();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
        transform.position = Input.mousePosition;
    }
    void GetAimTarget()
    {
        
        Ray screencast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(screencast, out hit, 40, aimTargetLayers))
        {
            AimTarget = hit.point;
        }
        else
        {
            AimTarget = screencast.origin + screencast.direction * 40f;//Camera.main.transform.position + Camera.main.transform.forward * 40f;
        }
    }
}
