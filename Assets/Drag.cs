using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    //public Vector3 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDrag()
    {
    Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    transform.position = objPosition;
    }
}
