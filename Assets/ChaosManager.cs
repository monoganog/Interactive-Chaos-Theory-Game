using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosManager : MonoBehaviour
{

    public float iterationTime;
    private float currentTime;
    public GameObject objectToInstantiate;
    public Transform currentLocation;
    public Transform nextChosenTransform;

    public Transform point1, point2, point3;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("FindNextPoint",0.5f,0.001f);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>= iterationTime)
        {
            FindNextPoint();
            currentTime = 0;
        }
    }

    public void FindNextPoint(){

        int randomNum = Random.Range(1,4);

        if(randomNum == 1)
        {
            nextChosenTransform = point1;
        }
        if(randomNum == 2)
        {
            nextChosenTransform = point2;
        }
        if(randomNum == 3)
        {
            nextChosenTransform = point3;
        }

        FindMiddle();
    }

    private void FindMiddle()
    {

        Vector3 newPos = (currentLocation.position + nextChosenTransform.position) / 2;
        GameObject instance = Instantiate(objectToInstantiate,newPos,Quaternion.identity);
        //instance.transform.localScale = Vector3.one * 0.1f;
        currentLocation.position = newPos;
    }
}
