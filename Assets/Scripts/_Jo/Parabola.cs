using System.Collections;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Collections.Generic;
using System.CodeDom;
using System;
using System.Linq;
using System.Globalization;

public class Parabola : MonoBehaviour
{
    public float Hauteur = -50f, TimeMotionObject = 5.0f, OffsetHauteurTarget = 0f, OffsetZTarget = 0f;

    private Vector3 targetAngle, currentAngle;
    private float x, y, z;
    private int idObjectToDisplay = 0;

    // LISTS CONTENANT TAGS DU JSON
    private List<int> idList = new List<int>();
    private List<float> timeList = new List<float>();

    [Header("Pool of Items")]
    [SerializeField]
    private List<GameObject> itemPool = new List<GameObject>();
    private List<int> itemIdList = new List<int>();

    [Header("Start GameObjects")]
    [SerializeField]
    private List<GameObject> startList = new List<GameObject>();
    private List<int> startIdList = new List<int>();

    [Header("Destination GameObjects")]
    [SerializeField]
    private List<GameObject> endList = new List<GameObject>();
    private List<int> endIdList = new List<int>();

    void Start()
    {
        //Clean Lists
        idList.Clear();
        timeList.Clear();
        itemIdList.Clear();
        startIdList.Clear();
        endIdList.Clear();

        string jsonString = File.ReadAllText(Application.dataPath + "/SONGTEST.json");
        JSONNode data = JSON.Parse(jsonString);

        // On parcours tous les nodes du fichier
        foreach (JSONNode o in data.Children)
        {
            // l'ID
            int id = int.Parse(o["id"]);

            // puis tous les autres
            JSONNode tags = o["tags"];
            int itemId = int.Parse(tags["itemId"]);
            float time = float.Parse(tags["time"], CultureInfo.InvariantCulture);
            int startId = int.Parse(tags["startId"]);
            int endId = int.Parse(tags["endId"]);

            // On alimente les listes pour les parcourir ensuite
            idList.Add(id);
            itemIdList.Add(itemId);
            timeList.Add(time);
            startIdList.Add(startId);
            endIdList.Add(endId);
        }
        // Call void to run game
        StartCoroutine(CreateFromList());
    }

    IEnumerator CreateFromList()
    {
        if (idList.Count > 0)
        {
            foreach (int idObjectToDisplay in idList)
            {
                // type de l'objet pour le cloner
                int objectToDisplay = itemIdList[idObjectToDisplay];
                // start time de l'objet
                float currentObjTime = timeList[idObjectToDisplay];

                Transform startTransform = startList[startIdList[idObjectToDisplay]].transform;

                // Start pos de l'objet
                float s_x = startTransform.position.x;
                float s_y = startTransform.position.y;
                float s_z = startTransform.position.z;

                Vector3 startPos = new Vector3(s_x, s_y, s_z);

                Transform endTransform = endList[endIdList[idObjectToDisplay]].transform;

                // End pos de l'objet
                float e_x = endTransform.position.x;
                float e_y = endTransform.position.y;
                float e_z = endTransform.position.z;

                Vector3 endPos = new Vector3(e_x, e_y + OffsetHauteurTarget, e_z + OffsetZTarget);

                // Clone source object from collection
                GameObject clone = Instantiate(itemPool[objectToDisplay], startPos, transform.rotation);
                clone.transform.parent = startTransform;

                /*
                // Random rotation
                x = UnityEngine.Random.Range(0f, 360f); // or anyvalue you like
                y = UnityEngine.Random.Range(0f, 360f);
                z = UnityEngine.Random.Range(0f, 360f);
                
                targetAngle = new Vector3(x, y, z);
                */

                // On attend d'etre au timing de l'objet
                if (idObjectToDisplay == 0)
                {
                    print("WAITING FOR FIRST OBJ START TIME DURING : " + currentObjTime);
                    yield return new WaitForSeconds(currentObjTime);

                    // Display Clone
                    // clone.GetComponent<Renderer>().enabled = true;
                    // clone.GetComponentInChildren<Renderer>().enabled = true;
                    //currentAngle = clone.transform.eulerAngles;

                }
                else
                {
                    float prevObjTime = timeList[idObjectToDisplay - 1];
                    print("WAITING FOR NEXT START TIME DURING : " + (currentObjTime - prevObjTime));
                    yield return new WaitForSeconds(currentObjTime - prevObjTime);

                    // Display Clone
                    // clone.GetComponent<Renderer>().enabled = true;
                    // clone.GetComponentInChildren<Renderer>().enabled = true;
                    //currentAngle = clone.transform.eulerAngles;
                }

                StartCoroutine(Moving(clone, startPos, endPos, TimeMotionObject));
            }
        }
    }

    IEnumerator Moving(GameObject clone, Vector3 start, Vector3 dest, float TimeMotionObject)
    {
        print("STARTING MOVING AT : " + Time.time);

        float timer = 0;

        while (timer <= 1.0f)
        {
            // Transform position
            float height = Mathf.Sin(Mathf.PI * timer) * Hauteur;
            clone.transform.position = Vector3.Lerp(start, dest, timer) + Vector3.up * height;

            /*
            // Rotation
            currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

            clone.transform.eulerAngles = currentAngle;
            */

            timer += Time.deltaTime / TimeMotionObject;
            yield return null;
        }
    }
}
