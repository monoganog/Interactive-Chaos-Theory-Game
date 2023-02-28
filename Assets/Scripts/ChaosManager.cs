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
	public List<Transform> vertices = new List<Transform>();
	
	// Private Variables
	private float iterationTime;
	private float currentTime;
	
	public bool paused { get; set; }
	private Transform nextChosenTransform;

	private bool canVisitPreviousVertex = true;

	private bool canBeOnePlaceAwayClockwise = true;

	private bool canBeTwoPlacesAwayFromPrevious = true;


	private int lastVisittedIndex;

	public int ammountOfParticles { get; set; }

	private int moveAmmountNumerator = 1;
	private int moveAmmountDenominator = 2;



	// Scale factor
	private int scaleFactor = 40;

	// Start is called before the first frame update
	void Start()
	{
		LoadPreset("Serpinsky Triangle");
		paused = false;
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
		FindMidpoint();

		// finally assign the value to the last visited index to be the value of the Next/Chosen index
		lastVisittedIndex = indexOfNextVertex;
	}

	private void FindMidpoint()
	{
		// TODO investigate why this isnt working as intended with the ability to change the numerator and denominator
		// Create and assign the value to the middle point to be the middle of the current position and the chosen corner point
		Vector3 middlePoint = (currentLocation.position / moveAmmountDenominator) + (nextChosenTransform.position / moveAmmountDenominator) * moveAmmountNumerator;
		
		
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
	
	
	// Creates a shape with a given number of vertices
	public void CreateShape(int numOfVertices)
	{
		// Remove all previous Vertices
		RemoveAllVertices();
		
		// Add new vertices
		for(int i = 0; i < numOfVertices; i++)
		{
			AddVertex();
		}
		// Calculate angle between each vertex
		float angle = 2 * Mathf.PI / numOfVertices; // 72°
		
		// Itterate over each vertex
		for (int i = 0; i < numOfVertices; i++)
		{	
			// Assign position to each vertex
			vertices[i].position  = new Vector3
			(
				Mathf.Cos(-angle * i + Mathf.PI/2),
				Mathf.Sin(-angle * i + Mathf.PI/2),
				0
			)
			* scaleFactor;
		}
	}

	// Adds a new vertex
	public void AddVertex()
	{
		// Spawn new vertex
		GameObject instance = Instantiate(vertexPrefab);
		// Apply a random Offset
		Vector3 randomOffset = new Vector3(0, Random.Range(-5, 6), 0);
		instance.transform.position += randomOffset;

		// Add new vertex to the vertices list
		vertices.Add(instance.transform);

		// Update the UI on the vertex to be representitive of the count
		instance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "V" + vertices.Count.ToString();
	}

	public void DrawLine(Vector3 start, Vector3 end)
	{
		line.SetPosition(0, start);
		line.SetPosition(1, end);
	}

	public void RemoveAllVertices()
	{
        if (vertices.Count > 0)
        {
			foreach (var vertex in vertices)
			{
				Destroy(vertex.gameObject);
			}
			vertices.Clear();
		}

	}

	private void RemoveAllRestrictions()
	{
		canBeOnePlaceAwayClockwise = true;
		canVisitPreviousVertex = true;
		canBeTwoPlacesAwayFromPrevious = true;
	}
	
	// TODO may need to rename these bools to be shorter
	public void SelectRestrictions(string restrictions)
	{
		switch (restrictions)
		{
			case "None":
			{

				RemoveAllRestrictions();
				break;
			}
			case "Can't be one away clockwise":
			{
				RemoveAllRestrictions();
				canBeOnePlaceAwayClockwise = false;
	
				break;
			}
			case "Can't visit previous vertex":
			{
				RemoveAllRestrictions();
				canVisitPreviousVertex = false;
				break;
			}
			case "Can't be two places away from previous vertex":
			{
				RemoveAllRestrictions();
				canBeTwoPlacesAwayFromPrevious = false;
				break;
			}
			default:
			{
				break;
			}
		}
	}

	// TODO add more presets
	public void LoadPreset(string presetName)
	{
		switch (presetName)
		{
			case "Serpinsky Triangle":
			{
				CreateShape(3);

				// Set other params
				RemoveAllRestrictions();
				
				break;
			}
			case "Restricted Square 1":
			{
				CreateShape(4);

				// Set other params
				RemoveAllRestrictions();
				canVisitPreviousVertex = false;

				break;
			}
			default:
			{
				break;
			}
		}
	}


	// TODO investigate why it needs to stop  before applying the value change otherwise its showing wrong positioning
	//public void SetMoveLengthNumerator(int val)
	//   {
	//	moveAmmountNumerator = val;

	//}

	//public void SetMoveLengthDenominator(int val)
	//{
	//	moveAmmountDenominator = val;
	//}



}
