using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    //introduce your global variable
    public Transform target;
    public Transform swivlebase;
    public Transform head;
    public Transform launchPoint;
    public GameObject bullet;
    bool spotted;
    [SerializeField] float rotSpeed, shootDelay;
    // Start is called before the first frame update
    void Start()
    {
        //store objects and values in your global variable
    }

    // Update is called once per frame
    void Update()
    {
        if (spotted)
        {
            // Determine which direction to rotate towards
            Vector3 swivleTarget = new Vector3(target.position.x, swivlebase.transform.position.y, target.position.z) - transform.position;
            Vector3 headTarget = target.position - head.transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = rotSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 swivleDirection = Vector3.RotateTowards(swivlebase.transform.forward, swivleTarget, singleStep, 0.0f);
            Vector3 headDirection = Vector3.RotateTowards(head.transform.forward, headTarget, singleStep, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            swivlebase.transform.rotation = Quaternion.LookRotation(swivleDirection);
            head.transform.rotation = Quaternion.LookRotation(headDirection);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        spotted = true;
        InvokeRepeating("Shoot", shootDelay, shootDelay);
    }
    private void OnTriggerExit(Collider other)
    {
        spotted = false;
        CancelInvoke("Shoot");
    }
    void Shoot()
    {
        Instantiate(bullet, launchPoint.position, launchPoint.rotation);
        if (Vector3.Distance(transform.position, target.transform.position) > 10)
        {
            spotted = false;
            CancelInvoke("Shoot");
        }
    }
}
