using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Buttons")] 
        [SerializeField, Tooltip("button to continue the game when paused and not lost")]
        private Button continueButton;
        [SerializeField, Tooltip("button to restart the game when paused or the game is lost")]
        private Button restartButton;

        [SerializeField, Tooltip("needed to pause and restart the game")]
        private Game game;
        
        // Start is called before the first frame update
        private void Awake()
        {
            continueButton.onClick.AddListener(ContinueGame);
            restartButton.onClick.AddListener(RestartGame);
        }

        private void ContinueGame()
        {
            game.ContinueGame();
            gameObject.SetActive(false);
        }

        private void RestartGame()
        {
            game.StartGame();
            gameObject.SetActive(false);
            continueButton.interactable = true;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetResetMenu()
        {
            gameObject.SetActive(true);
            continueButton.interactable = false;
        }
    }
}

