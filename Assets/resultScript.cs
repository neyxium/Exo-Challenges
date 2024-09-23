using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class resultScript : MonoBehaviour
{
    public GameObject textPrefab; // Dodeli v Unity Editorju na TextMeshProUGUI prefab
    public Transform content; // Dodeli v Unity Editorju na Content objekt od ScrollRect
    public TextMeshProUGUI txtTargetMedalCount;
    public TextMeshProUGUI txtOneDownMedalCount;
    public float elementHeight = 250f; // Višina vsakega elementa
    public float spacing = 20f; // Razdalja med elementi
    public TextMeshProUGUI txtTimer;

    void Start()
    {
        Debug.Log("Total time: " + PlayerPrefs.GetFloat("TotalTimeSurvived"));
        if(PlayerPrefs.HasKey("TotalTimeSurvived") && PlayerPrefs.GetFloat("TotalTimeSurvived") > 1)
        {
            
            float totalTime = PlayerPrefs.GetFloat("TotalTimeSurvived");
            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);
            txtTimer.text = "Total Time Survived: " + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else
        {
            txtTimer.text = "";
        }
        // Testni JSON niz za namene testiranja
        /*string testJson = @"
        [
            {
            ""LevelLink"": ""https://example.com/level1"",
            ""LevelName"": ""Sample Level 1"",
            ""LevelAuthor"": ""Author 1"",
            ""TargetMedal"": true,
            ""OneDownMedal"": false,
            ""skipped"": false
            },
            {
            ""LevelLink"": ""https://example.com/level2"",
            ""LevelName"": ""Sample Level 2"",
            ""LevelAuthor"": ""Author 2"",
            ""TargetMedal"": false,
            ""OneDownMedal"": false,
            ""skipped"": true
            },
            {
            ""LevelLink"": ""https://example.com/level3"",
            ""LevelName"": ""Sample Level 3"",
            ""LevelAuthor"": ""Author 3"",
            ""TargetMedal"": true,
            ""OneDownMedal"": false,
            ""skipped"": false
            },
            {
            ""LevelLink"": ""https://example.com/level4"",
            ""LevelName"": ""Sample Level 4"",
            ""LevelAuthor"": ""Author 4"",
            ""TargetMedal"": false,
            ""OneDownMedal"": true,
            ""skipped"": false
            },
            {
            ""LevelLink"": ""https://example.com/level5"",
            ""LevelName"": ""Sample Level 5"",
            ""LevelAuthor"": ""Author 5"",
            ""TargetMedal"": true,
            ""OneDownMedal"": false,
            ""skipped"": true
            },
            {
            ""LevelLink"": ""https://example.com/level6"",
            ""LevelName"": ""Sample Level 6"",
            ""LevelAuthor"": ""Author 6"",
            ""TargetMedal"": false,
            ""OneDownMedal"": true,
            ""skipped"": false
            },
            {
            ""LevelLink"": ""https://example.com/level7"",
            ""LevelName"": ""Sample Level 7"",
            ""LevelAuthor"": ""Author 7"",
            ""TargetMedal"": true,
            ""OneDownMedal"": false,
            ""skipped"": false
            },
            {
            ""LevelLink"": ""https://example.com/level8"",
            ""LevelName"": ""Sample Level 8"",
            ""LevelAuthor"": ""Author 8"",
            ""TargetMedal"": false,
            ""OneDownMedal"": false,
            ""skipped"": true
            }

        ]";
        PlayerPrefs.SetString("RunHistory", testJson);
        PlayerPrefs.Save();
*/
        string json = PlayerPrefs.GetString("RunHistory", "[]"); // Privzeto na prazno polje, če ni najdeno

        // Deserializiraj JSON niz nazaj v seznam objektov RunHistoryEntry
        List<RunHistoryEntry> runHistory = JsonConvert.DeserializeObject<List<RunHistoryEntry>>(json);

        // Počisti obstoječe otroke v content
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Loop skozi run history vnose
        foreach (var entry in runHistory)
        {
            // Ustvari nov TextMeshProUGUI za besedilo vnosa
            GameObject newTextObj = Instantiate(textPrefab, content);
            TextMeshProUGUI entryText = newTextObj.GetComponent<TextMeshProUGUI>();

            // Nastavi besedilo za prikaz podrobnosti vnosa run history
            entryText.text = $"Level: {entry.LevelName} \n - By {entry.LevelAuthor}\n - {(entry.skipped ? "Skipped" : "Not Skipped")}\n";

            // Pridobi gumb iz prefaba in nastavi funkcijo
            Button entryButton = newTextObj.GetComponentInChildren<Button>();
            entryButton.onClick.AddListener(() => OnEntryButtonClick(entry));

            // Set the color based on the entry properties
            if (entry.skipped)
            {
                entryText.color = new Color(1f, 0.5f, 0.5f);
            }
            else if (entry.TargetMedal)
            {
                entryText.color = new Color(0f, 1f, 0f);
            }
            else
            {
                entryText.color = Color.white;
            }
        }

        

        // Prilagodi velikost vsebine glede na število vnosov
        AdjustContentSize(runHistory.Count);

        // Prikaz števila medalj
        int targetMedalCount = runHistory.FindAll(entry => entry.TargetMedal).Count;
        int oneDownMedalCount = runHistory.FindAll(entry => entry.OneDownMedal).Count;

        txtTargetMedalCount.text = $"Target Medal: {targetMedalCount}";
        txtOneDownMedalCount.text = $"One Down Medal: {oneDownMedalCount}";


    }

    void AdjustContentSize(int entryCount)
    {
        RectTransform contentRect = content.GetComponent<RectTransform>();
        float newHeight = (entryCount * (elementHeight + spacing)) + spacing; // Dodaj dodatni spacing za spodnji rob
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);
    }

    void OnEntryButtonClick(RunHistoryEntry entry)
    {
        // Logika za obdelavo klika na gumb
       Application.OpenURL(entry.LevelLink);
        // Tukaj lahko dodaš dodatno logiko, kot je odpiranje povezave ali prikaz dodatnih informacij.
    }
}
