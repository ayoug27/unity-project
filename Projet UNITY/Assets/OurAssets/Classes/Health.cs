namespace OurAssets.Classes
{
    public class Health
    {
        private int m_CurrentHealth;
        private int m_BaseHealth;

        public Health(int baseHealth)
        {
            m_BaseHealth = baseHealth;
            m_CurrentHealth = baseHealth;
        }

        public void TakeDamage(int damage)
        {
            m_CurrentHealth -= damage < 0 ? 0 : damage;
        }

        public void Heal(int heal)
        {
            if (heal < 0) return;
            var newHealth = m_CurrentHealth + heal;
            m_CurrentHealth = newHealth > m_BaseHealth ? m_BaseHealth : newHealth;
        }

        public void ResetHealth()
        {
            m_CurrentHealth = m_BaseHealth;
        }

        public int GetCurrentHealth()
        {
            return m_CurrentHealth;
        }

        public int GetBaseHealth()
        {
            return m_BaseHealth;
        }

        public bool IsDead()
        {
            return m_CurrentHealth <= 0;
        }
    }
}