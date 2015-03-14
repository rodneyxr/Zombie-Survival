using UnityEngine;
using System.Collections;

public class RandomPowerUps : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject[] powerUps;

    public float spawnInterval = 40f;
    public float intervalVariation = 15f;
    public int numberOfActiveSpawnPoints = 9;

    void Start() {
        Invoke("SpawnRandomPowerUp", GetRandomInterval());
    }

    private float GetRandomInterval() {
        return spawnInterval + (Random.Range(0, intervalVariation) * 2 - intervalVariation);
    }

    private void SpawnRandomPowerUp() {
        int index = Random.Range(0, numberOfActiveSpawnPoints);
        ItemDisplay itemDisplay = (Instantiate(Resources.Load("Prefabs/ItemDisplay"), spawnPoints[index].position, Quaternion.identity) as GameObject).GetComponent<ItemDisplay>();
        itemDisplay.Init(powerUps[Random.Range(0, powerUps.Length)], 30f, false);
        SFX.PlayPowerUpSound();
        Invoke("SpawnRandomPowerUp", GetRandomInterval());
    }

    public void GameChanger() {
        numberOfActiveSpawnPoints = spawnPoints.Length;
    }
}
