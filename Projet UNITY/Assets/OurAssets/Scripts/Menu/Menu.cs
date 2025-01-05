using UnityEngine;
using UnityEngine.SceneManagement;

namespace OurAssets.Scripts
{
    public class Menu : MonoBehaviour
    {
        public void PlayGame()
        {
            var scene = SceneManager.GetSceneByName("Level 1");
            print(scene);
            SceneManager.LoadScene("Level 1");
        }
        public void QuitGame()
        {
            Debug.Log("Ca quitte");
            Application.Quit();
        }

    }
}
