using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Spawner : MonoBehaviour

{
    public GameObject enemyPrefab; // Prefab de l'ennemi classique
    public GameObject specialEnemyPrefab; // Prefab de l'ennemi sp�cial
    public Transform spawnPoint; // Point de r�f�rence du spawn

    public float normalEnemyHeight = 0f; // Hauteur de spawn des ennemis normaux
    public float specialEnemyHeight = 2f; // Hauteur de spawn des ennemis sp�ciaux
    public int enemiesPerWave = 8; // Nombre d'ennemis normaux par vague
    public float spacing = 2f; // Espacement entre les ennemis normaux
    public float specialEnemySpacing = 6f; // Espacement pour l'ennemi sp�cial
    public float spawnInterval = 1f; // Temps entre chaque vague
    public float enemySpeed = 5f; // Vitesse de d�placement des ennemis
    public float NewSpawnInterval = 10f; // Vitesse de d�placement des ennemis
    public int NewEnemiesAdd = 1; // Vitesse de d�placement des ennemis
    public int MaxEnnemies = 8; // Vitesse de d�placement des ennemis

    private float specialEnemySpawnTimer = 0f; // Temps �coul� depuis le dernier spawn sp�cial
    private float specialEnemySpawnDelay; // D�lai al�atoire entre les apparitions des ennemis sp�ciaux
    private bool specialEnemyEnabled = false; // Permet d'emp�cher le spawn des ennemis sp�ciaux au d�but

    void Start()
    {
        // Retarde l'activation du spawn des ennemis sp�ciaux 
        StartCoroutine(EnableSpecialEnemyAfterDelay(1f));
        StartCoroutine(SpawnEnemiesUpdate());
        StartCoroutine(SpawnEnemiesContinuously());
    }

    IEnumerator EnableSpecialEnemyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        specialEnemyEnabled = true; // Active le spawn des ennemis sp�ciaux apr�s le d�lai
        specialEnemySpawnDelay = Random.Range(10f, 20f); // G�n�re un premier d�lai al�atoire
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
        while (enemiesPerWave < MaxEnnemies) // contrairement au if cr�e une boucle.
        { 

        yield return new WaitForSeconds(NewSpawnInterval);
        enemiesPerWave += NewEnemiesAdd;

        }
    }

    void SpawnWave()
    {
        int i = 0;
        bool specialEnemySpawned = false; // V�rifie si un ennemi sp�cial a d�j� spawn

        while (i < enemiesPerWave)
        {
            GameObject enemy;
            Vector3 spawnPosition;

            // V�rifie si l'ennemi sp�cial doit appara�tre (seulement apr�s 20 secondes et un seul par ligne)
            if (specialEnemyEnabled && !specialEnemySpawned && specialEnemySpawnTimer >= specialEnemySpawnDelay && i <= enemiesPerWave - 3)
            {
                spawnPosition = spawnPoint.position + Vector3.right * (i * specialEnemySpacing) + Vector3.up * specialEnemyHeight;
                enemy = Instantiate(specialEnemyPrefab, spawnPosition, spawnPoint.rotation);
                StartCoroutine(MoveEnemy(enemy));
                specialEnemySpawned = true;
                specialEnemySpawnTimer = 0f; // R�initialise le timer
                specialEnemySpawnDelay = Random.Range(10f, 20f); // D�finir un nouveau d�lai al�atoire
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

        specialEnemySpawnTimer += spawnInterval; // Mise � jour du timer pour le spawn de l'ennemi sp�cial
    }

    IEnumerator MoveEnemy(GameObject enemy)
    {
        while (enemy != null) // V�rifie que l'objet existe toujours
        {
            enemy.transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
            yield return null; // Attend la prochaine frame
        }
    }
}