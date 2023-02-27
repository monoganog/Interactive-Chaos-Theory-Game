using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ChaosManager : MonoBehaviour
{
	public GameObject vertexPrefab;
	public ParticleSystem particles;

	public Transform currentLocation;
	[Header("Line Renderer")]
	public LineRenderer line;

  
	[Header("Corner Vertices")]
	public List<Transform> vertices;
	
	[Header("UI Settings")]
	public Slider itterationTimeSlider;
	public TMPro.TextMeshProUGUI itterationText;
	public TMPro.TextMeshProUGUI fpsText;
	
	public TMPro.TMP_InputField verticesInput;
	
	public TMPro.TMP_InputField moveAmountNumeratorInput;
	
	public TMPro.TMP_InputField moveAmountDenominatorInput;

	private Button removeButton;
	// Private Variables
	private float iterationTime;
	private float currentTime;
	
	private bool paused = false;
	private Transform nextChosenTransform;

	public bool canVisitPreviousVertex = true;

	public bool canBeOnePlaceAwayClockwise = true;
	
	public bool canBeTwoPlacesAwayFromPrevious = true;


	private int lastVisittedIndex;

	private int ammountOfParticles;

	// Scale factor
	private int scaleFactor = 40;

	// Start is called before the first frame update
	void Start()
	{
		//InitialiseUI();
		//removeButton = GameObject.Find("Remove").GetComponent<Button>();
		
		//LoadSerpinskyTrianglePreset();
		
		//CreateShape(9);
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
			FindNextVertex();
			currentTime = 0;
		}

		// Ammount of itterations so far is equal to the amount of instantiated objects
		itterationText.text = ammountOfParticles.ToString();

		// Calculation to display the frames per second without decimals
		fpsText.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
	}

	// Method to find the next corner vertex to be selected
	public void FindNextVertex(){

		// The index of the next vetex is a random number between 0 and the total amount of vertices
		int indexOfNextVertex = Random.Range(0,vertices.Count);

		// If not allowed to visit the previous vertex
		if(!canVisitPreviousVertex)
		{
			while(indexOfNextVertex == lastVisittedIndex)
			{
				indexOfNextVertex = Random.Range(0,vertices.Count);
			}
		}

		// If the current vertex cannot be one place away (clockwise) from the previously chosen vertex
		if(!canBeOnePlaceAwayClockwise)
		{
			// Calculate the next element in the array and wrap arround if the next sequential element is out of bounds
			int nextClockwiseElementIndex = (lastVisittedIndex + 1) % vertices.Count;

			// While the next vertex to be chosen is == next cloxkwise element, reroll
			while(indexOfNextVertex == nextClockwiseElementIndex)
			{
				indexOfNextVertex = Random.Range(0,vertices.Count);
			}
		}
		
		// Next Vertex cannot be two places away from the previous, meaning the next vertex selected must be either the same, or one either side of the last chosen vertex
		if(!canBeTwoPlacesAwayFromPrevious)
		{
			
			int nextClockwiseElementIndex = (lastVisittedIndex + 1) % vertices.Count;
			int nextAntiClockwiseElementIndex = (lastVisittedIndex - 1 + vertices.Count) % vertices.Count;
			
			Debug.Log("last visited: " + lastVisittedIndex + "  Next Clockwise: " + nextClockwiseElementIndex + "  Next anti clockwise: "+ nextAntiClockwiseElementIndex);
			
			//int twoElementsAway = (lastVisittedIndex + 2) % vertices.Count;
			
			// Basically we want the next index to either be one away from the last index or the last index. If its not then reroll
			while(indexOfNextVertex != nextClockwiseElementIndex && indexOfNextVertex != nextAntiClockwiseElementIndex && indexOfNextVertex != lastVisittedIndex)
			//while(indexOfNextVertex == twoElementsAway)
			{
				// Reroll
				indexOfNextVertex = Random.Range(0,vertices.Count);
			}
		}

		// Use the random number as the index of the points array that contains the corner points
		nextChosenTransform = vertices[indexOfNextVertex];

		// Draw a line from the current location to the next chosen corner transform
		DrawLine(currentLocation.position,nextChosenTransform.position);

		// Find the middle of the 2 chosen points
		FindMiddle();

		// finally assign the value to the last visited index to be the value of the Next/Chosen index
		lastVisittedIndex = indexOfNextVertex;
	}

	private void FindMiddle()
	{
		// Create and assign the value to the middle point to be the middle of the current position and the chosen corner point
		Vector3 middlePoint = (currentLocation.position / 2) + (nextChosenTransform.position/2 );
		
		// TODO make an option to select the fraction along the line that should be considered the "middle point"
		// Divide each vector by three before summing to get 1/3rd of the way across from a -> b. Times by 2 to get 2/3rds 
		Vector3 twoThirdsPoint = (currentLocation.position / 3) + (nextChosenTransform.position / 3) *2;
		
		// Create parameters for a particle system
		var emitParams = new ParticleSystem.EmitParams();

		// Assign the position of the particle system to be the middle point
		emitParams.position = middlePoint;
		
		// Emit one particle and increment particle count
		particles.Emit(emitParams, 1);
		ammountOfParticles++;

		// Animate the current location to be at the last middle point
		currentLocation.DOMove(middlePoint,iterationTime);
	}

	public void SetItterationTime(float time)
	{
		// Sets the interval between points placed
		iterationTime = time;
	}


	public void TogglePlayPause()
	{
		// flip pause bool
		paused = !paused;

		// If game is paused hide the line renderer otherwise if its playing then show it
		if(paused)
		{
			HideLineRenderer();
		}
		else
		{
			ShowLineRenderer();
		}
	}
	
	public void ToggleViewUI(bool val)
	{
		Debug.Log(val);
	}

	// Stop the simulation
	public void Stop()
	{
		// Reste ammount of particles
		ammountOfParticles = 0;

		// Pause sim
		paused = true;

		// Clear the spawned particles and hide the line renderer
		particles.Clear();
		HideLineRenderer();
	}
	
	public void CreateShape()
	{
		int sides = int.Parse(verticesInput.text.ToString());
		RemoveAllVertices();
		
		// Add vertices
		for(int i = 0; i < sides; i++)
		{
			AddVertex();
		}
		// Calculate angle between each vertex
		float angle = 2 * Mathf.PI / sides; // 72°
		
		// Itterate over each vertex
		for (int i = 0; i < sides; i++)
		{	
			// Assign position to each vertex
			vertices[i].position  = new Vector3
			(
				Mathf.Cos(-angle * i + Mathf.PI/2),
				Mathf.Sin(-angle * i + Mathf.PI/2),
				0
			) 	* scaleFactor;
		}
	}
	// Method overloading for calling the CreateShape method internally with arguments rather than via UI
	public void CreateShape(int sides)
	{
		
		RemoveAllVertices();
		
		// Add vertices
		for(int i = 0; i < sides; i++)
		{
			AddVertex();
		}
		// Calculate angle between each vertex
		float angle = 2 * Mathf.PI / sides; // 72°
		
		// Itterate over each vertex
		for (int i = 0; i < sides; i++)
		{	
			// Assign position to each vertex
			vertices[i].position  = new Vector3
			(
				Mathf.Cos(-angle * i + Mathf.PI/2),
				Mathf.Sin(-angle * i + Mathf.PI/2),
				0
			) 	* scaleFactor;
		}
	}
	
	

	public void HideLineRenderer()
	{
		line.gameObject.SetActive(false);
	}

	public void ShowLineRenderer()
	{
		line.gameObject.SetActive(true);
	}
	
	public void DrawLine(Vector3 start, Vector3 end)
	{
		// Draw a line from the start to the end vectors
		line.SetPosition(0, start);
		line.SetPosition(1, end);
	}

	// Adds a new vertex, called Via UI Button
	public void AddVertex()
	{
		// Spawn new vertex
		GameObject instance = Instantiate(vertexPrefab);
		// Apply a random Offset
		Vector3 randomOffset = new Vector3(0,Random.Range(-5,6),0);
		instance.transform.position += randomOffset;
		
		// Add new vertex to the vertices list
		vertices.Add(instance.transform);
		
		// Update the UI on the vertex to be representitive of the count
		instance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "V"+ vertices.Count.ToString();

		// If there is 3 vertices make the remove button interactable
		if(vertices.Count==3)
		{
			//removeButton.interactable = true;
		}
	}

	// Removes a vertex, called Via UI Button
	public void RemoveVertex()
	{
		// Check to see if theres atleast 2 Vertices
		if(vertices.Count>2)
		{
			// Destroy the vertex
			Destroy(vertices[vertices.Count-1].gameObject);
			// Remove it from the list
			vertices.RemoveAt(vertices.Count-1);

			// If theres only two vertices then make the remove button not interactable
			if(vertices.Count==2)
			{
				//removeButton.interactable = false;
			}
		}
	}
	
	public void RemoveAllVertices()
	{
		foreach(var vertex in vertices)
		{
			Destroy(vertex.gameObject);
		}
		vertices.Clear();
	}
	
	public void LoadSerpinskyTrianglePreset()
	{
		CreateShape(3);
		
		// Set other params
		canBeOnePlaceAwayClockwise = true;
		canVisitPreviousVertex = true;
	}
	
	public void LoadRestrictedSquarePreset()
	{
		CreateShape(4);
		
		// Set other params
		canBeOnePlaceAwayClockwise = true;
		canVisitPreviousVertex = false;
	}
	
	public void LoadRestrictedClockwiseSquarePreset()
	{
		
		CreateShape(4);
		
		// Then alter these params
		canBeOnePlaceAwayClockwise = false;
		canVisitPreviousVertex = true;
	}
	
	public void LoadPentagonPreset()
	{
		CreateShape(5);
	}
	
	public void SelectRestrictions()
	{
		
	}
}
