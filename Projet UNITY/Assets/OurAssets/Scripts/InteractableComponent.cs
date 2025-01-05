using OurAssets.Scripts.Player;
using UnityEngine;

namespace OurAssets.Scripts
{
    [RequireComponent(typeof(Outline))]
    public abstract class InteractableComponent : MonoBehaviour
    {
        private Outline m_Outline;
        
        private void Awake()
        {
            m_Outline = GetComponent<Outline>();
            m_Outline.enabled = false;
        }
        
        public abstract void Interact(PlayerComponent player);

        protected void Highlight(Color color)
        {
            m_Outline.OutlineColor = color;
            m_Outline.enabled = true;
        }
        
        protected void Unhighlight()
        {
            m_Outline.enabled = false;
        }
    }
}