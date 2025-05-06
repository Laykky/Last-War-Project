using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemies_Variables : MonoBehaviour
{
    [SerializeField] private int health = 4; // Points de vie (modifiable dans l'Inspector)
    private int currenthealth = 4; // Points de vie (modifiable dans l'Inspector)
    [SerializeField] private Renderer enemyRenderer; // Le Renderer pour modifier la couleur
    [SerializeField] private HealthBar healthBar;
    private Color originalColor; // Couleur de base de l'ennemi

    // 🔹 VFX (à activer plus tard)
    // [SerializeField] private ParticleSystem hitVFX; // Effet quand une balle touche
    // [SerializeField] private ParticleSystem deathVFX; // Effet de mort

    void Start()
    {
        if (enemyRenderer == null)
            enemyRenderer = GetComponent<Renderer>(); // Récupérer le Renderer

        originalColor = enemyRenderer.material.color; // Sauvegarder la couleur de base
        currenthealth = health;
        // HealthBar Healthbar = GetComponent<HealthBar>();
        healthBar.UpdateHealthBar(health,currenthealth);
    }

    public void TakeDamage(int damage)
    {
        currenthealth -= damage; // Réduction des PV
        healthBar.UpdateHealthBar(health, currenthealth);

        StartCoroutine(FlashDamageEffect()); // Effet visuel rapide

        if (currenthealth <= 0)
        {
            Die();
            print("mort");
        }
    }

    IEnumerator FlashDamageEffect()
    {
        enemyRenderer.material.color = Color.white; // Devient blanc
        yield return new WaitForSeconds(0.2f); // Ultra rapide
        enemyRenderer.material.color = originalColor; // Retour à la couleur d'origine
    }

    void Die()
    {
        // 🔹 VFX de mort (à activer plus tard)
        // if (deathVFX != null) Instantiate(deathVFX, transform.position, Quaternion.identity);

        Destroy(gameObject); // Supprimer l'ennemi après sa mort
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerAgent agent = other.GetComponentInParent<PlayerAgent>();
            agent.KillAgent();
            //Destroy(other.gameObject);
            Debug.Log("Le joueur a été détruit par les ennemis !");
        }

        if (other.tag == "EndCollider")
        {

        }
    }
}
