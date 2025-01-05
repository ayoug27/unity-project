using UnityEngine;

namespace OurAssets.Scripts
{
    public class CollectibleRotation : MonoBehaviour
    {
        public float rotationSpeed = 0.5f;

        private void Update()
        {
            transform.Rotate(0,rotationSpeed,0);
        }
    }
}
