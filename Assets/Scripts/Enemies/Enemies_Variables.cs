using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Variables : MonoBehaviour
{
    [SerializeField] private int health = 4; // Points de vie (modifiable dans l'Inspector)
    [SerializeField] private Renderer enemyRenderer; // Le Renderer pour modifier la couleur
    private Color originalColor; // Couleur de base de l'ennemi

    // 🔹 VFX (à activer plus tard)
    // [SerializeField] private ParticleSystem hitVFX; // Effet quand une balle touche
    // [SerializeField] private ParticleSystem deathVFX; // Effet de mort

    void Start()
    {
        if (enemyRenderer == null)
            enemyRenderer = GetComponent<Renderer>(); // Récupérer le Renderer

        originalColor = enemyRenderer.material.color; // Sauvegarder la couleur de base
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // Réduction des PV

        StartCoroutine(FlashDamageEffect()); // Effet visuel rapide

        if (health <= 0)
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
}
