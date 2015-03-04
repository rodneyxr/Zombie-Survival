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
    private int entrances = 4;

    public int initialNumberOfZombies = 6;
    public float initialZombieSpeed = .5f;
    private int wave;
    private int zombiesLeft;
    private float zombieSpeed;

    //void Update() {

    //}

    public void StartWave(int wave) {
        this.wave = wave;
        zombiesLeft = (int)(initialNumberOfZombies * Mathf.Pow(1.5f, (float)wave - 1));
        zombieSpeed = initialZombieSpeed * Mathf.Pow(1.2f, (float)wave - 1);
        print(zombieSpeed);
        StartCoroutine(DelayedWaveChange());
        SpawnZombies(zombiesLeft);
    }

    public void SpawnZombies(int amount) {
        for (int i = 0; i < amount; i++) {
            SpawnZombie();
        }
    }

    public void SpawnZombie() {
        AI zombie = (GameObject.Instantiate(zombies[Random.Range(0, zombies.Length - 1)], RandomOnUnitCircle(spawnRadius), Quaternion.identity) as GameObject).GetComponent<AI>();
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
        entrances = barricades.Length - 1;
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
