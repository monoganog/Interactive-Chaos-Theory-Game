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
	public TMPro.TMP_InputField moveAmmountNumeratorInput;
	public TMPro.TMP_InputField moveAmmountDenominatorInput;

	public TMP_Dropdown presetsDropdown;
	

	public TMP_Dropdown restrictionsDropdown;
	public TMP_Dropdown fractalColourDropdown;
	public TMP_Dropdown backgroundColourDropdown;


	public TMPro.TextMeshProUGUI itterationText;

	public Material fractalMaterial;
	public bool showingUI = true;
	
	
	// Start is called before the first frame update
	void Start()
	{
		BackgroundColourDropdownValueChanged();
		FractalColourDropdownValueChanged();
		verticesInput.text = "3";
	}

    private void Update()
    {
		// Ammount of itterations so far is equal to the amount of instantiated objects
		itterationText.text = ammountOfParticles.ToString();

		// Calculation to display the frames per second without decimals
		fpsText.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
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

	public void MoveAmmountNumeratorInputValueChanged()
	{
		chaosManager.SetMoveLengthNumerator(int.Parse(moveAmmountNumeratorInput.text.ToString()));
	}

	public void MoveAmmountDenominatorInputValueChanged()
	{
		chaosManager.SetMoveLengthDenominator(int.Parse(moveAmmountDenominatorInput.text.ToString()));
	}

	public void FractalColourDropdownValueChanged()
	{
		string fractalColourChoice = fractalColourDropdown.options[fractalColourDropdown.value].text;


			switch (fractalColourChoice)
			{
				case "White":
					{
						fractalMaterial.color = Color.white;
						break;
					}
				case "Blue":
					{
						fractalMaterial.color = Color.blue;

					break;
					}
				case "Red":
					{
						fractalMaterial.color = Color.red;
					break;
					}

				default:
					{
						break;
					}
			}
		}

	public void BackgroundColourDropdownValueChanged()
	{
		string backgroundColourChoice = backgroundColourDropdown.options[backgroundColourDropdown.value].text;
		switch (backgroundColourChoice)
		{
			case "Dark Grey":
                {
                    Camera.main.backgroundColor = new Color32(0x1D, 0x1D, 0x1D, 0xFF);
					break;
				}
			case "Black":
				{
					Camera.main.backgroundColor = Color.black;
					break;
				}
			case "Blue":
				{
					Camera.main.backgroundColor = Color.blue;
					break;
				}
			case "Red":
				{
					Camera.main.backgroundColor = Color.red;
					break;
				}

			default:
				{
					break;
				}
		}
	}

	public void ToggleUI()
    {
		showingUI = !showingUI;
		//var foundCanvasObjects = FindObjectsOfType<CanvasRenderer>();
		Debug.Log("ToggleShow canvas with UI set to " + showingUI);

		
		GameObject[] thingsToBeHidden = GameObject.FindGameObjectsWithTag("GameObjectsToBeHidden");

		foreach (GameObject go in thingsToBeHidden)
		{
			go.GetComponent<Renderer>().enabled = showingUI;
		}

		Canvas[] allCanvases = FindObjectsOfType<Canvas>();

		foreach(var canvas in allCanvases)
        {
			if(canvas.name != "MainCanvas")
            {
				canvas.enabled = showingUI;
			}
        
        }
	}

	public void TogglePlayPause()
	{
		// flip pause bool
		chaosManager.paused = !chaosManager.paused;

		// If game is paused hide the line renderer otherwise if its playing then show it
		if (chaosManager.paused)
		{
			HideLineRenderer();
		}
		else
		{
			ShowLineRenderer();
		}
	}
}
