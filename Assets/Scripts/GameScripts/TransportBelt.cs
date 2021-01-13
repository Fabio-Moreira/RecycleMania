using UnityEngine;

namespace GameScripts
{
    public class TransportBelt : MonoBehaviour
    {
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
            currentSpeed = -startSpeed;
            currentIncreaseTick = speedIncreaseTick;
        }

        private void Update()
        {
            IncreaseSpeed();
        }

        private void IncreaseSpeed()
        {
            if ((currentIncreaseTick -= Time.deltaTime) <= 0 && currentSpeed >= -maximumFallSpeed)
            {
                currentIncreaseTick += speedIncreaseTick;
                currentSpeed -= speedIncreaseSteps;
                Debug.Log(currentSpeed);
            }
        }

        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }
    }
}