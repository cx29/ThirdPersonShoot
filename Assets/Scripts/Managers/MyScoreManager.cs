using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace Managers
{
    public class MyScoreManager : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;

        private int _score;


        public int Score
        {
            get => _score;
        }

        public void SetScore(ScoreType score)
        {
            _score += (int)score;
            _scoreText.text =$"SCORE : {_score}";
        }
    }

    public enum ScoreType
    {
        normal = 10,
        Middle = 20,
        Boss = 50,
    };
}