using UnityEngine;
using UnityEngine.UI;
public class AboutMenu : MonoBehaviour
{
    private FirstScreen firstScreen;

    public Button BackButton;

    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(openFirstMenu);
    }


    public void setFirstScreen(FirstScreen firstScreen){
        this.firstScreen = firstScreen;
    }

    public void openFirstMenu(){
        firstScreen.menuOptions.ChangeMenuOption(MenuOption.First);
    }
}
