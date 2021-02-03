using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScripts
{
    public class Spawner : MonoBehaviour
    {
        [Header("Settings")] [SerializeField, Tooltip("TickRate at which the game starts")]
        private float spawntick;
        [SerializeField, Tooltip("Rate at which the difficulty increases")]
        private float tickdifficultystep;
    
        [Header("GameObjects")]
        [SerializeField] private GameObject[] candyObjects;

        private float currentSpawnTick;
        private float currentTickDifficulty;

        private void Start()
        {
            SetupSpawner();
        }

        public void SetupSpawner()
        {
            currentSpawnTick = spawntick;
            currentTickDifficulty = 0;
            var candies = FindObjectsOfType<Candy>();
            foreach (var variable in candies)
            {
                Destroy(variable.gameObject);
            }
        }
    
        private void Update()
        {
            SpawnCandy();
        }

        private void SpawnCandy()
        {
            if ((currentSpawnTick -= Time.deltaTime) <= 0)
            {
                var pos = gameObject.transform.position;
                pos.x += Random.Range(-1.6f, 1.6f);
                pos.y += 0.5f;

                var type = Random.Range(0, candyObjects.Length);
                Instantiate(candyObjects[type], pos, Quaternion.identity);

                IncreaseDifficulty();
                currentSpawnTick = spawntick  - currentTickDifficulty;
            }
        }

        private void IncreaseDifficulty()
        {
            if (currentTickDifficulty + tickdifficultystep < 4f)
                currentTickDifficulty += tickdifficultystep;
        }
    }
}