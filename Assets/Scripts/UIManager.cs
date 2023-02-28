using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// This class is responsible for allowing the UI to interact with the Chaos Manager as well as handling any other UI
/// logic such as hiding or showing the menu panel, animations etc. Public the methods here are called via Unity events set up through
/// the editor.
///
///  Ben Monaghan 2023
/// </summary>

public class UIManager : MonoBehaviour
{
	[Header("Chaos Manager")]
	public ChaosManager chaosManager;

	[Header("Sliders")]
	public Slider itterationTimeSlider;

	[Header("Input fields")]
	public TMP_InputField verticesInput;
	public TMP_InputField moveAmmountNumeratorInput;
	public TMP_InputField moveAmmountDenominatorInput;

	[Header("Dropdowns")]
	public TMP_Dropdown presetsDropdown;
	public TMP_Dropdown restrictionsDropdown;
	public TMP_Dropdown fractalColourDropdown;
	public TMP_Dropdown backgroundColourDropdown;

	[Header("Text")]
	public TextMeshProUGUI itterationText;
	public TextMeshProUGUI fpsText;
	public TextMeshProUGUI versionText;

	[Header("Materials")]
	public Material fractalMaterial;

	[Header("Line Renderer")]
	public LineRenderer line;

	[Header("UI Panels")]
	public GameObject UIPanelGameObject;
	public GameObject UIPanelPlusGameObject;

	// Private Variables
	private bool showingUI = true;
	private bool UIPanelVisible = true;
	private float UIPanelStartingHeight;
	private float UIPanelStartingWidth;
	private Vector3 RotateAmmount = new Vector3(0,0,-90);

	// Start is called before the first frame update
	void Start()
	{
		ApplyStartingUIConditions();
	}

	// Applys the UI starting conditions
	private void ApplyStartingUIConditions()
    {
		// Load the first element in the drop down for both Background Colour and Fractal Colour
		BackgroundColourDropdownValueChanged();
		FractalColourDropdownValueChanged();
		verticesInput.text = "3";

		versionText.text = "v" + Application.version.ToString();

		UIPanelStartingHeight = UIPanelGameObject.GetComponent<RectTransform>().sizeDelta.y;
		UIPanelStartingWidth = UIPanelGameObject.GetComponent<RectTransform>().sizeDelta.x;
	}

    private void Update()
    {
		// Ammount of itterations so far is equal to the particle count
		itterationText.text = chaosManager.ammountOfParticles.ToString();

		// Calculation to display the frames per second without decimals
		fpsText.text = Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
	}

	// Opens web browser to my social links
	public void OpenWebURL(string url)
    {
		Application.OpenURL(url);
	}

	// Toggle the visibility of the Menu panel 
	public void ToggleViewMenuUIPanel()
    {
		UIPanelVisible = !UIPanelVisible;

        if (UIPanelVisible)
        {
			// Maximise UI with animation
			UIPanelGameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(UIPanelStartingWidth, UIPanelStartingHeight), 0.4f);
			// Animate the close icon
			UIPanelPlusGameObject.transform.DOLocalRotate(Vector3.zero, 0.4f);
		}
        else
        {
			// Minimise UI with animation
			UIPanelGameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(UIPanelStartingWidth, 35),0.4f);
			// Animate the open icon
			UIPanelPlusGameObject.transform.DOLocalRotate(RotateAmmount, 0.4f);
		}

	}

	// Method Called via Itteration UI Slider which will tell the ChaosManager how much long between itterations
	public void ItterationSliderValueChanged()
	{
		chaosManager.SetItterationTime(itterationTimeSlider.value);
	}

	// Method Called via Vertices UI Input which will tell the ChaosManager to create a shape with the given number of vertices
	public void VerticiesInputValueChanged()
	{
		chaosManager.CreateShape(int.Parse(verticesInput.text.ToString()));
	}

	// Method Called via Presets UI dropdown which will tell the ChaosManager preset to apply
	public void PresetsDropdownValueChanged()
	{
		string presetChoice = presetsDropdown.options[presetsDropdown.value].text;
		chaosManager.LoadPreset(presetChoice);
	}

	// Method Called via Restrictions UI dropdown which will tell the ChaosManager what restrictions to apply
	public void RestrictionsDropdownValueChanged()
	{
		string restrictionsChoice = restrictionsDropdown.options[restrictionsDropdown.value].text;
		chaosManager.SelectRestrictions(restrictionsChoice);
	}

	// TODO not yet implemented
	public void MoveAmmountNumeratorInputValueChanged()
	{
		//chaosManager.SetMoveLengthNumerator(int.Parse(moveAmmountNumeratorInput.text.ToString()));
	}

	public void MoveAmmountDenominatorInputValueChanged()
	{
		//chaosManager.SetMoveLengthDenominator(int.Parse(moveAmmountDenominatorInput.text.ToString()));
	}

	// Method Called via fractal colour UI dropdown change the fractals material colour
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

	// Method Called via background colour UI dropdown change the cameras backgroud colour
	public void BackgroundColourDropdownValueChanged()
	{
		string backgroundColourChoice = backgroundColourDropdown.options[backgroundColourDropdown.value].text;

		switch (backgroundColourChoice)
		{
			case "Dark Grey":
            {
				Camera.main.backgroundColor = new Color(0.066f, 0.066f, 0.066f);
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


	// TODO need to set new Vertices to also after being spawned
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

	private void HideLineRenderer()
	{
		line.gameObject.SetActive(false);
	}

	private void ShowLineRenderer()
	{
		line.gameObject.SetActive(true);
	}
}
