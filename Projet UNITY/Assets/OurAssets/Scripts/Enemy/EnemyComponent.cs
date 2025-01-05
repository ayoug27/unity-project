using OurAssets.Classes;
using UnityEngine;
using UnityEngine.Events;

namespace OurAssets.Scripts.Enemy
{
    public class EnemyComponent : MonoBehaviour
    {
        public UnityEvent<float> dyingAnimation;
        
        [SerializeField]
        private int baseHealth = 20;
        
        [HideInInspector]
        private EnemyMovement m_Movement;
        
        private Health m_Health;

        private static GameObject m_TrophyObject;
        
        private void Awake()
        {
            m_Movement = GetComponent<EnemyMovement>();
            m_Health = new Health(baseHealth);
            if (!m_TrophyObject)
            {
                m_TrophyObject = GameObject.FindGameObjectWithTag("Trophy");
                m_TrophyObject.SetActive(false);

            }
        }
        
        public void Die()
        {
            m_Movement.enabled = false;
            dyingAnimation.Invoke(1.0f);
            gameObject.tag = "Dead";
            if (CheckIfAllEnemiesAreDead()) m_TrophyObject.SetActive(true);
        }
        
        public void TakeDamage(int damage)
        {
            if (IsDead()) return;
            m_Health.TakeDamage(damage);
            if (m_Health.IsDead()) Die();
        }
        
        public bool IsDead()
        {
            return m_Health.IsDead();
        }
        
        private static bool CheckIfAllEnemiesAreDead()
        {
            return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
        }
        
    }
}