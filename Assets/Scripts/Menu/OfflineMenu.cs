using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class OfflineMenu : MonoBehaviour
{
    private FirstScreen firstScreen;
    public InputField usernameInput;
    public Button EnterButton,BackButton;
    private OfflineManager offlineManager;

    // Start is called before the first frame update
    void Start()
    {
        offlineManager = new OfflineManager();
        offlineManager.loadPlayer();
        if(offlineManager.firstStart){
            EnterButton.onClick.AddListener(EnterNew);
        }else{
            usernameInput.text = offlineManager.username;
            usernameInput.interactable = false;
            EnterButton.onClick.AddListener(EnterCurrent);
        }
        BackButton.onClick.AddListener(goBack);
    }

    public void goBack(){
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.First);
    }

    public void EnterCurrent(){
        offlineManager.loadOffline();
        SceneManager.LoadScene("GameMenu",LoadSceneMode.Single);
    }

    public void EnterNew(){
        if(usernameInput.text.Length >= 5){
            offlineManager.createOffline(usernameInput.text);
            SceneManager.LoadScene("GameMenu",LoadSceneMode.Single);
        }
    }

    public void setFirstScreen(FirstScreen firstScreen){
        this.firstScreen = firstScreen;
    }
}
