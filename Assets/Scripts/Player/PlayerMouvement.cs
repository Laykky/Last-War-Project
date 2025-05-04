using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    // 🔹 Variables du déplacement 
    [SerializeField] private float speed = 5f; // Vitesse du joueur
    [SerializeField] private float groundRayLength = 0.1f; // Distance du Raycast vers le sol
    [SerializeField] private float sideRayLength = 0.5f; // Distance du Raycast vers les côtés
    [SerializeField] private LayerMask groundLayer; // Layer du sol
    [SerializeField] private float spacing = 0.5f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canMoveRight;
    private bool canMoveLeft;

    // 🔹 Variables du tir 
    [SerializeField] private GameObject bulletPrefab; // Prefab de la balle
    [SerializeField] private Transform shootingSpot; // Position où les balles spawnent
    [SerializeField] private float bulletSpeed = 15f; // Vitesse des balles
    [SerializeField] private float shootingRate = 0.3f; // Intervalle entre chaque tir

    // 🔹 Gestion des bonus 
    [SerializeField] private GameObject playerPrefab; // Le prefab du personnage à cloner
    private int bonusValue = 0; // Nombre de tireurs à ajouter

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ShootContinuously());
    }

    void Update()
    {
        CheckGround();
        CheckBorders();

        float moveInput = 0f;

        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && canMoveRight)
            moveInput = -1f; // Correction du sens du déplacement
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q)) && canMoveLeft)
            moveInput = 1f;

        if (isGrounded)
        {
            rb.velocity = new Vector3(moveInput * speed, rb.velocity.y, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundRayLength, groundLayer);
    }

    void CheckBorders()
    {
        canMoveRight = !Physics.Raycast(transform.position, Vector3.right, sideRayLength, groundLayer);
        canMoveLeft = !Physics.Raycast(transform.position, Vector3.left, sideRayLength, groundLayer);
    }

    IEnumerator ShootContinuously()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootingRate);
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && shootingSpot != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootingSpot.position, shootingSpot.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.velocity = transform.forward * bulletSpeed;
            }
        }
    }

    // 🔹 Détection du bonus 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bonus"))
        {
            BonusAddChara bonusScript = other.GetComponent<BonusAddChara>();
            if (bonusScript != null)
            {
                bonusValue = bonusScript.GetBonusValue();
                SpawnAdditionalPlayers(bonusValue);
            }
            Destroy(other.gameObject);
        }
    }

    // 🔹 Spawn des nouveaux personnages 
    void SpawnAdditionalPlayers(int numberOfPlayers)
    {
        int maxPerRow = 8; // Nombre maximum de joueurs par ligne (modifiable dans l'Inspector)
        float rowSpacing = 1.0f; // Espacement entre les lignes sur l'axe Z (modifiable dans l'Inspector)
        Vector3 startPosition = transform.position;
        Debug.Log("Ajout de " + numberOfPlayers + " nouveaux joueurs !");

        for (int i = 0; i < numberOfPlayers; i++)
        {
            int row = i / maxPerRow; // Détermine la ligne actuelle
            int positionInRow = i % maxPerRow; // Position sur la ligne actuelle

            Vector3 spawnPosition = startPosition + new Vector3(positionInRow * spacing, 0, row * rowSpacing);

            // Vérification si la position est sur le layer Ground
            if (Physics.Raycast(spawnPosition + Vector3.down * 0.5f, Vector3.down, 1f, groundLayer))
            {
                Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                spawnPosition = TrouverNouvellePosition(spawnPosition, startPosition, spacing, rowSpacing);
                if (spawnPosition != Vector3.zero)
                {
                    Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("Aucune position valide trouvée pour spawn !");
                }
            }
        }
    }

    Vector3 TrouverNouvellePosition(Vector3 tentativePosition, Vector3 startPosition, float spacing, float rowSpacing)
    {
        // Teste derrière dans la même ligne
        Vector3 nouvellePosition = startPosition - new Vector3(spacing, 0, 0);
        if (Physics.Raycast(nouvellePosition + Vector3.down * 0.5f, Vector3.down, 1f, groundLayer))
        {
            return nouvellePosition;
        }

        // Teste devant dans la même ligne
        nouvellePosition = startPosition + new Vector3(spacing, 0, 0);
        if (Physics.Raycast(nouvellePosition + Vector3.down * 0.5f, Vector3.down, 1f, groundLayer))
        {
            return nouvellePosition;
        }

        // Teste une nouvelle ligne en Z
        nouvellePosition = startPosition + new Vector3(0, 0, rowSpacing);
        if (Physics.Raycast(nouvellePosition + Vector3.down * 0.5f, Vector3.down, 1f, groundLayer))
        {
            return nouvellePosition;
        }

        return Vector3.zero; // Aucun endroit valide trouvé
    }
}