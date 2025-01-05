using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace OurAssets.Scripts.Player
{
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerAttack : MonoBehaviour
    {
        public UnityEvent<float> hittingAnimation;
        public UnityEvent<bool> hasWeaponAnimation;
        
        [SerializeField] 
        private GameObject m_WeaponObject;
        
        [SerializeField]
        private WeaponStats m_WeaponStats;
        
        private BoxCollider m_hitCollider;
        
        private bool m_HasWeapon;
        public bool HasWeapon
        {
            set
            {
                m_WeaponObject.SetActive(value);
                hasWeaponAnimation.Invoke(value);
                m_HasWeapon = value;
            }
            get => m_HasWeapon;
        }

        private bool m_Hitting;
        public bool Hitting
        {
            set
            {
                m_hitCollider.enabled = value;
                m_Hitting = value;
            }
            get => m_Hitting;
        }
        
        private void Awake()
        {
            m_hitCollider = GetComponent<BoxCollider>();
            Hitting = false;
        }

        public void Hit(InputAction.CallbackContext context)
        {
            if (!context.started || !HasWeapon) return;
            hittingAnimation.Invoke(1.0f);
            Invoke("OnHitStart",0.5f);
            Invoke("OnHitEnd",1f);
        }

        public void OnTriggerEnter(Collider other)
        {
            if(!Hitting) return;
            if (!other.CompareTag("Enemy") && other.GetType() != typeof(CharacterController)) return;
            var enemy = other.GetComponent<Enemy.EnemyComponent>();
            enemy.TakeDamage(m_WeaponStats.damage);
        }

        public void OnHitStart()
        {
            Hitting = true;
        }
        
        public void OnHitEnd()
        {
            Hitting = false;
        }
    }
}