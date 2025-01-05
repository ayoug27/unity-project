using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BUT;
using OurAssets.Classes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace OurAssets.Scripts.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerComponent : MonoBehaviour
    {
        public UnityEvent<float> dyingAnimation;
        
        [SerializeField]
        private Outline m_ModelOutline;

        
        [SerializeField] 
        private string[] m_CollectibleTags;
        
        
        [SerializeField]
        private int baseHealth = 20;
        
        [SerializeField]
        private float invulnerabilityTime = 1.0f;
        
        [HideInInspector]
        public InteractableComponent interactable;
        private PlayerMovement m_Movement;
        
        private Dictionary<string, int> m_Collectibles;
        private Health m_Health;
        
        private bool m_IsInvulnerable;

        private Coroutine m_HurtInvulnerabilityCoroutine;

        private static Dictionary<string, int> InitCollectiblesMap(string[] collectibleTags)
        {
            return collectibleTags.ToDictionary(tagName => tagName, tagName => 0);
        }
    
        private void Awake()
        {
            m_Collectibles = InitCollectiblesMap(m_CollectibleTags);
            m_Movement = GetComponent<PlayerMovement>();
            m_Health = new Health(baseHealth);
        }
    
        public void IncrementCollectible(string collectibleTag)
        {
            if (!m_Collectibles.ContainsKey(collectibleTag))
            {
                Debug.LogError($"Collectible tag {collectibleTag} is not registered in the storage.");
                return;
            }
            ++m_Collectibles[collectibleTag];
        }
        
        public void IncrementCollectible(string collectibleTag, int nb)
        {
            if (!m_Collectibles.ContainsKey(collectibleTag))
            {
                Debug.LogError($"Collectible tag {collectibleTag} is not registered in the storage.");
                return;
            }
            m_Collectibles[collectibleTag] += nb;
        }
        
        public void DecrementCollectible(string collectibleTag)
        {
            if (!m_Collectibles.ContainsKey(collectibleTag))
            {
                Debug.LogError($"Collectible tag {collectibleTag} is not registered in the storage.");
                return;
            }
            --m_Collectibles[collectibleTag];
        }
        
        public void DecrementCollectible(string collectibleTag, int nb)
        {
            if (!m_Collectibles.ContainsKey(collectibleTag))
            {
                Debug.LogError($"Collectible tag {collectibleTag} is not registered in the storage.");
                return;
            }
            m_Collectibles[collectibleTag] -= nb;
        }
    
        public int GetCollectibleCount(string collectibleTag)
        {
            return m_Collectibles.GetValueOrDefault(collectibleTag, 0);
        }
        
        public void Interact(InputAction.CallbackContext context)
        {
            if (!context.started || interactable.IsUnityNull()) return;
            interactable.Interact(this);
        }
        
        public void TakeDamage(int damage)
        {
            if (IsDead() || m_IsInvulnerable) return;
            m_Health.TakeDamage(damage);
            if (m_Health.IsDead()) Die();
            else m_HurtInvulnerabilityCoroutine = StartCoroutine(IsVulnerable());
        }
        
        public void Heal(int heal)
        {
            m_Health.Heal(heal);
        }
        
        public int GetCurrentHealth()
        {
            return m_Health.GetCurrentHealth();
        }
        
        public int GetMaxHealth()
        {
            return m_Health.GetBaseHealth();
        }
        
        public void Die()
        {
            m_Movement.enabled = false;
            dyingAnimation.Invoke(1.0f);
            Invoke("LaunchGameOverScreen", 1.0f);
        }

        private void LaunchGameOverScreen()
        {
            SceneManager.LoadScene("GameOver");
        }
        
        public bool IsDead()
        {
            return m_Health.IsDead();
        }

        public IEnumerator TakeSuperMedicine(float flickerTime = 0.1f)
        {
            if (m_HurtInvulnerabilityCoroutine != null)
            {
                StopCoroutine(m_HurtInvulnerabilityCoroutine);
                m_HurtInvulnerabilityCoroutine = null;
            }
            m_ModelOutline.OutlineColor = Color.green;
            m_IsInvulnerable = true;
            while (true)
            {
                m_ModelOutline.enabled = !m_ModelOutline.enabled;
                yield return new WaitForSeconds(flickerTime);
            }
        }

        public IEnumerator IsVulnerable(float flickerTime = 0.1f)
        {
            m_IsInvulnerable = true;
            // make outline flicker for invulnerabilityTime
            for (var i = 0; i < invulnerabilityTime / flickerTime; ++i)
            {
                m_ModelOutline.enabled = !m_ModelOutline.enabled;
                yield return new WaitForSeconds(flickerTime);
            }
            m_IsInvulnerable = false;
        }
    }
}
