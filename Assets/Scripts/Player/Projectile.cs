using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 8f; // Temps avant destruction automatique
    [SerializeField] private LayerMask targetLayers; // Ennemis & bonus à toucher
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] public float m_speed = 15f;
    [SerializeField] private int m_damage = 1;
    void Start()
    {
        Destroy(gameObject, lifetime); // Auto-destruction après un certain temps
        if (m_rb != null)
        {
            m_rb.velocity = transform.forward * m_speed; // Envoie la balle vers l'avant
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayers.value & (1 << collision.gameObject.layer)) > 0) // Vérifie si l'objet touché est un ennemi ou un bonus
        {
            Enemies_Variables enemy = collision.gameObject.GetComponent<Enemies_Variables>();
            if (enemy != null)
            {
                enemy.TakeDamage(m_damage); // Inflige 1 point de dégât
                print("dêgats infligés");
            }

            Destroy(gameObject); // Détruit la balle à l'impact
        }
    }

    public void IncreaseBulletDamage(int amount)
    {
        m_damage += amount; 
    }

    public int GetBulletDamage()
    {
        return m_damage; 
    }
}


