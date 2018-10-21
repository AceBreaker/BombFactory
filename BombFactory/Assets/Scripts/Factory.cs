using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

    [SerializeField]
    GameObject[] spawnPoints = null;
    [SerializeField]
    Material[] bombMaterials = null;
    [SerializeField]
    GameObject bombPrefab = null;

    [SerializeField]
    float spawnBombFrequency = 0.0f;
    [SerializeField]
    int bombSpawnCount = 1;

    Timer spawnBombTimer = null;
    Timer increaseSpawnCountTimer = null;

    float time = 0.0f;
    [SerializeField]
    int increaseSpawnRate = 6;
    int wavesSpawned = 0;

	// Use this for initialization
	void Start () {
        spawnBombTimer = GetComponent<Timer>();
        spawnBombTimer.RegisterOnTick(SpawnBomb);
        spawnBombTimer.RegisterOnTick(IncreaseSpawnCount);
        spawnBombTimer.StartTimer();
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
	}

    void SpawnBomb()
    {
        if (ScoreKeeper.gameOver)
        {
            spawnBombTimer.StopTimer();
            return;
        }
        for (int i = 0; i < bombSpawnCount; i++)
        {
            int bombColor = Random.Range(0, bombMaterials.Length);
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject go = Instantiate(bombPrefab, spawnPoint.transform);

            Bomb newBomb = go.GetComponent<Bomb>();
            newBomb.SetBombMaterial(bombMaterials[bombColor]);
            newBomb.StartMoving();
            newBomb.SetMyMaterial(bombMaterials[bombColor]);
        }
    }

    void IncreaseSpawnCount()
    {
        wavesSpawned++;
        if(wavesSpawned % increaseSpawnRate == 0 && bombSpawnCount < 5)
        {
            bombSpawnCount++;
        }
    }
}
