using System.Collections;
using BUT;
using OurAssets.Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

namespace OurAssets.Scripts.Enemy
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField]
        Movement m_Movement;
        
        private Vector3 m_OriginalPosition;
        private CharacterController m_CharacterController;
        
        private GameObject m_Player;
        private PlayerComponent m_PlayerComponent;
        
        public UnityEvent<float> OnSpeedChange;
        public UnityEvent<bool> OnMovingChange;
        public UnityEvent<bool> OnGroundedChange;
        
        float m_CurrentSpeed;
        public float CurrentSpeed
        {
            set
            {
                if (Mathf.Approximately(m_CurrentSpeed, value)) return;
                m_CurrentSpeed = value;
                OnSpeedChange?.Invoke(m_CurrentSpeed);
            }
            get => m_CurrentSpeed;
        }
        
        private bool m_IsMoving;
        public bool IsMoving
        {
            set
            {
                if (m_IsMoving == value) return;
                m_IsMoving = value;
                OnMovingChange?.Invoke(m_IsMoving);
            }
            get => m_IsMoving;
        }
        
        private Vector3 m_Direction;
        public Vector3 Direction { set => m_Direction = value; get => m_Direction; }
        public Vector3 FullDirection => (GroundRotationOffset * Direction * CurrentSpeed + Vector3.up * GravityVelocity);

        private Quaternion m_GroundRotationOffset;
        public Quaternion GroundRotationOffset { set => m_GroundRotationOffset = value; get => m_GroundRotationOffset; }

        public const float GRAVITY = -9.31f;
        
        private float m_GravityVelocity;
        public float GravityVelocity { set => m_GravityVelocity = value; get => m_GravityVelocity; }
        
        [SerializeField]
        float m_RayLenght;
        [SerializeField]
        LayerMask m_RayMask;

        RaycastHit m_Hit;

        private bool m_IsGrounded;
        public bool IsGrounded
        {
            set
            {
                if (IsGrounded == value) return;
                m_IsGrounded = value;
                OnGroundedChange?.Invoke(m_IsGrounded);
            }
            get => m_IsGrounded;
        }
        
        private Vector2 m_MovementBasedOnDestination;
        private Vector3 m_MovementDirection;
        
        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            m_OriginalPosition = transform.position;
            m_Player = GameObject.FindWithTag("Player");
            m_PlayerComponent = m_Player.GetComponent<PlayerComponent>();
        }
        
        public void MovingChanged(bool _moving)
        {
            OnMovingChange?.Invoke(_moving);
        }

        public void SpeedChanged(float _speed)
        {
            OnSpeedChange?.Invoke(_speed);
        }

        public void GroundedChanged(bool _grounded)
        {
            OnGroundedChange?.Invoke(_grounded);
        }
        
        private void OnDisable()
        {
            IsMoving = false;
        }

        private void OnEnable()
        {
            StartCoroutine(Moving());
        }

        IEnumerator Moving() {
            while (enabled) {
                m_MovementBasedOnDestination = IsPlayerAlive() ? GetChasingMovement() : GetToRestingMovement();
                if (m_MovementBasedOnDestination.magnitude > 0.1f)
                {
                    if (!IsMoving)
                    {
                        IsMoving = true;
                    }
                    m_MovementBasedOnDestination = Vector3.ClampMagnitude(m_MovementBasedOnDestination, 1);
                    
                }
                else if (IsMoving)
                {
                    IsMoving = false;
                }
                ManageDirection();
                ManageGravity();
                if (IsMoving) ApplyRotation();
                ApplyMovement();
                yield return new WaitForFixedUpdate();
            }
        }

        private bool IsPlayerAlive()
        {
            return m_Player && !m_PlayerComponent.IsDead();
        }
        
        private Vector2 GetChasingMovement()
        {
            var movement = new Vector2(m_Player.transform.position.x - transform.position.x,
                m_Player.transform.position.z - transform.position.z).normalized;
            return movement;
        }
        
        private Vector2 GetToRestingMovement()
        {
            if (Vector3.Distance(m_OriginalPosition, transform.position) < 0.1f) return Vector2.zero;
            var movement = new Vector2(m_OriginalPosition.x - transform.position.x,
                m_OriginalPosition.z - transform.position.z).normalized;
            return movement;
        }

        public void ManageDirection()
        {
            // set direction
            m_MovementDirection = new Vector3(m_MovementBasedOnDestination.x, 0, m_MovementBasedOnDestination.y);
            
            Debug.DrawRay(transform.position, -transform.up * m_RayLenght, Color.red);
            if (Physics.Raycast(transform.position, -transform.up, out m_Hit, m_RayLenght, m_RayMask))
            {
                IsGrounded = true;
                float angleOffset = Vector3.SignedAngle(transform.up, m_Hit.normal, transform.right);
                GroundRotationOffset = Quaternion.AngleAxis(angleOffset, transform.right);
                Debug.DrawRay(transform.position, GroundRotationOffset * m_MovementDirection, Color.green);
                //m_MovementDirection = Quaternion.LookRotation(m_Hit.normal) * m_MovementDirection;
            }
            else
            {
                IsGrounded = m_CharacterController.isGrounded;
                GroundRotationOffset = Quaternion.identity;
            }
            m_MovementDirection.Normalize();

            Direction = m_MovementDirection;
            Debug.DrawRay(transform.position, Direction, Color.red);
            
            // calculate speed
            CurrentSpeed = m_Movement.MaxSpeed * m_Movement.SpeedFactor.Evaluate(m_MovementBasedOnDestination.magnitude);
        }

        public void ApplyRotation()
        {
            if (!IsMoving) return;

            // calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(Direction, transform.up);
            // lerp toward the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                m_Movement.MaxAngularSpeed * Mathf.Deg2Rad * m_Movement.AngularSpeedFactor.Evaluate(Direction.magnitude) * Time.deltaTime);
        }
        
        public void ApplyMovement()
        {
            Debug.DrawRay(transform.position, FullDirection, Color.yellow);
            // move toward the direction with the current speed
            m_CharacterController.Move(FullDirection * Time.deltaTime);
        }
        
        private void ManageGravity()
        {
            if (m_CharacterController.isGrounded && GravityVelocity < 0.0f)
            {
                // if grounded set back gravity velocity to a normal number
                GravityVelocity = -1;
            }
            else
            {
                // if not grounded add gravity
                GravityVelocity += GRAVITY * m_Movement.GravityMultiplier * Time.deltaTime;
            }
        }
    }
}
