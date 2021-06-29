using SimpleJSON;
using System.IO;
using UnityEngine;

public class JSONParser : MonoBehaviour
{
    void Start()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/SONGTEST.json");
        JSONNode data = JSON.Parse(jsonString);
        foreach (JSONNode o in data.Children)
        { 
            JSONNode tags = o["tags"];
            string obj = tags["object"];
            string time = tags["time"];

            Debug.Log("obj : " + obj);
            Debug.Log("time : " + time);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
