using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Text username, Highscore, Speed, BestWoMistake;
    public Button StartGameButton;

    // Start is called before the first frame update
    private void Start()
    {
        StartGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}