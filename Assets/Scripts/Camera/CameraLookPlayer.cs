using System;
using UnityEngine;

public class CameraLookPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    private float smoothSpeed= 5f;
    private Vector3 offset;

    private void Start()
    {
        //初始时获取相机和玩家之间的距离
        offset =transform.position - _player.transform.position;
    }

    private void FixedUpdate()
    {
        if(_player)
        //插值计算，让行为更加平滑
        transform.position = Vector3.Lerp(transform.position, _player.transform.position + offset, smoothSpeed*Time.deltaTime);
    }
}
