using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceLaps : MonoBehaviour
{
    //**VARIABLES***//
    //Public = can be seen on the inspection section on Unity
    public int numLaps;
    public int currentLap;
    public int currentCPoint;
    public int numCPoint;
    public GameObject GameOverUI;
    public static bool disabled = false;

    // Start is called before the first frame update
    private void Start()
    {
        numCPoint = GameObject.Find("Checkpoints").transform.childCount;
        currentCPoint = 1;
        numLaps = 1;
        currentLap = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentCPoint < numCPoint)
        {
            currentLap++;
            currentCPoint = 1;
        }

        if (currentLap > numLaps) //This identifies whether the laps are finished
        {
            GameOverUI.SetActive(true); //If player has finish the lap, Game Over sets true
        } else
        {
            GameOverUI.SetActive(false);
        }
    }

    private void GameTrigger (Collider checkCollider)
    {
        if (checkCollider.name == currentCPoint.ToString())
        {
            currentCPoint++;
        }
    }
}
