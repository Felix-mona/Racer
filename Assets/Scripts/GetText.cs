using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;


public class GetText : MonoBehaviour
{
    //Where to place the text within the help scene
    public Transform helpWindow;
    public GameObject recallTextObject;

    private void Start()
    {
        //String that contains the file path to be later used
        string readPath = Application.streamingAssetsPath + "/TextFiles/" + "Help" + ".txt";

        List<string> fileLines = File.ReadAllLines(readPath).ToList();
        foreach(string line in fileLines)
        {
            Instantiate(recallTextObject, helpWindow);
            recallTextObject.GetComponent<Text>().text = line;
        }
    }
}
