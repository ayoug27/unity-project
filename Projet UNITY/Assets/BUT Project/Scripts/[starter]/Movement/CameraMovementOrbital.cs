using UnityEngine;
using UnityEngine.InputSystem;

namespace BUT
{
    /* Handle Camera's rotation */
    public class CameraMovementOrbital : MonoBehaviour
    {
        [SerializeField]
        float m_MouseSpeedRotation;
        
        [SerializeField]
        float m_StickSpeedRotation;
        
        public float MouseSpeedRotation => Mathf.Deg2Rad * m_MouseSpeedRotation * Time.deltaTime;
        public float StickSpeedRotation => Mathf.Deg2Rad * m_StickSpeedRotation;


        [SerializeField]
        Vector2 m_RotationXLimits;

        Vector3 m_CurrentRotation;
        
        private Vector2 m_RotationVector;
        private bool m_stickUsed = false;

        private void Start()
        {
            // init rotation
            m_CurrentRotation = transform.rotation.eulerAngles;
        }

        public void MouseRotate(InputAction.CallbackContext _context)
        {
            m_stickUsed = false;
            // add rotation
            m_CurrentRotation.y += _context.ReadValue<Vector2>().x * MouseSpeedRotation;
            m_CurrentRotation.x += -_context.ReadValue<Vector2>().y * MouseSpeedRotation;
            // clamp rotation
            m_CurrentRotation.x = Mathf.Clamp(m_CurrentRotation.x, m_RotationXLimits.x, m_RotationXLimits.y);
            
            // apply rotation
            transform.rotation = Quaternion.Euler(m_CurrentRotation.x, m_CurrentRotation.y, m_CurrentRotation.z);
        }

        public void StickRotate(InputAction.CallbackContext _context)
        {
            m_stickUsed = true;
            m_RotationVector = _context.ReadValue<Vector2>() * StickSpeedRotation;
        }

        public void Update()
        {
            if (!m_stickUsed) return;
            // add rotation
            m_CurrentRotation.y += m_RotationVector.x;
            m_CurrentRotation.x += m_RotationVector.y;

            // clamp rotation
            m_CurrentRotation.x = Mathf.Clamp(m_CurrentRotation.x, m_RotationXLimits.x, m_RotationXLimits.y);

            // apply rotation
            transform.rotation = Quaternion.Euler(m_CurrentRotation.x, m_CurrentRotation.y, m_CurrentRotation.z);
        }
    }
}