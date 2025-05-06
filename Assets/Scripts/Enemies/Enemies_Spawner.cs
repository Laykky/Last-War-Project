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
    public float enemySpeed = 5f; // Vitesse de déplacement des ennemis
    public float NewSpawnInterval = 10f; // Vitesse de déplacement des ennemis
    public int NewEnemiesAdd = 1; // Vitesse de déplacement des ennemis
    public int MaxEnnemies = 8; // Vitesse de déplacement des ennemis

    private float specialEnemySpawnTimer = 0f; // Temps écoulé depuis le dernier spawn spécial
    private float specialEnemySpawnDelay; // Délai aléatoire entre les apparitions des ennemis spéciaux
    private bool specialEnemyEnabled = false; // Permet d'empêcher le spawn des ennemis spéciaux au début

    void Start()
    {
        // Retarde l'activation du spawn des ennemis spéciaux 
        StartCoroutine(EnableSpecialEnemyAfterDelay(1f));
        StartCoroutine(SpawnEnemiesUpdate());
        StartCoroutine(SpawnEnemiesContinuously());
    }

    IEnumerator EnableSpecialEnemyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        specialEnemyEnabled = true; // Active le spawn des ennemis spéciaux après le délai
        specialEnemySpawnDelay = Random.Range(10f, 20f); // Génère un premier délai aléatoire
    }

    IEnumerator SpawnEnemiesContinuously()
    {
        while (true)
        {
            SpawnWave();
            yield return new WaitForSeconds(spawnInterval); // Attente avant la prochaine vague
        }
    }

    IEnumerator SpawnEnemiesUpdate()
    {
        while (enemiesPerWave < MaxEnnemies) // contrairement au if crée une boucle.
        { 

        yield return new WaitForSeconds(NewSpawnInterval);
        enemiesPerWave += NewEnemiesAdd;

        }
    }

    void SpawnWave()
    {
        int i = 0;
        bool specialEnemySpawned = false; // Vérifie si un ennemi spécial a déjà spawn

        while (i < enemiesPerWave)
        {
            GameObject enemy;
            Vector3 spawnPosition;

            // Vérifie si l'ennemi spécial doit apparaître (seulement après 20 secondes et un seul par ligne)
            if (specialEnemyEnabled && !specialEnemySpawned && specialEnemySpawnTimer >= specialEnemySpawnDelay && i <= enemiesPerWave - 3)
            {
                spawnPosition = spawnPoint.position + Vector3.right * (i * specialEnemySpacing) + Vector3.up * specialEnemyHeight;
                enemy = Instantiate(specialEnemyPrefab, spawnPosition, spawnPoint.rotation);
                StartCoroutine(MoveEnemy(enemy));
                specialEnemySpawned = true;
                specialEnemySpawnTimer = 0f; // Réinitialise le timer
                specialEnemySpawnDelay = Random.Range(10f, 20f); // Définir un nouveau délai aléatoire
                i += 3; // Il occupe l'espace de 3 ennemis normaux
            }
            else
            {
                spawnPosition = spawnPoint.position + Vector3.right * (i * spacing) + Vector3.up * normalEnemyHeight;
                enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);
                StartCoroutine(MoveEnemy(enemy));
                i++;
            }
        }

        specialEnemySpawnTimer += spawnInterval; // Mise à jour du timer pour le spawn de l'ennemi spécial
    }

    IEnumerator MoveEnemy(GameObject enemy)
    {
        while (enemy != null) // Vérifie que l'objet existe toujours
        {
            enemy.transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
            yield return null; // Attend la prochaine frame
        }
    }
}