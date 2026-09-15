using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MiniUI
{
    /// <summary>示例面板控制器（常驻 HUD）。</summary>
    public class HUD : UIScreen
    {
        [SerializeField] private Text scoreLabel;

        // 面板是常驻的，通常由外部直接调用刷新方法
        public void SetScore(int score)
        {
            scoreLabel.text = "分数：" + score;
        }

        void Start()
        {
            SetScore(1);
        }
    }
}
