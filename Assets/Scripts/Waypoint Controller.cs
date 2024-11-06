using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public float laps;
    public float currentWaypoint;
    public float currentLap;
    public bool started;
    public bool finished;


    public List<GameObject> waypoints;
    public GameObject startEnd;

    public GameManager gameManager;

    public TextMeshProUGUI waypointText;


    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("The start method of the waypoint controller has been started");
        //initiate the game manager script
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        waypointText = GameObject.Find("Waypoint Message").GetComponent<TextMeshProUGUI>();



        currentWaypoint = 0;
        currentLap = 1;

        started = false;
        finished = false;
        startEnd = GameObject.FindGameObjectWithTag("WayPoint");
        gameManager.InitiateWapoints(waypoints);
    }

    private void Update()
    {
        gameManager.DisplayTimer();

    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WayPoint"))
        {

            GameObject thisWaypoint = other.gameObject;

            //started the race
            if (thisWaypoint == startEnd && !started)
            {
                //if we are touching the start line and haven't started the game, start it.
                started = true;
                Debug.Log("Started");
            }
            //ending the lap or race
            else if (thisWaypoint == startEnd && started)
            {
                if (currentLap == laps)
                {
                    //then check if all the checkpoints have been gone through
                    if (currentWaypoint == waypoints.Count)
                    {
                        //if the simulation is finished, switch scenes and stop timer, make instance of the game manager script aswell.
                        //if the current waypoint is equal to the length of the waypoints array, must be at the finish
                        StartCoroutine(gameManager.DisplayMessage(waypointText, "Game Finished!", 3.0f));
                        //Should trigger the game manager to store the timer, and switch scenes.
                        finished = true;
                        started = false;
                        gameManager.GameEnd();
                    }
                    else
                    {
                        StartCoroutine(gameManager.DisplayMessage(waypointText, "Not all waypoints have been passed!", 2.0f));
                        //output the text on the GUI for not all waypoints complete
                    }
                }
                //if all laps are not finished, start a new lap
                else if (currentLap < laps)
                {
                    if (currentWaypoint == waypoints.Count)
                    {
                        currentLap++;
                        currentWaypoint = 0;
                        StartCoroutine(gameManager.DisplayMessage(waypointText, "Started Lap " + currentLap, 2.0f));
                        //print the 'started lap x on the GUI'
                    }
                }
                else
                {
                    StartCoroutine(gameManager.DisplayMessage(waypointText, "Not all waypoints have been passed!", 2.0f));
                    //Add GUI message for not going through all the waypoints
                }
            }

            //loop through the list of waypoints list to see which one the player passed through
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (finished)
                {
                    return;
                }


                if (thisWaypoint == waypoints[i] && i == currentWaypoint)
                {
                    StartCoroutine(gameManager.DisplayMessage(waypointText, "Correct waypoint!", 0.5f));
                    Debug.Log("Correct Waypoint!");
                    currentWaypoint++;
                }
                else if (thisWaypoint == waypoints[i] && i != currentWaypoint)
                {
                    StartCoroutine(gameManager.DisplayMessage(waypointText, "Incorrect waypoint!", 1.0f));
                    Debug.Log("incorrect Waypoint!");
                    //output on the GUI that this was the wrong checkpoint
                }
            }
        }
    }
}
