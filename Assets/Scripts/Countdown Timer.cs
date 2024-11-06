using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{

    public TextMeshProUGUI countDownTextBox;
    private float countTime = 3.4f;
    public bool timerFinished = false;

    public GameManager gameManager;


    //starts the countdown, printing a number every second, rounded down, as bug was occuring and not
    //displyaing '1'
    //starts the second coroutine that will remove the go! text after 2 second delay
    //Need to add a bool value that will then control whether or not the whole program can run. 
    public IEnumerator StartCountDown()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        while (countTime > 0)
        {
            yield return new WaitForSeconds(1f);
            countDownTextBox.text = "" + Mathf.Round(countTime);
            countTime--;
        }

        if (countTime < 1)
        {
            countDownTextBox.text = "GO!";


            StartCoroutine(ClearTextAfterDelay(2f));
            timerFinished = true;

            gameManager.InstantiatePlayer();

            //OnCountdownFinished?.Invoke();
        }

    }

    public IEnumerator ClearTextAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        countDownTextBox.text = "";
    }
}
