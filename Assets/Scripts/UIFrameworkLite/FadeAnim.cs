using System;
using System.Collections;
using UnityEngine;

namespace MiniUI
{
    /// <summary>
    /// 淡入淡出动画（用 CanvasGroup 控制透明度）。
    ///
    /// 用法：给界面 Prefab 挂两个 FadeAnim：
    ///   - 一个 fadeOut = false 作为“显示动画”(AnimIn)
    ///   - 一个 fadeOut = true  作为“隐藏动画”(AnimOut)
    /// </summary>
    public class FadeAnim : ScreenAnim
    {
        [SerializeField] private float duration = 0.3f;

        [Tooltip("true = 淡出（隐藏用），false = 淡入（显示用）")]
        [SerializeField] private bool fadeOut;

        public override void Animate(Transform target, Action onFinished)
        {
            CanvasGroup cg = target.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = target.gameObject.AddComponent<CanvasGroup>();
            }

            StartCoroutine(Run(cg, onFinished));
        }

        private IEnumerator Run(CanvasGroup cg, Action onFinished)
        {
            float start = fadeOut ? 1f : 0f;
            float end = fadeOut ? 0f : 1f;

            cg.alpha = start;
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                cg.alpha = Mathf.Lerp(start, end, t / duration);
                yield return null;
            }

            cg.alpha = end;
            onFinished?.Invoke();
        }
    }
}
