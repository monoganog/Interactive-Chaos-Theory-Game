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
	
	public TMP_InputField verticesInput;
	public TMP_InputField moveAmmountNumeratorInput;
	public TMP_InputField moveAmmountDenominatorInput;

	public TMP_Dropdown presetsDropdown;
	

	public TMP_Dropdown restrictionsDropdown;
	public TMP_Dropdown fractalColourDropdown;
	public TMP_Dropdown backgroundColourDropdown;


	public TextMeshProUGUI itterationText;
	public TextMeshProUGUI fpsText;

	public Material fractalMaterial;
	public bool showingUI = true;

	public LineRenderer line;

	public GameObject UIPanelGameObject;
	public GameObject UIPanelPlusGameObject;
	private bool UIPanelVisible = true;

	private float UIPanelStartingHeight;
	private float UIPanelStartingWidth;

	public Vector3 RotateAmmount;

	// Start is called before the first frame update
	void Start()
	{
		ApplyStartingUIConditions();

		UIPanelStartingHeight = UIPanelGameObject.GetComponent<RectTransform>().sizeDelta.y;
		UIPanelStartingWidth = UIPanelGameObject.GetComponent<RectTransform>().sizeDelta.x;
	}

	private void ApplyStartingUIConditions()
    {
		// Load the first element in the drop down for both Background Colour and Fractal Colour
		BackgroundColourDropdownValueChanged();
		FractalColourDropdownValueChanged();
		verticesInput.text = "3";
	}

    private void Update()
    {
		// Ammount of itterations so far is equal to the particle count
		itterationText.text = chaosManager.ammountOfParticles.ToString();

		// Calculation to display the frames per second without decimals
		fpsText.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
	}

	public void OpenWebURL(string url)
    {
		Application.OpenURL(url);
	}

	public void ToggleViewUIPanel()
    {
		UIPanelVisible = !UIPanelVisible;

        if (UIPanelVisible)
        {
			// Maximise UI
			UIPanelGameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(UIPanelStartingWidth, UIPanelStartingHeight), 0.4f);
			UIPanelPlusGameObject.transform.DOLocalRotate(Vector3.zero, 0.4f);
		}
        else
        {
			// Minimise UI
			UIPanelGameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(UIPanelStartingWidth, 35),0.4f);
			UIPanelPlusGameObject.transform.DOLocalRotate(RotateAmmount, 0.4f);
		}

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
		//chaosManager.SetMoveLengthNumerator(int.Parse(moveAmmountNumeratorInput.text.ToString()));
	}

	public void MoveAmmountDenominatorInputValueChanged()
	{
		//chaosManager.SetMoveLengthDenominator(int.Parse(moveAmmountDenominatorInput.text.ToString()));
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

	public void ToggleGameUIVisibility()
    {
		showingUI = !showingUI;
		
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

	// Stop the simulation
	public void Stop()
	{
		// Reset ammount of particles
		chaosManager.ammountOfParticles = 0;

		// Pause sim
		chaosManager.paused = true;

		// Clear the spawned particles and hide the line renderer
		chaosManager.particles.Clear();
		HideLineRenderer();
	}

	public void HideLineRenderer()
	{
		line.gameObject.SetActive(false);
	}

	public void ShowLineRenderer()
	{
		line.gameObject.SetActive(true);
	}
}
