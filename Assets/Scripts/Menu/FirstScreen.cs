using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class FirstScreen : MonoBehaviour
{
    private GameInstance instance;
    public MenuOptions menuOptions;

    public GameObject FirstMenu,RegisterMenu,LoginMenu,OfflineMenu,AboutMenu,CreditsMenu;

    public GameObject noInternet;

    private int ConnectionUpdateTick = 30;
    private bool changedOffline = false;
    private bool changedOnline = false;

    // Start is called before the first frame update
    void Start()
    {
        menuOptions = new MenuOptions(this);
        instance = GameInstance.getInstance();
        instance.LoadUserInformation();
        try{
            StartCoroutine(instance.Login());
        }catch(Exception ex){
            Debug.Log(ex.Message);
        }

        if (Application.internetReachability == NetworkReachability.NotReachable){
            menuOptions.ChangeMenuOption(MenuOption.Offline);
            noInternet.SetActive(true);
            changedOffline = true;
            changedOnline = false;
        }else{
            menuOptions.ChangeMenuOption(MenuOption.First);
            noInternet.SetActive(false);
            changedOffline = false;
            changedOnline = true;
        }
    }

    void FixedUpdate()
    {
        CheckInternetConnection();
    }

    void CheckInternetConnection(){
        if(--ConnectionUpdateTick == 0)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable){
                if(changedOffline==false){
                    menuOptions.ChangeMenuOption(MenuOption.Offline);
                    noInternet.SetActive(true);
                    changedOffline = true;
                    changedOnline = false;
                }
            }else{
                if(changedOnline == false){
                    menuOptions.ChangeMenuOption(MenuOption.First);
                    noInternet.SetActive(false);
                    changedOnline = true;
                    changedOffline = false;
                }
            }
            ConnectionUpdateTick = 30;
        }
    }
}
