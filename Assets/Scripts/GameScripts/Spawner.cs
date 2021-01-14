using System;
using GameScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("Settings")] [SerializeField, Tooltip("TickRate at which the game starts")]
    private float _SPAWNTICK;
    [SerializeField, Tooltip("Rate at which the difficulty increases")]
    private float _TICKDIFFICULTYSTEP;
    
    [Header("GameObjects")]
    [SerializeField] private GameObject[] candyObjects;

    private float currentSpawnTick;
    private float currentTickDifficulty;
    public Game game { set; private get; }

    private void Start()
    {
        SetupSpawner();
    }

    public void SetupSpawner()
    {
        currentSpawnTick = _SPAWNTICK;
        currentTickDifficulty = 0;
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
            pos.y += 2f;

            var type = Random.Range(0, candyObjects.Length);

            var obj = Instantiate(candyObjects[type], pos, Quaternion.identity);
            var candy = obj.GetComponent<Candy>();
            candy.game = this.game;
            
            IncreaseDifficulty();
            currentSpawnTick = _SPAWNTICK  - currentTickDifficulty;
        }
    }

    private void IncreaseDifficulty()
    {
        if (currentTickDifficulty + _TICKDIFFICULTYSTEP < 4f)
            currentTickDifficulty += _TICKDIFFICULTYSTEP;
    }
}