using System;
using Player;
using UnityEngine;

namespace Enemy
{
    public class MyEnemyAttack : MonoBehaviour
    {
        private GameObject _player;
        private MyPlayerHealth _playerHealth;
        [SerializeField]
        private float _timeBetweenAttacks=1.5f;
        [SerializeField]
        private float _damage=10;
        private bool _isAttacking=false;

        private float time = 0;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerHealth=_player.GetComponent<MyPlayerHealth>();
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time > _timeBetweenAttacks)
            {
                Attack();
            }
        }

        private void Attack()
        {
            time = 0;
            if (_isAttacking)
            {
                _playerHealth.TakeDamage(_damage);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                _isAttacking = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                _isAttacking =false;
            }
        }
    }
}