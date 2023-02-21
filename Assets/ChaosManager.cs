using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ChaosManager : MonoBehaviour
{
    public ParticleSystem particles;

    public Transform currentLocation;
    [Header("Line Renderer")]
    public LineRenderer line;
    [Header("Object to be placed")]
    public GameObject objectToInstantiate;
  
    [Header("Corner Points")]
    public Transform[] points;
    
    [Header("UI Settings")]
    public Slider itterationTimeSlider;
    public TMPro.TextMeshProUGUI itterationText;
    public TMPro.TextMeshProUGUI fpsText;
    public TMP_InputField p1x;
    public TMP_InputField p1y;
    public TMP_InputField p2x;
    public TMP_InputField p2y;
    public TMP_InputField p3x;
    public TMP_InputField p3y;
    public TMP_InputField startingPointx;
    public TMP_InputField startingPointy;

    // Private Variables
    private float iterationTime;
    private float currentTime;
    public List<GameObject> instantiatedObjects;
    private bool paused = false;
    private Transform nextChosenTransform;

    public bool canVisitPrevious = true;

    private int lastVisittedIndex;

    private int ammountOfParticles;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("FindNextPoint",0.5f,0.001f);
        InitialiseUI();




    }

    // Update is called once per frame
    void Update()
    {
        // Increment the current time counter
        currentTime += Time.deltaTime;

        // If the current time is bigger than the selected itteration time and its not paused
        if(currentTime>= iterationTime && !paused)
        {
            // Find the next point and reset the currentTime

               FindNextPoint();
         
            
            currentTime = 0;
        }


        
        

        // Ammount of itterations so far is equal to the amount of instantiated objects
        itterationText.text = ammountOfParticles.ToString();

        // Calculation to display the frames per second without decimals
        fpsText.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
    }

    // Method to find the next corner point to be selected
    public void FindNextPoint(){

        // Find a random number between 0 and the total amount of points
        int randomNum = Random.Range(0,points.Length);

        if(!canVisitPrevious)
        {
            while(randomNum == lastVisittedIndex)
            {
                randomNum = Random.Range(0,points.Length);
            }
        }

        // Use the random number as the index of the points array that contains the corner points
        nextChosenTransform = points[randomNum];

        // Draw a line from the current location to the next chosen corner transform
        DrawLine(currentLocation.position,nextChosenTransform.position);

        // Find the middle of the 2 chosen points
        FindMiddle();

        lastVisittedIndex = randomNum;
    }

    private void FindMiddle()
    {
        // Create and assign the value to the middle point to be the middle of the current position and the chosen corner point
        Vector3 middlePoint = (currentLocation.position + nextChosenTransform.position) / 2;
        // Instantiate an object at this middle point and add it to a list
        //GameObject instance = Instantiate(objectToInstantiate,middlePoint,Quaternion.identity);
        //instantiatedObjects.Add(instance);


        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = middlePoint;
        
        particles.Emit(emitParams, 1);

        ammountOfParticles++;

        // Animate the current location to be at the last middle point
        currentLocation.DOMove(middlePoint,iterationTime);
    }

    public void UpdateItterationTime()
    {
        // Assign the itteration time to be the value of the UI slider
        iterationTime = itterationTimeSlider.value;
    }

    
    public void InitialiseUI()
    {
        // Initialise all the UI text to be where the current points are
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
        // Update point positions once user has finishing entering values into the UI
        points[0].transform.position = new Vector3(float.Parse(p1x.text),float.Parse(p1y.text),0);
        points[1].transform.position = new Vector3(float.Parse(p2x.text),float.Parse(p2y.text),0);
        points[2].transform.position = new Vector3(float.Parse(p3x.text),float.Parse(p3y.text),0);
        currentLocation.position = new Vector3(float.Parse(startingPointx.text),float.Parse(startingPointy.text),0);
    }


    public void TogglePlayPause()
    {
        paused = !paused;
    }

    public void Stop()
    {
        ammountOfParticles = 0;

        // Pause
        paused = true;

        // Destroy all objects
        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        // Clear the list of objects
        instantiatedObjects.Clear();
    }

    public void DrawLine(Vector3 start, Vector3 end)
    {
        // Draw a line from the start to the end vectors
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
