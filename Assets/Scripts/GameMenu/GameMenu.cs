using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Text username,Highscore,Speed,BestWoMistake;
    public Button StartGameButton;

    // Start is called before the first frame update
    void Start()
    {
        username.text = GameInstance.getInstance().user.username;
        //Runs through the loop of Bins and Instantiates the bins into the right slots;
        Highscore.text = "Highscore: " + GameInstance.getInstance().achievements.highscore.ToString();
        Speed.text = "Fastest speed: " + GameInstance.getInstance().achievements.speed.ToString("0.00");
        BestWoMistake.text = "3lives Highscore: " + GameInstance.getInstance().achievements.woMistake.ToString();

        StartGameButton.onClick.AddListener(StartGame);
    }

    void StartGame(){
        SceneManager.LoadScene("Game",LoadSceneMode.Single);
    }

}
