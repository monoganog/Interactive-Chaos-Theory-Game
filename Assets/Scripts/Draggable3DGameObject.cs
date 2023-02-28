using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable3DGameObject : MonoBehaviour
{
    public void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
      
    }
}
