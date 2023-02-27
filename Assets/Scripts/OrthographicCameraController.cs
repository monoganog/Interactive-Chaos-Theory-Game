using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicCameraController : MonoBehaviour
{
	public float zoomSpeed = 4f;
	public float moveSpeed = 10f;

	void Update()
	{
		//Zoom in and out
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