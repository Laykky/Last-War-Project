using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bonus_Holder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Canvas bonusCanvas;
    public int bonusValue;
    [SerializeField] private int minBonusValue = 10;
    [SerializeField] private int maxBonusValue = 20;

    void Awake()
    {
        bonusValue = Random.Range(minBonusValue, maxBonusValue);

        textUI = GetComponentInChildren<TextMeshProUGUI>();
        bonusCanvas = GetComponentInChildren<Canvas>();

        if (textUI != null)
        {
            textUI.SetText("{0}", bonusValue);
        }
        else
        {
            Debug.LogError("TextMeshPro non trouvé !");
        }

        if (bonusCanvas != null)
        {
            bonusCanvas.renderMode = RenderMode.WorldSpace;
        }
        else
        {
            Debug.LogError("Canvas du bonus non assigné !");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            bonusValue--;
            UpdateBonusUI();
            Destroy(other.gameObject);

            if (bonusValue <= 0) // 🔹 Si le compteur atteint zéro
            {
                DestroyHolderBonusElements(); // 🔹 Supprime uniquement les éléments avec le tag HolderBonus
            }
        }

            if (other.CompareTag("Player") && bonusValue > 0) // 🔹 Si le bonus n'est PAS à zéro, détruit l'élément Player
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
            textUI.SetText("{0}", bonusValue);
        }
    }

    void DestroyHolderBonusElements()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("HolderBonus"))
            {
                Destroy(child.gameObject); // 🔹 Supprime uniquement les enfants ayant le tag HolderBonus
            }
        }
    }
}
