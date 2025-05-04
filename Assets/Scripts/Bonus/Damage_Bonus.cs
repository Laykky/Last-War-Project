using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Bonus : MonoBehaviour

{
    [SerializeField] private int damageBoost = 1; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Projectile Projectilemore = other.GetComponent<Projectile>(); 
            if (Projectilemore != null)
            {
                Projectilemore.IncreaseBulletDamage(damageBoost);
                Debug.Log("Bonus activé ! Les projectiles font maintenant " + Projectilemore.GetBulletDamage() + " dégâts.");
            }

            Destroy(gameObject); // 🔹 Supprime le bonus après activation
        }
    }
}
