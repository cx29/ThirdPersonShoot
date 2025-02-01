using System;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MyEnemyManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemiesTypes;
        [SerializeField] private float[] spawnWeights; //敌人权重
        [SerializeField] private float spawnTime = 3f;
        //存放敌人的坐标
        [SerializeField] private Transform[] spawnPoints;
        private List<GameObject> enemies;
        private int numberOfEnemies = 5;
        private MyPlayerHealth _player;
        private List<ScoreType> _scroeArray;
        private MyScoreManager _scoreManager;
        
        /// <summary>
        /// 启用的时候会运行，这样重复开始游戏都会进行以下操作
        /// </summary>
        private void OnEnable()
        {
            _player=GameObject.FindGameObjectWithTag("Player").GetComponent<MyPlayerHealth>();
            enemies = new List<GameObject>();
            _scroeArray = new List<ScoreType>((ScoreType[])Enum.GetValues(typeof(ScoreType)));
            _scoreManager = GetComponent<MyScoreManager>();
            //延迟三秒启动，并且间隔三秒继续启动
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }

        void Spawn()
        {
            if (_player.IsDead || enemies.Count > 10) return;
            int maxAttempts = 10;
            for (int i = 0; i < maxAttempts; i++)
            {
                int spawnPointIndex = Random.Range(0, spawnPoints.Length);
                Vector3 spawnPoint = spawnPoints[spawnPointIndex].position;
                NavMeshHit navMeshHit;
                //判断生成的坐标点是否在ai寻路的可到达范围
                if (NavMesh.SamplePosition(spawnPoint, out navMeshHit, 1.0f, NavMesh.AllAreas))
                {
                    GameObject selectedEnemy = SelectEnemyByWeight(out int index);
                    var successObj=Instantiate(selectedEnemy, navMeshHit.position, spawnPoints[spawnPointIndex].rotation);
                    MyEnemyHealth successHealth = successObj.GetComponent<MyEnemyHealth>();
                    if (successHealth)
                    {
                        successHealth.Initialize(_scroeArray[index], 100);
                        successHealth.OnEnemyDefeated += OnDefeat;
                    }
                    enemies.Add(successObj);
                    return;
                }
            }
        }

        /// <summary>
        /// 根据权重生成敌人
        /// </summary>
        /// <returns></returns>
        GameObject SelectEnemyByWeight(out int index)
        {
            float totalWeight = 0f;
            index = 0;
            foreach (var weight in spawnWeights)
            {
                totalWeight += weight;
            }

            float randomWeight = Random.Range(0f, totalWeight);
            float currWeight = 0f;
            for (int i = 0; i < enemiesTypes.Length; i++)
            {
                currWeight += spawnWeights[i];
                if (randomWeight <= currWeight)
                {
                    index = i;
                    return enemiesTypes[index];
                }
            }
            return enemiesTypes[index];
        }

        private void OnDefeat(ScoreType scoreType)
        {
            _scoreManager.SetScore(scoreType);
            if (enemies.Count>0)
            {
                enemies.RemoveAt(enemies.Count - 1);
            }
        }

        public void ClearAllEnemies()
        {
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }
}