using UnityEngine;
public enum MenuOption
{
    Register,
    Login,
    Offline,
    About,
    First,
    Credits
}

public class MenuOptions
{
    private FirstScreen firstScreen;
    private GameObject currentMenu;
    public MenuOption menuOption = MenuOption.Login;
    public GameObject noInternet;

    public MenuOptions(FirstScreen firstScreen){
        this.firstScreen = firstScreen;
        this.currentMenu = firstScreen.FirstMenu;

        //Sets the attribute FirstScreen in each menu.
        firstScreen.LoginMenu.GetComponent<LoginMenu>().setFirstScreen(firstScreen);
        firstScreen.RegisterMenu.GetComponent<RegisterMenu>().setFirstScreen(firstScreen);
        firstScreen.AboutMenu.GetComponent<AboutMenu>().setFirstScreen(firstScreen);
        firstScreen.OfflineMenu.GetComponent<OfflineMenu>().setFirstScreen(firstScreen);
        firstScreen.FirstMenu.GetComponent<FirstMenu>().setFirstScreen(firstScreen);
        firstScreen.CreditsMenu.GetComponent<AboutMenu>().setFirstScreen(firstScreen);
    }

    public void ChangeMenuOption(MenuOption menuOption)
    {
        this.menuOption = menuOption;
        //if there is a currentMenu active it will deactivate it.
        if(currentMenu!=null){
            currentMenu.SetActive(false);
        }
        //will switch current menu to the desired one.
        switch(menuOption){
            case MenuOption.Login:
                currentMenu = firstScreen.LoginMenu;
                break;
            case MenuOption.Register:
                currentMenu = firstScreen.RegisterMenu;
                break;
            case MenuOption.Offline:
                currentMenu = firstScreen.OfflineMenu;
                break;
            case MenuOption.About:
                currentMenu = firstScreen.AboutMenu;
                break;
            case MenuOption.First:
                currentMenu = firstScreen.FirstMenu;
                break;
            case MenuOption.Credits:
                currentMenu = firstScreen.CreditsMenu;
                break;

        }
        //will set the new current menu to active.
        currentMenu.SetActive(true);
    }
}
