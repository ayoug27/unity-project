using System;
using OurAssets.Scripts.Player;
using UnityEngine;


namespace OurAssets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class WeaponCollectibleCollision : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip weaponSound;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<PlayerAttack>();
            if (player.HasWeapon) return;
            player.HasWeapon = true;
            audioSource.PlayOneShot(weaponSound);
            Destroy(gameObject);
        }
    }
}