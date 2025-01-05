using UnityEngine;
using UnityEngine.SceneManagement;

namespace OurAssets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class TrophyComponent : MonoBehaviour
    {
        public string nextSceneName;
        
        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            PlayerPrefs.SetString("NextLevel", nextSceneName);
            SceneManager.LoadScene("WinMenu");
        }
    }
}