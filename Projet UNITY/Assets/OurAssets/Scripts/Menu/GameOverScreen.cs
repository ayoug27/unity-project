using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public AudioSource Laugh;
    public AudioSource EldenRingDeath;

    // Fonction pour afficher l'écran de game over
    public void ShowGameOverScreen()
    {
        gameObject.SetActive(true);
        StartCoroutine(PlayGameOverSequence());
    }

    IEnumerator PlayGameOverSequence()
    {
        EldenRingDeath.Play();
        yield return new WaitForSeconds(EldenRingDeath.clip.length);
        Laugh.Play();
    }

    // Fonction appelée par le bouton pour redémarrer la scène
    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
