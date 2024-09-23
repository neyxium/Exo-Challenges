using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuButtons : MonoBehaviour
{
    private int rulesNumber = 0;
    public Button LeftButton;
    public Button RightButton;
    public TextMeshProUGUI txtDescription;
    public TextMeshProUGUI txtTitle;

    private string[] titles = new string[] {"Selection", "Gameplay", "Skipping", "Survival", "Leaderboard"};
    private string rule1 = "To choose a medal to target, click on the desired medal. If you want to change your selection, simply click on a different medal. Next, use the slider to select how much time you want to play; the time will be displayed in minutes. \n \n Both the target medal and time have default values indicated next to them.";
    private string rule2 = "After the countdown ends, a random level will be selected, and the app will automatically transfer you to Exoracer. Your goal is to earn the medal you initially selected. Once you achieve that medal, quickly return to this app and click on the selected medal to receive your next random level. \n \n If you don't succeed in earning the selected medal, you can still proceed by earning the medal one level below it. In this case, return to the app and select the lower medal, which will still provide you with the next level to play. \n \n Try to earn as many target medals as possible within the selected time.";
    private string rule3 = "If you don't want to play a level or if it's too difficult to earn the target or lower medal, you can skip the level to receive the next one. However, in RLC mode, you can only skip up to 5 times. In RLS mode, you can skip as many times as you want, but each skip will reduce your remaining time by 1 minute until it reaches a minimum of 1 minute. \n \n There is also button to skip broken map. You can only use that button in case level doesn't have any finishes or is impossible to finish or get target medal.";
    private string rule4 = "In RLS mode, you start with less time than in RLC mode, but for each map you complete, you earn an extra minute. However, for every skip, you lose 1 minute from your max time, and you cannot exceed the original max time. \n \n If you earn one medal lower than your target, you get a free pass to continue. To proceed, click on the lower medal instead of the target medal. Try to survive as long as possible and collect as many target medals as you can.";
    private string rule5 = "Currently there will not be leaderboard on website but I will try my best to keep leaderboard updated in discord. In future you can expect leaderboard on website. To submit your run for the leaderboard, contact me on Discord (Neyxium). Please note that all WR (World Record) runs require video proof. Easiest way is to upload to youtube and send me link (yt video can be unlisted but not private)";

    public void RLC(){
        SceneManager.LoadScene("RLC");
    }
    public void RLS(){
        SceneManager.LoadScene("RLS");
    }
    public void rules(){
        SceneManager.LoadScene("Rules");
    }
    public void about(){
        SceneManager.LoadScene("About");
    }
    public void mainMenu(){
        Debug.Log("Main Menu");
        SceneManager.LoadScene("Menu");
    }

    public void changeRules(bool right){
        if(right){
            rulesNumber++;
            if(rulesNumber >= 4){
                rulesNumber = 4;
            }
            else{
            }
        }else{
            rulesNumber--;
            if(rulesNumber <= 0){
                rulesNumber = 0;
            }
            else{
            }
        }
        switcher();
    }

    private void switcher(){
        switch(rulesNumber){
            case 0:
                LeftButton.interactable = false;
                RightButton.interactable = true;
                txtTitle.text = titles[rulesNumber];
                txtDescription.text = rule1;
                break;
            case 1:
                LeftButton.interactable = true;
                RightButton.interactable = true;
                txtTitle.text = titles[rulesNumber];
                txtDescription.text = rule2;
                break;
            case 2:
                LeftButton.interactable = true;
                RightButton.interactable = true;
                txtTitle.text = titles[rulesNumber];
                txtDescription.text = rule3;
                break;
            case 3:
                LeftButton.interactable = true;
                RightButton.interactable = true;
                txtTitle.text = titles[rulesNumber];
                txtDescription.text = rule4;
                break;
            case 4:
                LeftButton.interactable = true;
                RightButton.interactable = false;
                txtTitle.text = titles[rulesNumber];
                txtDescription.text = rule5;
                break;
        }
    }

}
