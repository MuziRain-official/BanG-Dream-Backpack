using System;
using UnityEngine;

namespace MiniUI
{
    /// <summary>
    /// 所有界面的基类（窗口、面板都继承它）。
    /// 它负责：ID、显隐、进出动画、属性接收，以及留给子类填逻辑的钩子方法。
    ///
    /// 使用方式：写一个类继承它，重写 OnShow / OnHide / OnPropertiesSet 即可。
    /// </summary>
    public abstract class UIScreen : MonoBehaviour
    {
        [Header("动画（可留空）")]
        [Tooltip("显示时播放的动画，如淡入")]
        [SerializeField] private ScreenAnim animIn;

        [Tooltip("隐藏时播放的动画，如淡出")]
        [SerializeField] private ScreenAnim animOut;

        /// <summary>界面 ID（由 Layer 注册时自动赋值）</summary>
        public string ScreenId { get; set; }

        /// <summary>是否正在显示</summary>
        public bool IsVisible { get; private set; }

        /// <summary>打开时传入的属性对象，子类用 (MyData)Props 强转后读取</summary>
        public object Props { get; private set; }

        // 三个事件，由 Layer 订阅：
        public event Action<UIScreen> Shown;         // 显示动画完成
        public event Action<UIScreen> Hidden;        // 隐藏动画完成
        public event Action<UIScreen> CloseRequest;  // 界面自己请求关闭

        /// <summary>
        /// 显示界面。props 是打开时传入的数据，可为 null。
        /// </summary>
        public void Show(object props = null)
        {
            Props = props;
            OnPropertiesSet();   // 先让子类拿到属性
            OnShow();            // 再通知子类“要显示了”

            if (!gameObject.activeSelf)
            {
                PlayAnim(animIn, true, () =>
                {
                    IsVisible = true;
                    Shown?.Invoke(this);
                });
            }
            else
            {
                // 已经激活的话，直接认为显示完成（避免重复播动画）
                IsVisible = true;
                Shown?.Invoke(this);
            }
        }

        /// <summary>
        /// 隐藏界面。animate = false 时跳过动画。
        /// </summary>
        public void Hide(bool animate = true)
        {
            OnHide();   // 通知子类“要隐藏了”（比如清理临时对象）

            PlayAnim(animate ? animOut : null, false, () =>
            {
                IsVisible = false;
                gameObject.SetActive(false);
                Hidden?.Invoke(this);
            });
        }

        /// <summary>
        /// 子类在按钮回调里调用它，请求“关闭自己”。
        /// 具体怎么关由所在的 Layer 决定（窗口层会返回上一个窗口）。
        /// </summary>
        public void Close()
        {
            CloseRequest?.Invoke(this);
        }

        // ==================== 子类可重写的钩子 ====================

        /// <summary>显示时触发。常用于初始化/刷新界面。</summary>
        protected virtual void OnShow() { }

        /// <summary>隐藏时触发。常用于清理、停止逻辑。</summary>
        protected virtual void OnHide() { }

        /// <summary>属性设置后触发。在这里用 (MyData)Props 读取打开时传进来的数据。</summary>
        protected virtual void OnPropertiesSet() { }

        // ==================== 内部实现 ====================

        private void PlayAnim(ScreenAnim anim, bool show, Action onFinished)
        {
            if (anim == null)
            {
                // 没有动画：直接切换激活状态
                gameObject.SetActive(show);
                onFinished?.Invoke();
                return;
            }

            if (show && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            anim.Animate(transform, onFinished);
        }

        protected virtual void OnDestroy()
        {
            // 解引用，避免事件导致残留
            Shown = null;
            Hidden = null;
            CloseRequest = null;
        }
    }
}
