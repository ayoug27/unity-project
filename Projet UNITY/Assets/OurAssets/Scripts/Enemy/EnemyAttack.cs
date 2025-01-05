using OurAssets.Scripts.Player;
using UnityEngine;

namespace OurAssets.Scripts.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class EnemyAttack : MonoBehaviour
    {
        public int damage = 10;
    
        private void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Player") || other.GetType() != typeof(CharacterController)) return;
            var player = other.GetComponent<PlayerComponent>();
            player.TakeDamage(damage);
        }
    }
}
