using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{

    //Makes this script able to be accessed from the simulation screen and take the chosen car and 
    //track varibales to instantiate them in the next scene
    private static MenuManager instance;

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

    public static MenuManager Instance
    {
        get { return instance; }
    }

    public GameObject chosenCar;
    public GameObject chosenTrack;
    public Button startButton;

    private string carNeededMessage = "Please chose a car before starting!";
    private string trackNeededMessage = "Please choose a track before starting!";
    private string startText = "Start!";

    private float revertDelay = 5.0f;

    //two lists that hold the different car and track prefabs, assigned within the editor
    public List<GameObject> cars;
    public List<GameObject> tracks;


    public void SetCar(int carIndex)
    {
        //check the index given is between the indexes in the car list
        if (carIndex >= 0 && carIndex <= cars.Count)
        {
            chosenCar = cars[carIndex];
        }
        else
        {
            Debug.Log("There was an error selecting the car (indexOutOfBounds)");
            chosenCar = null;
        }
    }

    public void SetTrack(int trackIndex)
    {
        //check the index given is between the indexes in the track list
        if (trackIndex >= 0 && trackIndex <= tracks.Count)
        {
            chosenTrack = tracks[trackIndex];
        }
        else
        {
            Debug.Log("There was an error selecting the track (indexOutOfBounds)");
            chosenTrack = null;
        }
    }

    public void StartSim()
    {
        //checks that both chosenCar and chosenTrack both are NOT NULL
        if (chosenCar == null)
        {
            ChangeButtonText(carNeededMessage, startButton);
        }
        else if (chosenTrack == null)
        {
            ChangeButtonText(trackNeededMessage, startButton);
        }
        else
        {

            Debug.Log("Switching scenes now...");
            Debug.Log("\nStarting the scene with track: " + chosenTrack.name + " And the car is: " + chosenCar.name);
            SceneManager.LoadScene(3);
        }
    }


    //takes the message you wish to display, and the button to display it on
    public void ChangeButtonText(string message, Button button)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = message;
        StartCoroutine(RevertTextAfterDelay(revertDelay));
    }


    //timer before reverting the start button to its orginal text
    IEnumerator RevertTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeButtonText(startText, startButton);
    }


}
