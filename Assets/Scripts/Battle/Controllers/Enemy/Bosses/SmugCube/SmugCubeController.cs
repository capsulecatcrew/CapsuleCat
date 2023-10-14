using Enemy.Attacks;
using Enemy.Bosses.Attacks.Lasers;
using UnityEngine;

namespace Enemy.Bosses
{
    public class SmugCubeController : MonoBehaviour
    {
        [Header("Transforms")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private GameObject cannonLeft;
        [SerializeField] private GameObject cannonRight;

        [Header("Expressions")]
        [SerializeField] private GameObject smugFace;
        [SerializeField] private GameObject angryFace;
        [SerializeField] private GameObject painFace;
        [SerializeField] private float faceTime = 0.32f;
        private float _faceTimer;

        [Header("Controllers")]
        [SerializeField] private EnemyController enemyController;
        [SerializeField] private BasicBulletController enemyBulletController;
        [SerializeField] private SmugCubeLaserController enemyLaserController;

        public void OnEnable()
        {
            enemyController.OnEnemyPrimaryHealthChanged += HandleAttacked;
            enemyBulletController.OnAttack += HandleAttack;
            enemyLaserController.OnAttack += HandleAttack;
            enemyController.OnEnemyDeath += Die;
        }

        public void Update()
        {
            var position = playerTransform.position;
            gameObject.transform.LookAt(position);
            cannonLeft.transform.LookAt(position);
            cannonRight.transform.LookAt(position);

            UpdateFaceTimer(Time.deltaTime);
        }

        private void UpdateFaceTimer(float deltaTime)
        {
            if (smugFace.activeSelf) return;
            if (_faceTimer <= 0)
            {
                ResetFace();
                return;
            }
            _faceTimer -= deltaTime;
        }

        private void HandleAttack()
        {
            smugFace.SetActive(false);
            painFace.SetActive(false);
            angryFace.SetActive(true);
            _faceTimer = faceTime;
        }

        private void HandleAttacked(float ignored)
        {
            smugFace.SetActive(false);
            angryFace.SetActive(false);
            painFace.SetActive(true);
            _faceTimer = faceTime;
        }

        private void ResetFace()
        {
            painFace.SetActive(false);
            angryFace.SetActive(false);
            smugFace.SetActive(true);
        }

        private void Die()
        {
            smugFace.SetActive(false);
            angryFace.SetActive(false);
            painFace.SetActive(false);
            enemyController.OnEnemyPrimaryHealthChanged -= HandleAttacked;
            enemyBulletController.OnAttack -= HandleAttack;
            enemyLaserController.OnAttack -= HandleAttack;
            enemyController.OnEnemyDeath -= Die;
        }
    }
}