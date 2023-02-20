using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ChaosManager : MonoBehaviour
{
    public LineRenderer line;
    public Slider itterationTimeSlider;
    
    public float iterationTime;
    private float currentTime;
    public GameObject objectToInstantiate;
    public Transform currentLocation;
    private Transform nextChosenTransform;

    int itteration = 0;
    public TMPro.TextMeshProUGUI itterationText;

    public TMPro.TextMeshProUGUI fpsText;

    public Transform[] points;
    

    public TMP_InputField p1x;
    public TMP_InputField p1y;

    public TMP_InputField p2x;
    public TMP_InputField p2y;

    public TMP_InputField p3x;
    public TMP_InputField p3y;

    public TMP_InputField startingPointx;
    public TMP_InputField startingPointy;

    public List<GameObject> instantiatedObjects;


public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("FindNextPoint",0.5f,0.001f);
        InitialiseUI();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>= iterationTime && !paused)
        {
            FindNextPoint();
            currentTime = 0;
        }

        itterationText.text = itteration.ToString();

        fpsText.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
    }

    public void FindNextPoint(){

        int randomNum = Random.Range(0,points.Length);


        nextChosenTransform = points[randomNum];

        DrawLine(currentLocation.position,nextChosenTransform.position);

        FindMiddle();
    }

    private void FindMiddle()
    {

        Vector3 newPos = (currentLocation.position + nextChosenTransform.position) / 2;
        GameObject instance = Instantiate(objectToInstantiate,newPos,Quaternion.identity);
        instantiatedObjects.Add(instance);
        //instance.transform.localScale = Vector3.one * 0.1f;


        currentLocation.DOMove(newPos,iterationTime);

        itteration++;
    }

    public void UpdateItterationTime(){
        iterationTime = itterationTimeSlider.value;
    }

    public void InitialiseUI()
    {
        p1x.text = points[0].transform.position.x.ToString();
        p1y.text = points[0].transform.position.y.ToString();

        p2x.text = points[1].transform.position.x.ToString();
        p2y.text = points[1].transform.position.y.ToString();

        p3x.text = points[2].transform.position.x.ToString();
        p3y.text = points[2].transform.position.y.ToString();

        startingPointx.text = currentLocation.position.x.ToString();
        startingPointy.text = currentLocation.position.y.ToString();
    }


    public void UpdatePointPositions()
    {
        points[0].transform.position = new Vector3(float.Parse(p1x.text),float.Parse(p1y.text),0);
        points[1].transform.position = new Vector3(float.Parse(p2x.text),float.Parse(p2y.text),0);
        points[2].transform.position = new Vector3(float.Parse(p3x.text),float.Parse(p3y.text),0);

        currentLocation.position = new Vector3(float.Parse(startingPointx.text),float.Parse(startingPointy.text),0);
    }

    public void UpdateStartPos()
    {

    }

    public void TogglePlayPause()
    {
        paused = !paused;
    }

    public void Stop()
    {
        itteration = 0;
        paused = true;

        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();
    }

    public void DrawLine(Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
