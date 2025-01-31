using System;
using Player;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyMovement:MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Vector3 _lastPosition;
    private MyPlayerHealth _playerHealth;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerHealth = _player.GetComponent<MyPlayerHealth>();
    }

    private void Update()
    {
        if (!_playerHealth.IsDead)
        {
            _agent.SetDestination(_player.transform.position);
            SwitchMoveOrIdel(false);
        }
        else
        {
            SwitchMoveOrIdel(true);
        }
    }

    private void SwitchMoveOrIdel(bool isMove)
    {
        bool isMoving = transform.position != _lastPosition;
        isMoving &= isMoving;
        _animator.SetBool("IsMove", isMoving);
        _lastPosition=transform.position;
    }
}
