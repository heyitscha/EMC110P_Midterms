using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    //***VARIABLES****//
    //Public = can be seen on the inspector section
    public Color lineColor; //Inspector section: this lets you choose the color of the line for the gizmos

    //Private = cannot be seen on the inspector section
    private List<Transform> nodes = new List<Transform>();

    void OnDrawGizmos() 
    {
        //Gizmos to have our own lineColor
        Gizmos.color = lineColor; 

        //To find the type of transform
        //Contains the errors of transform
        Transform[] pathTransforms = GetComponentsInChildren<Transform>(); 
        nodes = new List<Transform>(); //To make sure nodes list is empty at the beginning

        for (int i = 0; i < pathTransforms.Length; i++) // index is incremented
        {
            if (pathTransforms[i] != transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            //Current position of the node
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero ;

            if (i > 0) //Check if index is greater than zero
            {
                //Previous position of a node
                previousNode = nodes[i - 1].position;
            } else if (i == 0 && nodes.Count > 1) //i = 0 or nodes is greater than 1
            {
                previousNode = nodes[nodes.Count - 1].position; //Last node of the array
            }

            Gizmos.DrawLine(previousNode, currentNode); //Line shows when parent object is selected
            Gizmos.DrawWireSphere(currentNode, 0.3f); //Spheres per nodes
        }
    }
}
