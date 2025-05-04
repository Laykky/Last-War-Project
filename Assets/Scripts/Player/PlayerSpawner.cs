using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float spacing = 0.5f;
    [SerializeField] private int maxPerRow = 8; // Nombre max de joueurs par ligne
    [SerializeField] private float rowSpacing = 1.0f; // Espacement entre les lignes en Z
    [SerializeField] private LayerMask groundLayer;
    private int bonusValue = 0;
    private bool isOriginalPlayer = true;


    void OnTriggerEnter(Collider other)
    {
        if (isOriginalPlayer && other.CompareTag("Bonus")) // Seul le joueur principal interagit avec le bonus
        {
            BonusAddChara bonusScript = other.GetComponent<BonusAddChara>();
            if (bonusScript != null)
            {
                bonusValue = bonusScript.GetBonusValue();
                SpawnAdditionalPlayers(bonusValue);
            }
            Destroy(other.gameObject); // 🔹 Assurer que le bonus est bien détruit
        }
    }

    void SpawnAdditionalPlayers(int numberOfPlayers)
    {
        Vector3 startPosition = transform.position;
        Debug.Log("Ajout de " + numberOfPlayers + " nouveaux joueurs !");

        for (int i = 0; i < numberOfPlayers; i++)
        {
            int row = i / maxPerRow;
            int positionInRow = i % maxPerRow;
            Vector3 spawnPosition = startPosition + new Vector3(positionInRow * spacing, 0, row * rowSpacing);

            // 🔹 Vérifier que l'emplacement est bien sur le layer Ground
            if (Physics.Raycast(spawnPosition + Vector3.down * 0.5f, Vector3.down, 1f, groundLayer) &&
                Physics.OverlapSphere(spawnPosition, spacing * 0.5f).Length == 0) // Vérifie qu'il n'y a pas un autre joueur trop proche
            {
                GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                newPlayer.GetComponent<PlayerSpawner>().isOriginalPlayer = false;
            }
            else
            {
                Debug.LogWarning("Zone interdite ou collision détectée, recherche d’une nouvelle position...");
                spawnPosition = TrouverNouvellePosition(startPosition, rowSpacing);

                if (spawnPosition != Vector3.zero)
                {
                    GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                    newPlayer.GetComponent<PlayerSpawner>().isOriginalPlayer = false;
                }
                else
                {
                    Debug.LogError("Aucune position valide trouvée pour spawn !");
                }
            }
        }
    }

    // 🔹 Fonction pour trouver une nouvelle position en cas de problème
    Vector3 TrouverNouvellePosition(Vector3 startPosition, float rowSpacing)
    {
        Vector3 nouvellePosition;

        // Teste une nouvelle ligne derrière
        nouvellePosition = startPosition - new Vector3(0, 0, rowSpacing);
        if (Physics.Raycast(nouvellePosition + Vector3.down * 0.5f, Vector3.down, 1f, groundLayer))
        {
            return nouvellePosition;
        }

        // Teste une nouvelle ligne devant
        nouvellePosition = startPosition + new Vector3(0, 0, rowSpacing);
        if (Physics.Raycast(nouvellePosition + Vector3.down * 0.5f, Vector3.down, 1f, groundLayer))
        {
            return nouvellePosition;
        }

        return Vector3.zero; // Impossible de trouver une position valide
    }
}