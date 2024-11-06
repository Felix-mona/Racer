using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    // allows this script to be accessed from the end screen, for the lap time
    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get { return instance; }
    }





    //need a collider for the start line of the track, could then in the start() method add in getcomponentinchildren of the track prefab
    public GameObject startLineBox;

    //Game objects that hold the 'chosen' track and car from the menu script
    private GameObject trackPrefab;
    private GameObject carPrefab;
    public CarController carControllerScript;



    //readability of the code, origin vector
    private Vector3 origin = new Vector3(0, 0, 0);

    //scripts to grab other variables from
    public CountdownTimer timerScript;
    private MenuManager menuManagerScript;
    public WaypointController waypointController;




    private void Start()
    {
        //links to the menu manager script to grab the chosen car and track.
        menuManagerScript = MenuManager.Instance;
        //sets the gameObjects to the selected by the user, ready to be instantiated
        trackPrefab = menuManagerScript.chosenTrack;
        carPrefab = menuManagerScript.chosenCar;

        //reference to the car controller script
        carControllerScript = carPrefab.GetComponent<CarController>();

        InstantiateWorld();

        //starting the go timer from the countdown timer script
        StartCoroutine(timerScript.StartCountDown());
    }

    private void Update()
    {

        if (GameObject.FindGameObjectWithTag("Player Car").transform.position.y <= -80)
        {
            //resets the cars position and displays message to the user.
            GameObject.FindGameObjectWithTag("Player Car").transform.position = startLineBox.transform.position;
            GameObject.FindGameObjectWithTag("Player Car").transform.rotation = startLineBox.transform.rotation;
            GameObject.FindGameObjectWithTag("Player Car").GetComponent<Rigidbody>().velocity = Vector3.zero;

            DisplayMessage(waypointController.waypointText, "Car out of bounds! Resetting you at the start line...", 2.0f);
        }
        //error handling, if the player goes below the map, move them back to the start line


    }

    private void InstantiateWorld()
    {
        //instantiate the track prefab at the origin
        Instantiate(trackPrefab, origin, Quaternion.identity);
    }

    public void InstantiatePlayer()
    {
        //link to the start line GameObject for the location on where to instantiate the car
        startLineBox = GameObject.FindGameObjectWithTag("WayPoint");
        //instantiate the car at the same position as the start line
        Instantiate(carPrefab, startLineBox.transform.position, Quaternion.identity);
        waypointController = GameObject.FindGameObjectWithTag("Player Car").GetComponent<WaypointController>();
    }


    public void InitiateWapoints(List<GameObject> GMwaypoints)
    {
        //add a new item to the list of waypoints until the .Find() returns null, storing every waypoint in the waypoints[] list.
        int i = 0;
        while (GameObject.Find("WayPoint_" + i) != null)
        {
            //stores the Waypoint_i in the list of waypoints, until the find method returns null, which happens once the find() can't find anything.
            GMwaypoints.Add(GameObject.Find("WayPoint_" + i));
            i++;
        }

    }



    //All the GUI components that will be changed by other scripts, can attach these in the editor


    //All the timer variables
    //the index variables relate to the playerTime list, where the mins,secs, millisecs are being stored seperately
    private int minuteCount;
    public int minuteIndex = 0;
    private int secondCount;
    public int secondIndex = 1;
    private float milliCount;
    public int milliIndex = 2;
    private string MilliDisplay;

    public GameObject MinuteBox;
    public GameObject SecondBox;
    public GameObject MilliBox;

    public List<float> playerTime;
    public IEnumerator DisplayMessage(TextMeshProUGUI textBox, string message, float amountOfTime)
    {
        //display the message
        textBox.text = message;
        Debug.Log(message);
        //wait for the set amount of time
        yield return new WaitForSeconds(amountOfTime);
        //remove the message again
        textBox.text = "";
    }

    public void DisplayTimer()
    {
        //if the game has started then start the timer, this shouldn't start until the car has been spawned in.
        if (waypointController.started == true)
        {
            milliCount += Time.deltaTime * 10;
            MilliDisplay = milliCount.ToString("F0");
            MilliBox.GetComponent<TextMeshProUGUI>().text = "" + MilliDisplay;

            if (milliCount >= 10)
            {
                milliCount = 0;
                secondCount += 1;
            }

            if (secondCount <= 9)
            {
                SecondBox.GetComponent<TextMeshProUGUI>().text = "0" + secondCount + ".";
            }
            else
            {
                SecondBox.GetComponent<TextMeshProUGUI>().text = "" + secondCount + ".";
            }

            if (secondCount >= 60)
            {
                secondCount = 0;
                minuteCount += 1;
            }

            if (minuteCount <= 9)
            {
                MinuteBox.GetComponent<TextMeshProUGUI>().text = "0" + minuteCount + ":";
            }
            else
            {
                MinuteBox.GetComponent<TextMeshProUGUI>().text = "" + minuteCount + ":";
            }
        }
    }

    public void GameEnd()
    {
        //on game end, stop timer, remove player to stop movement, switch scenes
        //stores the players score. as a list of floats
        playerTime.Add(minuteCount);
        playerTime.Add(secondCount);
        playerTime.Add(milliCount);

        SceneManager.LoadScene("End Screen");

    }

}
