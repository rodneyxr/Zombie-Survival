using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

    public GameEngine gameEngine;
    public AudioSource waveComplete;
    public AudioSource[] keyStrokes;

    public Player player;
    public GameObject[] zombies;
    public Transform[] barricades;
    public float spawnRadius = 50f;
    public int entrances = 5;

    public int initialNumberOfZombies = 5;
    public float initialZombieSpeed = 1.5f;
    public float spawnInterval = 20f;
    public int hordeSize = 15;
    private int wave;
    private int zombiesLeft;
    private float zombieSpeed;

    public void StartWave(int wave) {
        this.wave = wave;
        zombiesLeft = (int)(initialNumberOfZombies * Mathf.Pow(1.5f, (float)wave - 1));
        zombieSpeed = initialZombieSpeed * Mathf.Pow(1.2f, (float)wave - 1);
        StartCoroutine(DelayedWaveChange());
        StartCoroutine(SpawnZombies(zombiesLeft));
    }

    public void InstantSpawnZombies(int amount) {
        for (int i = 0; i < amount; i++) {
            SpawnZombie();
        }
    }

    IEnumerator SpawnZombies(int amount) {
        int i = 0;
        while (i < amount) {
            int horde = Mathf.Min(hordeSize, amount - i);
            InstantSpawnZombies(horde);
            i += horde;
            if (i == amount) break;
            yield return new WaitForSeconds(spawnInterval);
        }
        yield return null;
    }

    public void SpawnZombie() {
        AI zombie = (GameObject.Instantiate(zombies[Random.Range(0, zombies.Length)], RandomOnUnitCircle(spawnRadius), Quaternion.identity) as GameObject).GetComponent<AI>();
        zombie.Init(player, barricades[Random.Range(0, entrances)], Random.Range(initialZombieSpeed, zombieSpeed));
    }

    public static Vector3 RandomOnUnitCircle(float radius) {
        Vector2 randomPointOnCircle = Random.insideUnitCircle;
        randomPointOnCircle.Normalize();
        randomPointOnCircle *= radius;
        return new Vector3(randomPointOnCircle.x, 0, randomPointOnCircle.y);
    }

    public void ZombieDied() {
        zombiesLeft--;
        HUD.ZombiesLeft = "Zombies Left: " + zombiesLeft;
        if (zombiesLeft == 0) {
            waveComplete.Play();
            gameEngine.EndWave();
        }
    }

    public void GameChanger() {
        entrances = barricades.Length;
    }

    IEnumerator DelayedWaveChange() {
        HUD.Wave = "";
        HUD.ZombiesLeft = "";
        string waveString = "Wave: " + wave;
        string tmp = "";
        foreach (char c in waveString) {
            keyStrokes[Random.Range(0, keyStrokes.Length - 1)].Play();
            tmp += c;
            HUD.Wave = tmp;
            yield return StartCoroutine(Wait(Random.Range(0f, .4f)));
        }
        string zombiesLeftString = "Zombies Left: " + zombiesLeft;
        tmp = "";
        foreach (char c in zombiesLeftString) {
            keyStrokes[Random.Range(0, keyStrokes.Length - 1)].Play();
            tmp += c;
            HUD.ZombiesLeft = tmp;
            yield return StartCoroutine(Wait(Random.Range(0f, .4f)));
        }
    }

    IEnumerator Wait(float duration) {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }
}
