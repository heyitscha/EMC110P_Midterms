using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //***VARIABLES****//
    //Public = can be seen on the inspection section on Unity
    public WheelCollider[] Wheels = new WheelCollider[4]; //Shows there are four wheels
    public GameObject[] WheelMesh = new GameObject[4];
    public float MaximumSteer = 30;
    public float CarRadius = 10;
    public float DownForceValue = 100;
    public float CarBrake = 5000;
    public int motorTorque = 200;

    //Private = cannot be seen on the inspection section on Unity
    private Vector3 centerOfMass;
    private Rigidbody rb;
    private Inputs inputManager;

    // Start is called before the first frame update
    void Start()
    {
        getObjects();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //***FUNCTIONS***//
        DriveControl();
        SteerControl();
        CarDownforce();
        CarAnimate();
    }

    //***METHODS***//
    //Controls for the Car
    private void DriveControl()
    {
        for (int i = 0; i < Wheels.Length; i++)
        {
            Wheels[i].motorTorque = inputManager.vertical * motorTorque; 
        }

        if (inputManager.handbrake)
        {
            Wheels[3].brakeTorque = Wheels[2].brakeTorque = CarBrake;
        }
        else
        {
            Wheels[3].brakeTorque = Wheels[2].brakeTorque = 0;
        }
    }
    
    //Steering control
    private void SteerControl()
    {
        if (inputManager.horizontal > 0)
        {
            Wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (CarRadius + (1.5f / 3))) * inputManager.horizontal; //Note: Mathf is a mathematical function helper
            Wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (CarRadius - (1.5f / 3))) * inputManager.horizontal; 
        } else if (inputManager.horizontal < 0)
        {
            Wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (CarRadius - (1.5f / 3))) * inputManager.horizontal;
            Wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (CarRadius + (1.5f / 3))) * inputManager.horizontal;
        } else
        {
            Wheels[0].steerAngle = 0;
            Wheels[1].steerAngle = 0;
        }
    }
    //Force that holds the object to the ground
    private void CarDownforce()
    {
        rb.AddForce(-transform.up * DownForceValue * rb.velocity.magnitude);
    }

    //WheelsAnimation
    private void CarAnimate()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            Wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            WheelMesh[i].transform.position = wheelPosition;
            WheelMesh[i].transform.rotation = wheelRotation;
        }
    }

    private void getObjects()
    {
        inputManager = GetComponent<Inputs>();
    }
}
