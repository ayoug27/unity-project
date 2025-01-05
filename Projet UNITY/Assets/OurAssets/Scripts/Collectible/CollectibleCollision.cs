using OurAssets.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace OurAssets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class CollectibleCollision : MonoBehaviour
    {
        public AudioSource audioSource;
        [FormerlySerializedAs("coinSound")] public AudioClip collectibleSound;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<PlayerComponent>();
            player.IncrementCollectible(gameObject.tag);
            audioSource.PlayOneShot(collectibleSound);
            Destroy(gameObject);
        }
    }
}
