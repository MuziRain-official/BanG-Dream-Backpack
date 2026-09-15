using System.Collections.Generic;
using UnityEngine;

namespace MiniUI
{
    /// <summary>
    /// 窗口层：一次只显示一个窗口，关闭后自动回到上一个。
    ///
    /// 核心是一个“历史栈”(Stack)：打开新窗口就压栈，关闭当前窗口就弹栈，
    /// 然后重新显示栈顶（上一个窗口）。
    /// </summary>
    public class WindowLayer : UILayer
    {
        private readonly Stack<UIScreen> history = new();

        /// <summary>当前显示的窗口（没有则为 null）。</summary>
        public UIScreen Current { get; private set; }

        public override void ShowScreen(UIScreen screen, object props = null)
        {
            // 已经是当前窗口，不做重复操作
            if (Current == screen)
            {
                Debug.LogWarning($"[WindowLayer] 窗口已打开: {screen.ScreenId}");
                return;
            }

            // 隐藏上一个窗口，但保留在历史栈里（等会儿还要回来）
            if (Current != null)
            {
                Current.Hide();
            }

            history.Push(screen);
            screen.Show(props);
            Current = screen;
        }

        public override void HideScreen(UIScreen screen)
        {
            // 窗口只能关闭“当前这个”，否则历史栈会乱
            if (screen != Current)
            {
                Debug.LogError($"[WindowLayer] 只能关闭当前窗口，忽略请求: {screen.ScreenId}");
                return;
            }

            screen.Hide();
            history.Pop();
            Current = null;

            // 栈里还有上一个窗口，重新显示它
            if (history.Count > 0)
            {
                UIScreen prev = history.Peek();
                prev.Show();
                Current = prev;
            }
        }

        public override void HideAll(bool animate = true)
        {
            base.HideAll(animate);
            history.Clear();
            Current = null;
        }
    }
}
