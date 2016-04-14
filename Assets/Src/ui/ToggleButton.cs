using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
	private ColorBlock _defaultColors;
	private ColorBlock _activeModeColors;

	// Use this for initialization
	void Start ()
    {
        SetupColors();
    }

    void SetupColors()
    {
        _defaultColors = GetComponent<Button>().colors;
        _activeModeColors = _defaultColors;
        _activeModeColors.pressedColor = _activeModeColors.highlightedColor = _defaultColors.pressedColor;
        GetComponent<Button>().onClick.AddListener(TurnOn);
    }

    void TurnOn()
	{
		GetComponent<Button> ().colors = _activeModeColors;
		GetComponent<Button> ().onClick.RemoveListener (TurnOn);
	}

	void TurnOff()
	{
		GetComponent<Button> ().onClick.AddListener (TurnOn);
		GetComponent<Button> ().colors = _defaultColors;
	}

}

