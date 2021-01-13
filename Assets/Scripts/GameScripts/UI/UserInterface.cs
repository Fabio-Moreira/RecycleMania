using UnityEngine;
using UnityEngine.UI;

namespace GameScripts
{
    public class UserInterface : MonoBehaviour, IObserver<string,int>
    {
        [Header("Scripts")] [SerializeField] [Tooltip("Needed to pause game and get points")]
        private Game game;

        [Header("UI Text components")] [SerializeField] [Tooltip("Points text")]
        private Text pointsText;

        [SerializeField] [Tooltip("Points text")]
        private Text livesText;

        [Header("Control buttons")] [SerializeField] [Tooltip("Button to open pause menu")]
        private Button pauseButton;
        [SerializeField, Tooltip("Pause panel needed to display and hide it")]
        private GameObject pauseMenu;
        private void Awake()
        {
            pauseButton.onClick.AddListener(PauseControl);
            game.Subscribe(this);
        }

        private void PauseControl()
        {
            if (game.Paused)
            {
                pauseMenu.SetActive(false);
                game.ContinueGame();
            }
            else
            {
                pauseMenu.SetActive(true);
                game.PauseGame();
            }
        }

        public void UpdateObserver(string param, int value)
        {
            switch (param)
            {
                case "Points":
                    pointsText.text = "Points: " + value;
                    break;
                case "Lives":
                    livesText.text = "Lives: " + value;
                    break;
            }
        }
    }
}