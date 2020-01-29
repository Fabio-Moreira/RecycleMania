using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoginMenu : MonoBehaviour
{
    private FirstScreen firstScreen;

    public InputField UsernameInput,PasswordInput;
    public Text ErrorMessage;
    public Button LoginButton,BackButton;
    
    void Start()
    {
        LoginButton.onClick.AddListener(Login);
        BackButton.onClick.AddListener(openFirstMenu);
    }

    public void setFirstScreen(FirstScreen firstScreen){
        this.firstScreen = firstScreen;
    }

    public void Login(){
        if(UsernameInput.text.Length >= 5){
            try{
                StartCoroutine(GameInstance.getInstance().Login(UsernameInput.text, PasswordInput.text));
            }catch(Exception ex){
                ErrorMessage.text = ex.Message.ToString();
                ErrorMessage.gameObject.SetActive(true);
            }
        }
    }

    public void openFirstMenu(){
        //Go back to the first menu
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.First);
    }
}
