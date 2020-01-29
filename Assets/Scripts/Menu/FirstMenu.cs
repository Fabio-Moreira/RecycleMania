using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMenu : MonoBehaviour
{
    private FirstScreen firstScreen;
    public Button LoginButton,RegisterButton,AboutButton,PlayButton,CreditsButton;

    void Start()
    {
        LoginButton.onClick.AddListener(openLogin);
        RegisterButton.onClick.AddListener(openRegister);
        AboutButton.onClick.AddListener(openAbout);
        PlayButton.onClick.AddListener(openOffline);
        CreditsButton.onClick.AddListener(openCredits);
    }

    public void setFirstScreen(FirstScreen firstScreen){
        this.firstScreen = firstScreen;
    }

    private void openLogin(){
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.Login);
    }

    private void openRegister(){
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.Register);
    }

    private void openAbout(){
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.About);
    }

    private void openOffline(){
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.Offline);
    }

    private void openCredits(){
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.Credits);
    }
}
