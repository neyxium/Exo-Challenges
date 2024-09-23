using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RunHistoryEntry
{
    public string LevelLink { get; set; }
    public string LevelName { get; set; }
    public string LevelAuthor { get; set; }
    public bool TargetMedal { get; set; }
    public bool OneDownMedal { get; set; }
    public bool skipped { get; set; }
}
public class TimerRunning : MonoBehaviour
{
    public float timerDuration = 300f; // 5 minutes timer, adjust as needed
    public float MaxTime;
    private float currentTime = 0;
    private float elapsedTime = 0f;
    private float savedTime = 0f;
    //private bool isTimerRunning = true;
    public TextMeshProUGUI huntedMedalCountText;
    public TextMeshProUGUI oneDownMedalCountText;

    //set images for medals
    public Image huntedMedalImage;
    public Image oneDownMedalImage;

    public TextMeshProUGUI txtTimer;
    public TextMeshProUGUI txtRemainer;
    private string huntedMedal;
    private int huntedMedalCount = 0;
    private int oneDownMedalCount = 0;

    string levelLink;
    int skips = 5;
    public TextMeshProUGUI txtSkips;
    public TextMeshProUGUI txtLevelName;
    public string levelName;
    public TextMeshProUGUI txtAuthor;
    public string author;

    public Sprite medal_author;
    public Sprite medal_gold;
    public Sprite medal_silver;
    public Sprite medal_bronze;

    public Button PauseButton;
    public Button SkipButton;

    bool paused = false;

    bool countdownEnd = false;
    float countDownTimer = 3f;
    public TextMeshProUGUI txtCountdown;
    public Image cover;

