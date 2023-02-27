using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	public ChaosManager chaosManager;
	public Slider itterationTimeSlider;
	
	public TMPro.TMP_InputField verticesInput;
	
	public TMP_Dropdown presetsDropdown;
	public TMP_Dropdown restrictionsDropdown;
	
	
	public TMPro.TextMeshProUGUI itterationText;
	
	
	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	
	public void ItterationSliderValueChanged()
	{
		chaosManager.SetItterationTime(itterationTimeSlider.value);
	}
	
	public void VerticiesInputValueChanged()
	{
		chaosManager.CreateShape(int.Parse(verticesInput.text.ToString()));
	}
	
	public void PresetsDropdownValueChanged()
	{
		string presetChoice = presetsDropdown.options[presetsDropdown.value].text;
		chaosManager.LoadPreset(presetChoice);
	}
	
	
	public void RestrictionsDropdownValueChanged()
	{
		string restrictionsChoice = restrictionsDropdown.options[restrictionsDropdown.value].text;
		chaosManager.SelectRestrictions(restrictionsChoice);
	}
	
	
}
