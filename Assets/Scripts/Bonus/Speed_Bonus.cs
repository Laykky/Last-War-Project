using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_Bonus : MonoBehaviour
{
    [SerializeField] public float speedBoost = 5f; // augmentation de la vitesse des projectiles

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           Projectile projectilescript = other.GetComponent<Projectile>(); // récupère le script du joueur
            if (projectilescript != null)
            {
                projectilescript.m_speed += speedBoost; // 🔹 Augmente la vitesse des projectiles
                Debug.Log("Bonus activé ! Nouvelle vitesse des projectiles : " + projectilescript.m_speed);
            }

            Destroy(gameObject); // 🔹 Supprime le bonus après activation
        }
    }
}
