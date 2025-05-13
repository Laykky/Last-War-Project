using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Spawner : MonoBehaviour

{
    public GameObject enemyPrefab; // Prefab de l'ennemi classique
    public GameObject specialEnemyPrefab; // Prefab de l'ennemi spécial
    public Transform spawnPoint; // Point de référence du spawn

    public float normalEnemyHeight = 0f; // Hauteur de spawn des ennemis normaux
    public float specialEnemyHeight = 2f; // Hauteur de spawn des ennemis spéciaux
    public int enemiesPerWave = 8; // Nombre d'ennemis normaux par vague
    public float spacing = 2f; // Espacement entre les ennemis normaux
    public float specialEnemySpacing = 6f; // Espacement pour l'ennemi spécial
    public float spawnInterval = 1f; // Temps entre chaque vague
    public float NewSpawnInterval = 10f;
    public int NewEnemiesAdd = 1;
    public int MaxEnnemies = 8;

    [SerializeField] private float initialSpeed = 2f; // 🔹 Vitesse initiale des ennemis (modifiable)
    [SerializeField] private float maxSpeed = 8f; // 🔹 Vitesse maximale des ennemis (modifiable)
    [SerializeField] private float speedIncreaseRate = 0.1f; // 🔹 Vitesse de croissance (modifiable)

    private float enemySpeed; // 🔹 Stocke la vitesse actuelle des ennemis
    private float specialEnemySpawnTimer = 0f;
    private float specialEnemySpawnDelay;
    private bool specialEnemyEnabled = false;

    void Start()
    {
        enemySpeed = initialSpeed; // 🔹 Définit la vitesse de base des ennemis
        StartCoroutine(EnableSpecialEnemyAfterDelay(1f));
        StartCoroutine(SpawnEnemiesUpdate());
        StartCoroutine(SpawnEnemiesContinuously());
        StartCoroutine(IncreaseSpeedOverTime()); // 🔹 Commence l'augmentation progressive de la vitesse
    }

    IEnumerator EnableSpecialEnemyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        specialEnemyEnabled = true;
        specialEnemySpawnDelay = Random.Range(10f, 20f);
    }

    IEnumerator SpawnEnemiesContinuously()
    {
        while (true)
        {
            SpawnWave();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnEnemiesUpdate()
    {
        while (enemiesPerWave < MaxEnnemies)
        {
            yield return new WaitForSeconds(NewSpawnInterval);
            enemiesPerWave += NewEnemiesAdd;
        }
    }

    IEnumerator IncreaseSpeedOverTime()
    {
        while (enemySpeed < maxSpeed) // 🔹 Augmente progressivement mais ne dépasse pas maxSpeed
        {
            yield return new WaitForSeconds(5f);
            enemySpeed = Mathf.Min(enemySpeed + speedIncreaseRate, maxSpeed);
        }
    }

    void SpawnWave()
    {
        int i = 0;
        bool specialEnemySpawned = false;

        while (i < enemiesPerWave)
        {
            GameObject enemy;
            Vector3 spawnPosition;

            if (specialEnemyEnabled && !specialEnemySpawned && specialEnemySpawnTimer >= specialEnemySpawnDelay && i <= enemiesPerWave - 3)
            {
                spawnPosition = spawnPoint.position + Vector3.right * (i * specialEnemySpacing) + Vector3.up * specialEnemyHeight;
                enemy = Instantiate(specialEnemyPrefab, spawnPosition, spawnPoint.rotation);
                StartCoroutine(MoveEnemy(enemy));
                specialEnemySpawned = true;
                specialEnemySpawnTimer = 0f;
                specialEnemySpawnDelay = Random.Range(10f, 20f);
                i += 3;
            }
            else
            {
                spawnPosition = spawnPoint.position + Vector3.right * (i * spacing) + Vector3.up * normalEnemyHeight;
                enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);
                StartCoroutine(MoveEnemy(enemy));
                i++;
            }
        }

        specialEnemySpawnTimer += spawnInterval;
    }

    IEnumerator MoveEnemy(GameObject enemy)
    {
        while (enemy != null)
        {
            enemy.transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
            yield return null;
        }
    }
}