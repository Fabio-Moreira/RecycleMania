using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using GameScripts.UI;
using UnityEngine;

namespace GameScripts
{
    public class Game : MonoBehaviour
    {
        [Header("GameObjects")] [SerializeField] [Tooltip("Spawner needs game for speed")]
        private Spawner spawner;

        [SerializeField] [Tooltip("Bins needs game to add points and remove lives")]
        private Bins bins;

        [SerializeField] [Tooltip("Transport belt is responsible for the speed at which candy falls")]
        public TransportBelt transportBelt;

        [SerializeField] [Tooltip("UserInterface needed to update points, lives and to pause the game")]
        private UserInterface userInterface;

        [SerializeField] [Tooltip("Needed to remove PauseMenu and reset the game")]
        private PauseMenu pauseMenu;

        //lives tracking of current game
        private int lives;
        //points tracking of current game
        private int points;

        //mainCamera needed for screenToWorldPoint for mouse input
        private Camera mainCamera;

        //Candy currently being controlled by the player
        private Rigidbody2D selectedCandy;
        //difference of MousePosition to selectedCandy, avoid the candy getting centered to mouse and unexpected candy movements
        private Vector2 deltaMouseCandy;

        //needed to avoid input while game is paused
        private bool Paused { set; get; }

        private static Game _instance;
        
        // Start is called before the first frame update
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            mainCamera = Camera.main;
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {
            CandyMoving();
        }
        
        private void CandyMoving()
        {
            if (Paused) return;
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 v2 = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.CircleCast(v2, 0.3f, Vector2.zero);
                if (!hit.collider || hit.collider.gameObject.layer != 8) return;
                selectedCandy = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                deltaMouseCandy = v2 - selectedCandy.position;
                selectedCandy.velocity = Vector2.zero;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!selectedCandy) return;
                selectedCandy.velocity = new Vector2(0, transportBelt.GetCurrentSpeed());
                selectedCandy = null;
            }
            else
            {
                if (!selectedCandy) return;
                Vector2 v2 = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                selectedCandy.gameObject.transform.position = new Vector3(v2.x-deltaMouseCandy.x, v2.y-deltaMouseCandy.y, 0);
            }
        }

        public void AddPoints()
        {
            userInterface.UpdatePoints(++points);
        }

        public void LoseLive()
        {
            if (--lives == 0)
            {
                Time.timeScale = 0;
                pauseMenu.SetResetMenu();
            }
            userInterface.UpdateLives(lives);
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Paused = true;
        }

        public void ContinueGame()
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            Paused = false;
        }

        public void StartGame()
        {
            points = 0;
            userInterface.UpdatePoints(points);
            lives = 3;
            userInterface.UpdateLives(lives);
            Paused = false;
            Time.timeScale = 1;
            transportBelt.SetupTransportBelt();
            spawner.SetupSpawner();
            bins.SetupBins();
        }

        public static Game GetInstance()
        {
            return _instance;
        }
    }
}