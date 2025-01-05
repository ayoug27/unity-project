using System;
using OurAssets.Scripts.Player;
using UnityEngine;

namespace OurAssets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class ChestComponent : InteractableComponent
    {
        private static readonly int Opened = Animator.StringToHash("opened");
        public Animator animator;
        private bool m_Opened;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<PlayerComponent>();
            player.interactable = this;
            Highlight(player.GetCollectibleCount("Key") == 0 || m_Opened ? Color.red : Color.green);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player") && other.GetComponent<PlayerComponent>()?.interactable != this) return;
            other.GetComponent<PlayerComponent>().interactable = null;
            Unhighlight();
        }
        
        public override void Interact(PlayerComponent player)
        {
            if (player.GetCollectibleCount("Key") == 0 || m_Opened) return;
            player.DecrementCollectible("Key");
            animator.SetBool(Opened, true);
            StartCoroutine(player.TakeSuperMedicine());
            m_Opened = true;
            Highlight(player.GetCollectibleCount("Key") == 0 || m_Opened ? Color.red : Color.green);
        }
    }
}
