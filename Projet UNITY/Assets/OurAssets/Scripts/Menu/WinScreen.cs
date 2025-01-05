using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    // Fonction pour afficher l'écran de win
    public void ShowWinScreen()
    {
        gameObject.SetActive(true);
    }

    // Fonction appelée par le bouton pour mettre la scène suivante
    public void WinGame()
    {
        // int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        //
        // // Vérifier si l'index existe pour éviter des erreurs
        // if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        // {
        //     SceneManager.LoadScene(nextSceneIndex);
        // }
        // else
        // {
        //     Debug.Log("Dernier niveau atteint !");
        //     //Revenir au menu principal
        //     SceneManager.LoadScene(0);
        // }
        var nextLevel = PlayerPrefs.GetString("NextLevel");
        if (nextLevel == "") nextLevel = "MainMenu";
        SceneManager.LoadScene(nextLevel);
    }
}
