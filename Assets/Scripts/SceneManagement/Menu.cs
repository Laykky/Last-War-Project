using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button playButton; // Bouton de démarrage du jeu
    [SerializeField] private Button quitButton; // Bouton pour quitter le jeu
    

    private void Start()
    {
        // 🔹 Assigne les fonctions aux boutons
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // Charge la scène spécifiée
    }

    private void QuitGame()
    {
        Application.Quit(); // Quitte le jeu
        Debug.Log("Le jeu a été quitté."); // Fonctionne en build, mais affiche un message en mode éditeur
    }
}

