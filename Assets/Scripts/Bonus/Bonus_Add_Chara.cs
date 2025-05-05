using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BonusAddChara : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI; // Texte affichant la valeur du bonus
    [SerializeField] private Canvas bonusCanvas; // Canvas du bonus
    public int bonusValue; // Valeur aléatoire entre 1 et 4
    public int hitCount = 0; // Nombre de fois où le bonus est touché
    [SerializeField] public int maxHits = 3; // Limite de montée à 3 impacts
    [SerializeField] public float moveAmount = 0.1f; // Déplacement vertical par impact

    public int GetBonusValue()
    {
        return bonusValue;
    }

    void Awake()
    {
        bonusValue = Random.Range(1, 5); // Valeur aléatoire avant le spawn

        textUI = GetComponentInChildren<TextMeshProUGUI>(); // Trouve TextMeshPro automatiquement
        bonusCanvas = GetComponentInChildren<Canvas>(); // Trouve le Canvas automatiquement

        if (textUI != null)
        {
            textUI.SetText("{0}", bonusValue); // Affichage du chiffre sur le bonus
        }
        else
        {
            Debug.LogError("TextMeshPro non trouvé sur l'objet Bonus !");
        }

        if (bonusCanvas != null)
        {
            bonusCanvas.renderMode = RenderMode.WorldSpace; // Assure que le Canvas est bien configuré
        }
        else
        {
            Debug.LogError("Canvas du bonus non assigné !");
        }
    }


        void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet")) // Vérifie si l'objet touché est une balle du joueur
        {
            
                if (hitCount < maxHits) // 🔹 Vérifie que la montée est encore possible
                {
                    transform.position += new Vector3(0, moveAmount, 0); // 🔹 Déplace le bonus vers le haut
                    hitCount++; // 🔹 Incrémente le nombre de fois où il a été touché
                }
                else //  Si le bonus atteint sa hauteur max, on augmente la valeur
                {
                    bonusValue = Mathf.Min(bonusValue + 1, 8); // 🔹 Augmente mais ne dépasse jamais 8
                    UpdateBonusUI(); // 🔹 Met à jour l'affichage UI
                }

                Destroy(other.gameObject); // Détruit la balle après l'impact
        }
        if (other.CompareTag("Player") && hitCount < maxHits) // 🔹 Si le bonus n'est PAS à zéro, détruit l'élément Player
        {
            PlayerAgent agent = other.GetComponentInParent<PlayerAgent>();
            agent.KillAgent();
            //Destroy(other.gameObject);
            Debug.Log("Le joueur a été détruit par le bonus !");
        }
    }

    void UpdateBonusUI()
    {
        if (textUI != null)
        {
            textUI.SetText("{0}", bonusValue); // Met à jour l'affichage du bonus
        }
    }
}