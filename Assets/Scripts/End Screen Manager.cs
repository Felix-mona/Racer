using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    public GameManager gameManagerScript;
    public TextMeshProUGUI userScoreBox;
    private List<float> userScore;

    private void Start()
    {
        gameManagerScript = GameManager.Instance;
        userScore = gameManagerScript.playerTime;
        userScoreBox.text = userScore[0].ToString() + ":" + userScore[1].ToString() + ":" + Mathf.Round(userScore[2]).ToString();
    }

}
