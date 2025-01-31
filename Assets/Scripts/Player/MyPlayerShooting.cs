using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class MyPlayerShooting : MonoBehaviour
{
    [SerializeField]
    private float _damage = 5f;
    //开枪声音
    private AudioSource _audioSource;
    //开枪冷却时间
    private float _shootCoolDown= 0.15f;
    //开枪的光
    private Light _light;
    private ParticleSystem _particleSystem;
    private LineRenderer _lineRenderer;
    //效果显示的时间
    private float _effectTime=0.2f;
    //计时器
    private float time = 0;
    
    private Ray _ray;
    private RaycastHit _hit;
    private int layerMask;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _light = GetComponent<Light>();
        _lineRenderer = GetComponent<LineRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();
        layerMask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && time > _shootCoolDown)
        {
            Shooting();
            time = 0;
        }

        if (time >= _effectTime * _shootCoolDown)
        {
            _light.enabled = false;
            _lineRenderer.enabled = false;
        }
    }

    private void Shooting()
    {
        _audioSource.Play();
        _light.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        var enemy = IsHitEnemy();
        var distance=transform.position + transform.forward * 100;
        if (enemy is not null)
        {
            //如果击中敌人则射线的长度到敌人为止
            distance = enemy.position;
            var enemyObj = enemy.gameObject.GetComponent<MyEnemyHealth>();
            enemyObj.GetHit(_damage,transform.position,enemy.position);
        }
        _lineRenderer.SetPosition(1,distance );
        _lineRenderer.enabled = true;
        _particleSystem.Play();
    }

    private Transform IsHitEnemy()
    {
        //这里定义这个ray的话，是从main camera发射的，不适用当前射击
        // _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //这里的射线检测应该直接从角色开始射出j k w
        _ray.origin=transform.position;
        _ray.direction = transform.forward;
        bool isHit=Physics.Raycast(_ray, out _hit,100, layerMask);
        if (!isHit) return null;
        var enemyTransform=_hit.transform;
        return enemyTransform;
    }
}
