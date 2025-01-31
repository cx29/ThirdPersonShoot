using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class MyPlayerHealth : MonoBehaviour
    {
        private static readonly int Dead = Animator.StringToHash("IsDead");

        [SerializeField]
        private float health = 100;
        [SerializeField]
        private Text _healthText;
        [SerializeField]
        private float _currHealth;
        [SerializeField]
        private Image _hitFill;
        private bool _isDead=false;
        private Animator _animator;
        //标准颜色
        private Color hitColor = new(255f / 255f, 0 / 255f, 13 / 255f, 36 / 255f);
        private bool _isHit = false;
        
        
            
        public bool IsDead => _isDead;

        public float CurrHealth
        {
            get => _currHealth;
        }
        

        private void Awake()
        {
            _currHealth = health;
            _animator = GetComponent<Animator>();
            _hitFill.color = Color.clear;
        }

        private void Update()
        {
            _healthText.text = CurrHealth < 0 ? "0" : CurrHealth.ToString();
            if (_isHit)
            {
                _hitFill.color = hitColor;
            }
            else
            {
                _hitFill.color = Color.Lerp(_hitFill.color, Color.clear, 0.5f * Time.deltaTime);
            }

            _isHit = false;
            if (_currHealth <= 0 && !_isDead)
            {
                StartCoroutine(Death());
            }
        }

        public void TakeDamage(float damage)
        {
            _currHealth -= damage;
            _isHit = true;
            if (_currHealth <= 0 && !_isDead)
            {
                StartCoroutine(Death());
            }
        }

        private IEnumerator Death()
        {
            _animator.SetTrigger(Dead);
            _isDead = true;
            yield return new WaitForSeconds(2.0f);
            _hitFill.color = Color.Lerp(_hitFill.color, Color.clear, 0.5f * Time.deltaTime);
            Destroy(gameObject);
        }
    }
}