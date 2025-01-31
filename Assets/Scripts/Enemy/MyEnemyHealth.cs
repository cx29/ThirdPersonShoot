using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyHealth:MonoBehaviour
{
    private ScoreType _score;
    private float _health=100;
    //受击回退距离
    [SerializeField]
    private float _backDistance=5f;
    [SerializeField]
    private float knockBackDuration = 0.5f;
    [SerializeField]
    private ParticleSystem _hitParticles;
    
    private MyEnemyMovement movement;
    private NavMeshAgent _agent;
    private Animator _animator;
    private float _currHealth;
    //敌人被击败后事件
    public event Action<ScoreType> OnEnemyDefeated;

    public float CurrentHealth
    {
        get => _currHealth;
    }
    private Rigidbody _rigidbody;
    private bool _isDead=false;

    public void Initialize(ScoreType score, float health)
    {
        _score = score;
        _health = health;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currHealth = _health;
        _animator = GetComponent<Animator>();
        movement = GetComponent<MyEnemyMovement>();
        _agent = GetComponent<NavMeshAgent>();
        //从子物体中获取组件
        _hitParticles= GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (_currHealth<= 0&&!_isDead)
        {
            Death();
        }
    }

    void Death()
    {
        _isDead = true;
        movement.enabled = false;
        _agent.enabled = false;
        //当前的rigibody不受周围物理系统的影响，减小开销
        _rigidbody.isKinematic = true;
        OnEnemyDefeated?.Invoke(_score);
        _animator.SetTrigger("Death");
    }

    /// <summary>
    /// 受击函数
    /// </summary>
    /// <param name="damage">伤害</param>
    /// <param name="fromWhere">从哪里来的攻击</param>
    public void GetHit(float damage, Vector3 fromWhere,Vector3 hitPos)
    {
        _currHealth -= damage;
        //计算受击方向
        Vector3 knockDirection=(transform.position - fromWhere).normalized;
        _rigidbody.AddForce(knockDirection * _backDistance, ForceMode.Impulse);
        //设置一个定时器，逐渐停止
        StartCoroutine(StopKnockBack());
        _hitParticles.transform.position =hitPos;
        _hitParticles.Play();
    }

    private IEnumerator StopKnockBack()
    {
        yield return new WaitForSeconds(knockBackDuration);
        //逐渐减速
        while (_rigidbody.linearVelocity.magnitude>0.1f)
        {
            _rigidbody.linearVelocity= Vector3.Lerp(_rigidbody.linearVelocity, Vector3.zero, Time.deltaTime * 5f);
            yield return null;
        }
        _rigidbody.linearVelocity = Vector3.zero;
    }

    public void StartSinking()
    {
        Destroy(gameObject,2f);
    }
}
