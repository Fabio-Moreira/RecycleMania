using System;
using UnityEngine;

namespace GameScripts
{
    public class Game : MonoBehaviour, IObservable<string, int>
    {
        [Header("Starting Settings")]
        [SerializeField]
        [Tooltip("Speed at which the Candy falls at the start of the game")]
        private float speed;

        [SerializeField] [Tooltip("Time between increase in speed")]
        public int speedTick;

        [Header("GameObjects")] [SerializeField] [Tooltip("Spawner needs game for speed")]
        private Spawner spawner;

        [SerializeField] [Tooltip("Bins needs game to add points and remove lives")]
        private Bins bins;

        [SerializeField] [Tooltip("Transport belt is responsible for the speed at which candy falls")]
        public TransportBelt transportBelt;

        //Points tracking
        private int lives;

        private Camera mainCamera;

        private int points;

        //Candy currently being controlled by the player
        private Rigidbody2D selectedCandy;

        private IObserver<string, int> userInterface;

        public bool Paused { private set; get; }

        // Start is called before the first frame update
        private void Awake()
        {
            mainCamera = Camera.main;
            spawner.game = this;
            bins.game = this;
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {
            MouseInput();
        }

        private void FixedUpdate()
        {
            
        }

        public void Subscribe(IObserver<string, int> o)
        {
            userInterface = o;
        }

        public void Notify(string param, int value)
        {
            userInterface.UpdateObserver(param,value);
        }


        private void MouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = Input.mousePosition;

                Vector2 v2 = mainCamera.ScreenToWorldPoint(mousePos);
                var hit = Physics2D.CircleCast(v2, 0.3f, Vector2.zero);
                if (!hit.collider || hit.collider.gameObject.layer != 8) return;
                selectedCandy = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                selectedCandy.velocity = Vector2.zero;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!selectedCandy) return;
                Vector2 v2 = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                selectedCandy.velocity = new Vector2(0, transportBelt.GetCurrentSpeed());
                selectedCandy = null;
            }
            else
            {
                if (!selectedCandy) return;
                Vector2 v2 = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                selectedCandy.gameObject.transform.position = new Vector3(v2.x, v2.y, 0);
            }
        }

        public void AddPoints()
        {
            ++points;
            Notify("Points",points);
        }

        public void LoseLive()
        {
            --lives;
            Notify("Lives",lives);
        }

        public void PauseGame()
        {
            Paused = true;
            Time.timeScale = 0;
        }

        public void ContinueGame()
        {
            Paused = false;
            Time.timeScale = 1;
        }

        public void StartGame()
        {
            points = 0;
            Notify("Points",points);
            lives = 3;
            Notify("Lives",lives);
            Paused = false;
            Time.timeScale = 1;
            transportBelt.SetupTransportBelt();
            spawner.SetupSpawner();
            bins.SetupBins();
        }
    }
}