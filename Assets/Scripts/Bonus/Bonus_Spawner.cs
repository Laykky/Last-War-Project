using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject bonusAddCharaPrefab; // 🔹 Bonus de base
    [SerializeField] private GameObject bonusHolderPrefab; // 🔹 Bonus SpeedBonus
    [SerializeField] private Transform spawnPoint; // 🔹 Point de spawn
    [SerializeField] private float spawnInterval = 1f; // 🔹 Temps entre chaque spawn
    [SerializeField] private float bonusSpeed = 2f; // 🔹 Vitesse de déplacement des bonus

    void Start()
    {
        InvokeRepeating("SpawnRandomBonus", 2f, spawnInterval); // 🔹 Spawn aléatoire à intervalles réguliers
    }

    void SpawnRandomBonus()
    {
        GameObject bonusToSpawn = Random.Range(0, 2) == 0 ? bonusAddCharaPrefab : bonusHolderPrefab; // 🔹 Choix aléatoire

        if (bonusToSpawn != null && spawnPoint != null)
        {
            GameObject bonus = Instantiate(bonusToSpawn, spawnPoint.position, spawnPoint.rotation);
            StartCoroutine(MoveBonus(bonus));
        }
    }

    IEnumerator MoveBonus(GameObject bonus)
    {
        while (bonus != null)
        {
            bonus.transform.Translate(Vector3.forward * bonusSpeed * Time.deltaTime);
            yield return null;
        }
    }
}