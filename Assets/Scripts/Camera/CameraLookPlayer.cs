using System;
using UnityEngine;

public class CameraLookPlayer : MonoBehaviour
{
    // [SerializeField] private GameObject _gamePos;
    private GameObject _player;
    private float smoothSpeed= 5f;
    private Vector3 offset;

    private void FixedUpdate()
    {
        if (_player)
        {
            //插值计算，让行为更加平滑
            transform.position = Vector3.Lerp(
                transform.position,
                _player.transform.position + offset,
                smoothSpeed * Time.deltaTime
            );
        }

    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
        offset = transform.position -_player.transform.position;
    }

    public void RemovePlayer()
    {
        _player = null;
        offset=Vector3.zero;
    }
}
