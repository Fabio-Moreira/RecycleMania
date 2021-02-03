using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class UserInterface : MonoBehaviour
    {
        [Header("Scripts")] [SerializeField] [Tooltip("Needed to pause game and get points")]
        private Game game;

        [Header("UI Text components")] [SerializeField] [Tooltip("Points text")]
        private Text pointsText;

        [SerializeField] [Tooltip("Points text")]
        private Text livesText;

        [Header("Control buttons")] [SerializeField] [Tooltip("Button to open pause menu")]
        private Button pauseButton;
        
        private void Awake()
        {
            pauseButton.onClick.AddListener(PauseControl);
        }

        private void PauseControl()
        {
            game.PauseGame();
        }

        public void UpdatePoints(int currentPoints)
        {
            pointsText.text = currentPoints.ToString();
        }

        public void UpdateLives(int currentLives)
        {
            livesText.text = currentLives.ToString();
        }
    }
}