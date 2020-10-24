using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    //***VARIABLES****//
    //public = can be seen on the inspector section on unity
    public float maxSteerAngle = 50f;
    public Transform path; //This checks the nodes of Path

    [Header("Wheel Colliders")]
    public WheelCollider WheelFrontLeft;
    public WheelCollider WheelFrontRight;
    public WheelCollider WheelRearLeft;
    public WheelCollider WheelRearRight;
   

    [Header("Ai Engine")]
    public float currentSpeed;
    public float maxSpeed;
    public float maxMotorTorque = 50f;
    public float DownforceValue = 100f;
    public float brakingTorque = 8000f;
    public bool isBraking = false;

    //private = cannot be seen on the inspector section on unity

    private List<Transform> nodes;
    private int currentNode = 0;
    private Rigidbody rb;
    private Vector3 centerOfMass;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;

        //This is taken from the Path scripts to get the nodes path
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //***FUNCTIONS***//
        AiDrive();
        AiSteering();
        AiPointDistance();
        AiBrake();
        AiDownforce();
    }

    //***METHODS***//
    //Calculation and control for Ai's speed 
    private void AiDrive()
    {
        currentSpeed = 2 * Mathf.PI * WheelFrontLeft.radius * WheelFrontLeft.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            WheelFrontLeft.motorTorque = maxMotorTorque;
            WheelFrontRight.motorTorque = maxMotorTorque;
        }
        else
        {
            WheelFrontLeft.motorTorque = 0;
            WheelFrontRight.motorTorque = 0;
        }
    }
    //Ai Car Steering Controls
    private void AiSteering()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        WheelFrontLeft.steerAngle = newSteer;
        WheelFrontRight.steerAngle = newSteer;
    }

    //Waypoint distance of nodes from Path
    private void AiPointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 5f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            } else
            {
                currentNode++;
            }
        }
    }

    //Breaks on the back wheels
    private void AiBrake()
    {
        if (isBraking) //The Ai Vehicle brakes
        {
            WheelRearLeft.brakeTorque = brakingTorque;
            WheelRearRight.brakeTorque = brakingTorque;
        }
        else //The Ai Vehicle continues to run
        {
            WheelRearLeft.brakeTorque = 0;
            WheelRearRight.brakeTorque = 0;
        }
    }

    //The force that holds the car down to the ground
    private void AiDownforce()
    {
        rb.AddForce(-transform.up * DownforceValue * rb.velocity.magnitude);
    }
}
