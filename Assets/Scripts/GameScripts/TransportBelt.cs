using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class TransportBelt : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField, Tooltip("Transport belt prefabs to it look different each play through")]
        private GameObject transportBeltPrefab;
        [SerializeField, Tooltip("Sprites to change when the belt piece is moved up, makes a better illusion of movement")]
        private Sprite[] beltSprites;
        private readonly LinkedList<Rigidbody2D> rb2dTransportBelts = new LinkedList<Rigidbody2D>();
        private readonly LinkedList<SpriteRenderer> spriteRendererTransportBelts = new LinkedList<SpriteRenderer>();
        private int nextRandomColouredBelt=0;

        
        [Header("Speed settings")] [SerializeField] [Tooltip("Speed at which candy falls at the start of the game")]
        private float startSpeed;

        [SerializeField] [Tooltip("Value at which speed increases each speedIncreaseTick")]
        private float speedIncreaseSteps;

        [SerializeField] [Tooltip("Time between increase in speed")]
        private float speedIncreaseTick;

        [SerializeField, Tooltip("Max falling speed of the candy can reach")]
        private float maximumFallSpeed;

        private float currentIncreaseTick;
        private float currentSpeed;

        private void Start()
        {
            currentSpeed = -startSpeed;
            currentIncreaseTick = speedIncreaseTick;
        }

        public void SetupTransportBelt()
        {
            foreach (var variable in rb2dTransportBelts)
            {
                Destroy(variable.gameObject);
            }
            rb2dTransportBelts.Clear();
            spriteRendererTransportBelts.Clear();
                
            currentSpeed = -startSpeed;
            currentIncreaseTick = speedIncreaseTick;
            
            for (var i=0; i<10; i++)
            {
                var go = Instantiate(transportBeltPrefab, new Vector3(0, -4.9f + i * 1.13f, 0), Quaternion.identity);
                go.transform.parent = gameObject.transform;
                rb2dTransportBelts.AddLast(go.GetComponent<Rigidbody2D>());
                spriteRendererTransportBelts.AddLast(go.GetComponent<SpriteRenderer>());
            }
            
        }

        private void Update()
        {
            IncreaseSpeed();
            MoveBelt();
        }

        private void MoveBelt()
        {
            if (rb2dTransportBelts.First.Value.position.y <= -5f)
            {
                var toMoveUpRigidbody2D = rb2dTransportBelts.First.Value;
                var toMoveUpSpriteRenderer = spriteRendererTransportBelts.First.Value;
                rb2dTransportBelts.RemoveFirst();
                spriteRendererTransportBelts.RemoveFirst();

                var random = 12;
                if (nextRandomColouredBelt-- == 0)
                {
                    nextRandomColouredBelt = Random.Range(3, 10);
                    random = Random.Range(0, 11);
                }
                
                var posLast = rb2dTransportBelts.Last.Value.position.y;
                toMoveUpRigidbody2D.position = new Vector3(0, posLast + 1.13f, 0);
                toMoveUpSpriteRenderer.sprite = beltSprites[random];
                rb2dTransportBelts.AddLast(toMoveUpRigidbody2D);
                spriteRendererTransportBelts.AddLast(toMoveUpSpriteRenderer);
            }
            
            var tmpVel = new Vector2(0, currentSpeed);
            foreach (var tmp in rb2dTransportBelts)
            {
                tmp.velocity = tmpVel;
            }
        }
        

        private void IncreaseSpeed()
        {
            if ((currentIncreaseTick -= Time.deltaTime) <= 0 && currentSpeed >= -maximumFallSpeed)
            {
                currentIncreaseTick += speedIncreaseTick;
                currentSpeed -= speedIncreaseSteps;
            }
        }

        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }
    }
}