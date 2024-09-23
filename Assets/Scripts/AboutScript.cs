using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AboutScript : MonoBehaviour
{
    public TextMeshProUGUI aboutTMP;

    void Start()
    {
        string aboutText = "Welcome to the Exoracer Challenge Tracker! This app is designed to enhance your Exoracer experience by selecting and tracking maps you can play and earn medals on.\n\n" +
                           "Features:\n" +
                           "- Map Selection: Randomly selects maps for you to play, ensuring a fresh challenge each time.\n" +
                           "- Medal Tracking: Earn Target Medals and One Down Medals, with options to skip maps after achieving certain medals.\n" +
                           "- Flexible Timer: Complete as many maps as possible within a 30-minute timer.\n" +
                           "- Run History: Keeps a detailed record of your runs, including skipped and completed maps.\n\n" +
                           "This tool is designed to make your Exoracer sessions more exciting and challenging, helping you improve your skills and enjoy the game even more.";

        aboutTMP.text = aboutText;
    }
}
