using System;
using Player;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// 玩家移动速度
    /// </summary>
    [SerializeField]
    private float speed=5f;

    private Rigidbody _rigidbody;
    private bool hasMainCamera = false;
    private Animator _ani;
    private MyPlayerHealth _playerHealth;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        hasMainCamera=Camera.main != null;
        _ani = GetComponent<Animator>();
        _playerHealth = GetComponent<MyPlayerHealth>();
    }

    private void FixedUpdate()
    {
        if(_playerHealth.IsDead)return;
            
        var moveHorizontal = Input.GetAxisRaw("Horizontal");
        var moveVertical = Input.GetAxisRaw("Vertical");

        PlayerMove(moveHorizontal, moveVertical);
        PlayerRotate();
        SwitchMoveOrIdel(moveHorizontal, moveVertical);
    }

    void PlayerMove(float moveHorizontal, float moveVertical)
    {
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        Vector3 targetmove = movement.normalized * (speed * Time.deltaTime);
        //normalized归一化向量
        _rigidbody.MovePosition(transform.position + targetmove);
    }

    void PlayerRotate()
    {
        if (hasMainCamera)
        {
            
            //创建一个射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            //查找到对应的Layer 索引,注意：不要用错成NameToLayer了
            var floorLayer=LayerMask.GetMask($"Floor");
            
            //用来承载返回的穿透的物体
            RaycastHit floorHit;
            //maxDistance不使用Mathf.Infinity原因是不使用无穷大减小开销
            bool isTouchFloor= Physics.Raycast(ray, out floorHit, Mathf.Infinity, floorLayer);

            //判断一下是否穿透了物体
            if (!isTouchFloor) return;
            //计算穿透点和当前gameObject的向量差值
            var whereFloor=new Vector3(floorHit.point.x-transform.position.x,0,floorHit.point.z-transform.position.z);

            if (whereFloor.sqrMagnitude <= 0.01f) return;
            //计算四元数  Q=w+xi+yj+zk,其中w是实部，xyz是虚部
            Quaternion lookRotation = Quaternion.LookRotation(whereFloor);
            _rigidbody.MoveRotation(lookRotation);
        }
    }

    void SwitchMoveOrIdel(float h,float v)
    {
        bool isWorking = h != 0 || v != 0;
        _ani.SetBool("IsWalking",isWorking);
    }
}