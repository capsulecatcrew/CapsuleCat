using Battle.Hitboxes;
using UnityEngine;

namespace Battle
{
    public abstract class LaserController : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private Firer firer;
        [SerializeField] private DamageType damageType;
        private float _damage;
        private bool _isTracking;
        [SerializeField] private HitboxTrigger hitbox;
        [SerializeField] private Transform target;

        [Header("Animator")]
        [SerializeField] private Animator animator;
        private static readonly int LockOnTrigger = Animator.StringToHash("Lock On");
        private static readonly int FireTrigger = Animator.StringToHash("Fire");
        private static readonly int FinishTrigger = Animator.StringToHash("Finish");

        [SerializeField] private float chargingDuration = 0.52f;
        [SerializeField] private float targetLockDuration = 0.52f;
        [SerializeField] private float firingDuration = 2.02f;
        [SerializeField] private float finishDuration = 0.42f;
        
        private float _chargingTimer;
        private float _targetLockTimer;
        private float _firingTimer;
        private float _finishTimer;

        private int _mode;

        /// <summary>
        /// Initialise a laser for tracking enemy attack.
        /// </summary>
        public void Init(float damage, Transform target = null, float duration = 2.02f, bool isTracking = false)
        {
            _damage = damage;
            this.target = target;
            firingDuration = duration;
            _isTracking = isTracking;
            InitTimers();
        }

        /// <summary>
        /// Initialise a laser for non tracking enemy attack.
        /// </summary>
        public void Init(float damage, Vector3 targetPosition, float duration = 5.02f)
        {
            _damage = damage;
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
            firingDuration = duration;
            InitTimers();
        }

        /// <summary>
        /// Initialise a player for player attack.
        /// Laser will have 0.25x charge up time and 0 target lock time.
        /// </summary>
        public void Init(Firer player, float damage, Vector3 origin, Vector3 forward)
        {
            transform.position = origin;
            firer = player;
            _damage = damage;
            transform.forward = forward;
            firingDuration = float.MaxValue;
            InitTimers();
            _chargingTimer = chargingDuration / 4;
            _targetLockTimer = 0.02f;
        }

        private void InitTimers()
        {
            _chargingTimer = chargingDuration;
            _targetLockTimer = targetLockDuration;
            _firingTimer = firingDuration;
            _finishTimer = finishDuration;
        }

        public void FixedUpdate()
        {
            if (target != null && (_isTracking || _chargingTimer > 0)) SetTargetPosition(target.position);
            UpdateTimers();
            UpdateMode();
        }

        public void OnEnable()
        {
            FireLaser();
            hitbox.HitboxStay += OnHitBoxStay;
        }

        public void OnDisable()
        {
            hitbox.HitboxStay -= OnHitBoxStay;
        }

        private void UpdateTimers()
        {
            if (_chargingTimer > 0)
            {
                _chargingTimer -= Time.deltaTime;
                return;
            }
            if (_targetLockTimer > 0)
            {
                _targetLockTimer -= Time.deltaTime;
                return;
            }
            if (_firingTimer > 0)
            {
                _firingTimer -= Time.deltaTime;
                return;
            }
            if (_finishTimer > 0)
            {
                _finishTimer -= Time.deltaTime;
            }
        }

        private void UpdateMode()
        {
            var mode = 0;
            if (_chargingTimer <= 0) mode = 1;
            if (_targetLockTimer <= 0) mode = 2;
            if (_firingTimer <= 0) mode = 3;
            if (_finishTimer <= 0) mode = 4;
            if (_mode == mode) return;
            _mode = mode;
            switch (_mode)
            {
                case 1:
                    animator.SetTrigger(LockOnTrigger);
                    break;
                case 2:
                    animator.SetTrigger(FireTrigger);
                    PlayFiringSound();
                    break;
                case 3:
                    animator.SetTrigger(FinishTrigger);
                    break;
                case 4:
                    gameObject.SetActive(false);
                    break;
            }
        }
        
        private void FireLaser()
        {
            PlayChargingSound();
        }

        private void SetTargetPosition(Vector3 targetPos)
        {
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
        }

        public void SetTargetForward(Vector3 targetForward)
        {
            transform.forward = targetForward;
        }

        public void SetLaserOrigin(Vector3 origin)
        {
            transform.position = origin;
        }

        private void OnHitBoxStay(Collider other)
        {
            var otherHitbox = other.gameObject.GetComponent<Hitbox>();
            if (otherHitbox == null) return;
            otherHitbox.Hit(firer, _damage, damageType);
        }

        protected abstract void PlayChargingSound();
        protected abstract void PlayFiringSound();
    }
}