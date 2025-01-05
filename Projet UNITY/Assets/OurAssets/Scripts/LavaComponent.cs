using System.Collections;
using OurAssets.Scripts.Player;
using UnityEngine;

namespace OurAssets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class LavaComponent : MonoBehaviour
    {
        public int damage = 1;
        
        private void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Player") || other.GetType() != typeof(CharacterController)) return;
            var player = other.GetComponent<PlayerComponent>();
            player.TakeDamage(damage);
        }
    }
}
