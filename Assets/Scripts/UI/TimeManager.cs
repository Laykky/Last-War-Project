using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    private float elapsedTime = 0f; // 🔹 Temps écoulé depuis le début du jeu
    [SerializeField] private TMP_Text timerText; // 🔹 Référence au texte TextMeshPro

    private void Update()
    {
        elapsedTime += Time.deltaTime; // 🔹 Ajoute le temps écoulé à chaque frame
        UpdateTimerUI(); // 🔹 Met à jour l'affichage
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.SetText("Scoring : {0}s", Mathf.Floor(elapsedTime)); // 🔹 Affiche le temps écoulé
        }
    }
}

