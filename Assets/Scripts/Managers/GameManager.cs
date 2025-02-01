using System;
using Managers;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _gameCanvas;
    [SerializeField]
    private GameObject _gamePlayer;
    [SerializeField]
    private Transform _playerPos;
    
    //游戏是否开始
    private bool _isGameRunning= false;
    private MyEnemyManager enemyManager;
    private Camera _currentCamera;
    private CameraLookPlayer _cameraLookPlayer;
    public bool IsGameRunning=>_isGameRunning;

    private void Awake()
    {
        enemyManager = GetComponent<MyEnemyManager>();
    }

    public void StartGame()
    {
        try
        {
            //隐藏所有UI，打开指定UI
            ModifyCanvas(_gameCanvas[1]);

            //生成player
            var player = Instantiate(_gamePlayer, _playerPos.position, _playerPos.rotation);

            var playerHealth = player.GetComponent<MyPlayerHealth>();
            playerHealth.OnPlayerDied += HandlePlayerDied;
            //将player赋值给脚本
            _currentCamera = Camera.main;
            if (_currentCamera!= null)
            {
                _cameraLookPlayer= _currentCamera.GetComponent<CameraLookPlayer>();
                _cameraLookPlayer.SetPlayer(player);
                _cameraLookPlayer.enabled = true;
            }
            //启用敌人生成管理,在玩家生成之后再开启enemyManager
            enemyManager.enabled = true;

            _isGameRunning = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void ModifyCanvas(GameObject canvas)
    {
        foreach (var canva in _gameCanvas)
        {
            if(canva.activeSelf)canva.SetActive(false);
        }
        canvas.SetActive(true);
    }

    public void ShowScoreList()
    {
        
    }


    /// <summary>
    /// 游戏结束方法
    /// </summary>
    private void HandlePlayerDied()
    {
        _isGameRunning = false;
        enemyManager.ClearAllEnemies();
        _cameraLookPlayer.RemovePlayer();
        _cameraLookPlayer.enabled = false;
        enemyManager.enabled = false;
        ModifyCanvas(_gameCanvas[0]);
    }
    
}
