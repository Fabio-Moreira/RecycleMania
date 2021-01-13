using UnityEngine;
using UnityEngine.UI;

namespace GameScripts
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
        void Start()
        {
            continueButton.onClick.AddListener(continueGame);
            restartButton.onClick.AddListener(restartGame);
        }

        private void continueGame()
        {
            game.ContinueGame();
            this.gameObject.SetActive(false);
        }

        private void restartGame()
        {
            game.StartGame();
            this.gameObject.SetActive(false);
        }
    }
}

