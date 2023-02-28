using UnityEngine;

/// <summary>
/// Simple camera controller to allow zome and movement on x and y axis
///
/// Ben Monaghan 2023
/// </summary>

public class OrthographicCameraController : MonoBehaviour
{
	public float zoomSpeed = 4f;
	public float moveSpeed = 10f;

	void Update()
	{
		
		// Mouse ScrollWheel to zoom
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if (zoom != 0f)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoom * zoomSpeed, 0.1f, 100f);
        }

		//Move camera up and down
		if (Input.GetMouseButton(1))
		{
			float xAxisValue = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
			float yAxisValue = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;

			transform.Translate(xAxisValue, yAxisValue, 0f);
		}
	}
}