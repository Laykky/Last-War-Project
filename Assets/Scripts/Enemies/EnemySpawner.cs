using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Le prefab de l'ennemi
    public Transform spawnPoint; // Le point de spawn initial
    public int enemiesPerWave = 8; // Nombre d'ennemis par vague
    public float spacing = 2f; // Espacement entre les ennemis
    public float spawnInterval = 1f; // Temps entre chaque vague

    void Start()
    {
        StartCoroutine(SpawnEnemiesContinuously());
    }

    IEnumerator SpawnEnemiesContinuously()
    {
        while (true)
        {
            SpawnWave();
            yield return new WaitForSeconds(spawnInterval); // Attente avant la prochaine vague
        }
    }

    void SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 spawnPosition = spawnPoint.position + Vector3.right * (i * spacing);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);
            enemy.AddComponent<EnemyMovement>(); // Ajoute le script de mouvement à l'ennemi
        }
    }
}

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f; // Vitesse de déplacement de l'ennemi

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Déplacement sur l'axe Z
    }
}

