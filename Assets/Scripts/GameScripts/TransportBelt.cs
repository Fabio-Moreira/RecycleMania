using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class TransportBelt : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField, Tooltip("Transport belt prefabs to it look different each play through")]
        private GameObject[] transportBeltPrefab;
        private LinkedList<Rigidbody2D> transportBelts = new LinkedList<Rigidbody2D>();
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
            foreach (var VARIABLE in transportBelts)
            {
                Destroy(VARIABLE.gameObject);
            }
            transportBelts.Clear();
                
            currentSpeed = -startSpeed;
            currentIncreaseTick = speedIncreaseTick;
            
            for (int i=0; i<10; i++)
            {
                var go = Instantiate(transportBeltPrefab[12], new Vector3(0, -4.9f + i * 1.13f, 0), Quaternion.identity).GetComponent<Rigidbody2D>();
                transportBelts.AddLast(go);
            }
            
        }

        private void Update()
        {
            IncreaseSpeed();
            MoveBelt();
        }

        private void MoveBelt()
        {
            if (transportBelts.First.Value.position.y <= -5f)
            {
                var firstTemp = transportBelts.First.Value.gameObject;
                transportBelts.RemoveFirst();
                Destroy(firstTemp);

                var random = 12;
                if (nextRandomColouredBelt-- == 0)
                {
                    nextRandomColouredBelt = Random.Range(3, 10);
                    random = Random.Range(0, 11);
                }
                
                var posLast = transportBelts.Last.Value.position.y;
                var go = Instantiate(transportBeltPrefab[random], new Vector3(0, posLast + 1.13f, 0), Quaternion.identity).GetComponent<Rigidbody2D>();
                transportBelts.AddLast(go);
            }
            
            var tmpVel = new Vector2(0, currentSpeed);
            foreach (var tmp in transportBelts)
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