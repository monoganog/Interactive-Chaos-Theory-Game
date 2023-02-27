using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	public ChaosManager chaosManager;
	public Slider itterationTimeSlider;
	
	public 
	// Start is called before the first frame update
	void Start()
	{
		
	}

	
	public void UpdateItterationTime()
	{
		chaosManager.SetItterationTime(itterationTimeSlider.value);
	}
	
	public void UpdateVertices()
	{
		
	}
}