    private List<RunHistoryEntry> runHistory = new List<RunHistoryEntry>();
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "PlayRLC")
        {
            skips = 5;
            txtSkips.text = "SKIP (" + skips.ToString() + ")";
        }
        else
        {
            float sliderValue = PlayerPrefs.GetFloat("SliderValue");

            MaxTime = sliderValue * 60f;
            skips = 99999;

        }
        if (PlayerPrefs.HasKey("SliderValue"))
        {
            float sliderValue = PlayerPrefs.GetFloat("SliderValue");

            timerDuration = sliderValue * 60f;
        }
        if (PlayerPrefs.HasKey("MedalChosen"))
        {
            huntedMedal = PlayerPrefs.GetString("MedalChosen");
        }
        if (huntedMedal == "AT")
        {
            //tu se nastavi UI za AT medal ki jih dobim iz Assets/Icons
            huntedMedalImage.sprite = medal_author;
            oneDownMedalImage.sprite = medal_gold;
        }
        else if (huntedMedal == "Gold")
        {
            //tu se nastavi UI za gold medal in silver
            huntedMedalImage.sprite = medal_gold;
            oneDownMedalImage.sprite = medal_silver;
        }
        else if (huntedMedal == "Silver")
        {
            //tu se nastavi UI za silver medal in bronze
            huntedMedalImage.sprite = medal_silver;
            oneDownMedalImage.sprite = medal_bronze;
        }
        else if (huntedMedal == "Bronze")
        {
            //tu se nastavi UI za bronze medal
            huntedMedalImage.sprite = medal_bronze;
        }
        elapsedTime = 0f;
        //LoadTimer();
        float currentTime = timerDuration - elapsedTime;
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int remainingMinutes = Mathf.FloorToInt(elapsedTime / 60f);
        int remainingSeconds = Mathf.FloorToInt(elapsedTime % 60f);
        //if it's RLS then add / MaxTime in format that's only in minutes
        if (SceneManager.GetActiveScene().name == "PlayRLS")
        {
            txtTimer.text = string.Format("{0:00}:{1:00} / {2:00}", minutes, seconds, MaxTime / 60);
            txtRemainer.text = string.Format("{0:00}:{1:00}", remainingMinutes, remainingSeconds);
        }
        else
        {
            txtTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!countdownEnd)
        {
            countDownTimer -= Time.deltaTime;
            txtCountdown.text = Mathf.Ceil(countDownTimer).ToString();
            if (countDownTimer <= 0)
            {
                countdownEnd = true;
                txtCountdown.enabled = false;
                cover.enabled = false;
                runLevel();
            }
        }
        else
        {
            if (!paused)
            {
                elapsedTime += Time.deltaTime;
            }
            float currentTime = timerDuration - elapsedTime;
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            int remainingMinutes = Mathf.FloorToInt(elapsedTime / 60f);
            int remainingSeconds = Mathf.FloorToInt(elapsedTime % 60f);
            //if it's RLS then add / MaxTime in format that's only in minutes
            if (SceneManager.GetActiveScene().name == "PlayRLS")
            {
                txtTimer.text = string.Format("{0:00}:{1:00} / {2:00}", minutes, seconds, MaxTime / 60);
                txtRemainer.text = string.Format("{0:00}:{1:00}", remainingMinutes, remainingSeconds);
            }
            else
            {
                txtTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

            if (currentTime <= 0)
            {
                timerEnd();
            }
        }

    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveTimer();
        }
        else
        {
            LoadTimer();
        }
    }

    void OnApplicationQuit()
    {
        SaveTimer();
    }

    private void SaveTimer()
    {
        savedTime = Time.realtimeSinceStartup;
        PlayerPrefs.SetFloat("SavedTime", savedTime);
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.Save();
    }

    private void LoadTimer()
    {
        if (PlayerPrefs.HasKey("SavedTime"))
        {
            savedTime = PlayerPrefs.GetFloat("SavedTime");
            elapsedTime = PlayerPrefs.GetFloat("ElapsedTime");
            float delta = Time.realtimeSinceStartup - savedTime;
            elapsedTime += delta;
            PlayerPrefs.DeleteKey("SavedTime");
            PlayerPrefs.DeleteKey("ElapsedTime");
            PlayerPrefs.Save();
        }
    }

    public void targetMedal()
    {
        if (!paused)
        {
            if (SceneManager.GetActiveScene().name == "PlayRLS")
            {
                if (timerDuration - elapsedTime + 60 < MaxTime)
                {
                    timerDuration += 60;
                }
                else if(timerDuration - elapsedTime > MaxTime)
                {
                }
                else
                {
                    timerDuration += MaxTime - (timerDuration - elapsedTime) + 1;
                }
            }
            var entry = new RunHistoryEntry
            {
                LevelLink = levelLink,
                LevelAuthor = author,
                LevelName = levelName,
                TargetMedal = true,
                skipped = false,
                OneDownMedal = false
            };
            runHistory.Add(entry);
            huntedMedalCount++;
            huntedMedalCountText.text = huntedMedalCount.ToString();
            runLevel();
        }
    }

    public void oneDownMedal()
    {
        if (!paused)
        {
            var entry = new RunHistoryEntry
            {
                LevelLink = levelLink,
                LevelName = levelName,
                TargetMedal = false,
                skipped = false,
                OneDownMedal = true
            };
            runHistory.Add(entry);
            oneDownMedalCount++;
            oneDownMedalCountText.text = oneDownMedalCount.ToString();
            runLevel();
        }

    }
    public void runLevel(int mode = 0)
    {
        if (mode == 0)
        {
            StartCoroutine(FetchAndRunLevel());
        }
        else
        {
            Application.OpenURL(levelLink);
        }

    }

    private IEnumerator FetchAndRunLevel()
    {
        // Start fetching the next level asynchronously
        yield return StartCoroutine(getNextLevel());

        // Now that levelLink, levelName, and author are updated, open the level link
        Application.OpenURL(levelLink);
    }

    public void skipLevel()
    {
        if (!paused)
        {
            if (SceneManager.GetActiveScene().name == "PlayRLS")
            {
                if (MaxTime > 60)
                {
                    MaxTime -= 60;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (skips <= 0)
                {
                    return;
                }
            }
            var entry = new RunHistoryEntry
            {
                LevelLink = levelLink,
                LevelAuthor = author,
                LevelName = levelName,
                TargetMedal = false,
                skipped = true,
                OneDownMedal = false
            };
            runHistory.Add(entry);
            runLevel();
            skips--;
            if (SceneManager.GetActiveScene().name == "PlayRLC")
            {
                txtSkips.text = "SKIP (" + skips.ToString() + ")";
            }
            if (skips <= 0)
            {
                SkipButton.GetComponent<Image>().color = new Color32(113, 55, 241, 100);
            }
        }

    }

    public void brokenMap()
    {
        if (!paused)
        {
            var entry = new RunHistoryEntry
            {
                LevelLink = levelLink,
                LevelAuthor = author,
                TargetMedal = false,
                skipped = true,
                OneDownMedal = false
            };
            runHistory.Add(entry);
            runLevel();
        }

    }

    public void pauseTimer()
    {
        paused = !paused;
        PauseButton.GetComponentInChildren<TextMeshProUGUI>().text = paused ? "Resume" : "Pause";
        //make button little darker when paused
        if (paused)
        {
            PauseButton.GetComponent<Image>().color = new Color32(113, 55, 241, 100);
        }
        else
        {
            PauseButton.GetComponent<Image>().color = new Color32(113, 55, 241, 255);
        }
        //SaveTimer();
    }

    public void resumeTimer()
    {
        LoadTimer();
    }

    public void rejoinLevel()
    {
        if(!paused)
        {
            runLevel(1);
        }
    }

    public void timerEnd()
    {
        //SaveTimer();
        PlayerPrefs.SetString("RunHistory", Newtonsoft.Json.JsonConvert.SerializeObject(runHistory));
        if (SceneManager.GetActiveScene().name == "PlayRLS")
        {
            PlayerPrefs.SetFloat("TotalTimeSurvived", elapsedTime);
        }
        Debug.Log("Total time: " + elapsedTime);
        Debug.Log("saved time" + PlayerPrefs.GetFloat("TotalTimeSurvived"));
        txtLevelName.text = Newtonsoft.Json.JsonConvert.SerializeObject(runHistory);
        SceneManager.LoadScene("Results");
    }

    /*public string getNextLevel()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://exostats.nl/?api&randomlevellink&minAT=1000&maxAT=30000&minGold=1000&maxGold=30000");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            string jsonResponse = reader.ReadToEnd();
            JObject responseObject = JObject.Parse(jsonResponse);

            string link = (string)responseObject["link"];
            levelName = (string)responseObject["name"];
            author = (string)responseObject["authorName"];
            return link;
        }

    }*/
    public IEnumerator getNextLevel()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("https://exostats.nl/?api&randomlevellink&minAT=1000&maxAT=30000&minGold=1000&maxGold=30000");
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            string jsonResponse = uwr.downloadHandler.text;
            JObject responseObject = JObject.Parse(jsonResponse);

            // Extract level data from the JSON response
            levelLink = (string)responseObject["link"];
            levelName = (string)responseObject["name"];
            author = (string)responseObject["authorName"];

            // Update the UI with the level information
            txtLevelName.text = levelName;
            txtAuthor.text = author;

            Debug.Log("Level Link: " + levelLink);
            Debug.Log("Level Name: " + levelName);
            Debug.Log("Author: " + author);
        }
    }
    public void mainMenu()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadScene("Menu");
    }
}
