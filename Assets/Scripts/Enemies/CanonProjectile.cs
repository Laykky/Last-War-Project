using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectile : MonoBehaviour
{
    // [SerializeField] private GameObject explosionVFX; // 🔹 VFX d'explosion
    [SerializeField] private LayerMask hitLayer; // 🔹 Layer des objets à toucher

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & hitLayer) != 0) // 🔹 Vérifie si l'objet est sur le Layer `Hit`
        {
            // if (explosionVFX != null)
            // {
            //      Instantiate(explosionVFX, transform.position, Quaternion.identity); // 🔹 Crée le VFX d'explosion
            //  }

            Destroy(gameObject); // 🔹 Supprime la balle après l’impact
        }

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

    }
}
