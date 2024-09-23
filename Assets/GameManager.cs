using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{
    public float timerDuration = 300f; // 5 minutes timer, adjust as needed
    private float currentTime = 0;
    private float elapsedTime = 0f;
    private float savedTime = 0f;
    private bool isTimerRunning = false;

    private int chosenMedalCount = 0;
    private int oneDownMedalCount = 0;

    public TextMeshProUGUI txtTimer;
    public TextMeshProUGUI txtStart;
    public Slider timeSlider;

    public Button btnStart;

    public bool enableStartButton = false;
    public Button btnBronze;
    public Button btnSilver;
    public Button btnGold;
    public Button btnAT;
    private Button selectedButton; // Keeps track of the currently selected medal button
    private string SelectedMedal;

    public TextMeshProUGUI txtChoose;
    public TextMeshProUGUI txtDuration;

    public TextMeshProUGUI txtChosenMedalCount;
    public TextMeshProUGUI txtOneDownCount;

    public Button btnPause;
    public Button btnSkip;
    public Button btnBrokenMap;

    public int increment;

    void Start()
    {
        // Automatically select the "AT" medal button at the start
        SelectATMedal();

        if(SceneManager.GetActiveScene().name == "RLC")
        {
            timeSlider.maxValue = 60;
            timeSlider.minValue = 1;
            timeSlider.value = 30;
            increment = 60;
        }
        else
        {
            timeSlider.maxValue = 30;
            timeSlider.minValue = 1;
            timeSlider.value = 10;
            increment = 60;
        }
        // Reset all values
        //PlayerPrefs.DeleteAll();
    }

    void Update()
    {
        timerDuration = timeSlider.value * increment;
        btnStart.enabled = enableStartButton;

        // Optional: For debugging or visual feedback, you can use this line
        // txtTimer.text = btnStart.enabled.ToString();
    }

    public void StartChallenge()
    {
        PlayerPrefs.SetFloat("SliderValue", timeSlider.value);
        PlayerPrefs.SetString("MedalChosen", SelectedMedal);
        SceneManager.LoadScene("Play" + SceneManager.GetActiveScene().name);
    }

    public void slideChange()
    {
        txtTimer.text = timeSlider.value.ToString() + ":00";
    }

    public void medalChosen(string medal)
    {
        enableStartButton = true;
        SelectedMedal = medal;

        // Deselect the previously selected button
        if (selectedButton != null)
        {
            ResetButtonAppearance(selectedButton);
        }

        // Find the button associated with the selected medal and mark it as selected
        switch (medal)
        {
            case "Bronze":
                selectedButton = btnBronze;
                break;
            case "Silver":
                selectedButton = btnSilver;
                break;
            case "Gold":
                selectedButton = btnGold;
                break;
            case "AT":
                selectedButton = btnAT;
                break;
        }

        // Apply the selected state appearance to the new selected button
        HighlightButton(selectedButton);
    }

    private void SelectATMedal()
    {
        medalChosen("AT");
        btnAT.Select(); // This will visually indicate that AT is selected
    }

    private void HighlightButton(Button button)
    {
        // Set the selected button's normal color to a brighter color
        ColorBlock cb = button.colors;
        Color normalColor;
        Color highlightedColor;

        // Set the color based on provided hex values
        if (ColorUtility.TryParseHtmlString("#828282", out normalColor) &&
            ColorUtility.TryParseHtmlString("#F5F5F5", out highlightedColor))
        {
            cb.normalColor = highlightedColor;
            cb.highlightedColor = highlightedColor;
            cb.pressedColor = normalColor; // Optional: make the pressed color match normal
            cb.selectedColor = highlightedColor;
            cb.disabledColor = normalColor; // Set a default color for disabled state
        }

        button.colors = cb;
    }

    private void ResetButtonAppearance(Button button)
    {
        // Reset the button to its original appearance
        ColorBlock cb = button.colors;
        Color normalColor;
        Color highlightedColor;

        // Set the color based on provided hex values
        if (ColorUtility.TryParseHtmlString("#828282", out normalColor) &&
            ColorUtility.TryParseHtmlString("#F5F5F5", out highlightedColor))
        {
            cb.normalColor = normalColor;
            cb.highlightedColor = highlightedColor;
            cb.pressedColor = normalColor; // Optional: match pressed color with normal
            cb.selectedColor = normalColor;
            cb.disabledColor = normalColor; // Set a default color for disabled state
        }

        button.colors = cb;
    }

    public string getNextLevel()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://exostats.nl/?api&randomlevellink&minAT=0&maxAT=30000&minGold=0&maxGold=30000");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            string jsonResponse = reader.ReadToEnd();
            JObject responseObject = JObject.Parse(jsonResponse);

            string link = (string)responseObject["link"];
            return link;
        }
    }
}
