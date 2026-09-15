using System;
using UnityEngine;

namespace MiniUI
{
    /// <summary>
    /// 界面动画的基类。
    /// 想做别的动画（缩放、滑动、DoTween），继承它重写 Animate 即可。
    /// </summary>
    public abstract class ScreenAnim : MonoBehaviour
    {
        /// <summary>
        /// 播放动画。
        /// </summary>
        /// <param name="target">要动画的对象（界面的 transform）</param>
        /// <param name="onFinished">动画播完后回调</param>
        public abstract void Animate(Transform target, Action onFinished);
    }
}
