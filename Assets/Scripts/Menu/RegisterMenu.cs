using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class RegisterMenu : MonoBehaviour
{
    private FirstScreen firstScreen;
    public InputField Username,Password,PasswordConfirm;
    public Text ErrorMessage;
    public Button RegisterButton,BackButton;

    void Start()
    {
        RegisterButton.onClick.AddListener(Register);
        BackButton.onClick.AddListener(openFirstMenu);
    }

    public void setFirstScreen(FirstScreen firstScreen){
        this.firstScreen = firstScreen;
    }

    public void Register(){
        if(Username.text.Length >= 5 && Password.text == PasswordConfirm.text){
            try{
                StartCoroutine(GameInstance.getInstance().Register(Username.text, Password.text));
                StartCoroutine(GameInstance.getInstance().Login(Username.text, Password.text));
            }catch(Exception ex){
                ErrorMessage.text = ex.Message;
                ErrorMessage.gameObject.SetActive(true);
            }
        }else if(Password.text == PasswordConfirm.text){
            ErrorMessage.text = "Passwords don't match";
            ErrorMessage.gameObject.SetActive(true);
        }
    }

    public void NextScene(){
        SceneManager.LoadScene("GameMenu",LoadSceneMode.Single);
    }

    public void openFirstMenu(){
        //Go back to the first menu
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.First);
    }
}
