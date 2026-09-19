using System.Collections.Generic;
using UnityEngine;

namespace MiniUI
{
    /// <summary>
    /// “层”的基类：负责管理一批界面（用字典记录 ID → 界面），
    /// 并提供按 ID 显示 / 隐藏 / 查询的通用逻辑。
    /// 具体的显示方式（窗口有历史栈，面板没有）交给子类重写。
    /// </summary>
    public abstract class UILayer : MonoBehaviour
    {
        /// <summary>已注册的界面表：ID → 界面</summary>
        protected Dictionary<string, UIScreen> screens = new();

        /// <summary>子类实现：如何显示一个界面（带属性）</summary>
        public abstract void ShowScreen(UIScreen screen, object props = null);

        /// <summary>子类实现：如何隐藏一个界面</summary>
        public abstract void HideScreen(UIScreen screen);

        /// <summary>
        /// 注册界面。注册后界面会被挂到本层下，并按 ID 管理。
        /// </summary>
        public virtual void Register(string id, UIScreen screen)
        {
            if (screens.ContainsKey(id))
            {
                Debug.LogError($"[UILayer] 界面已注册，不能重复注册: {id}");
                return;
            }

            screen.ScreenId = id;
            screen.transform.SetParent(transform, false);
            screen.gameObject.SetActive(false);      // 注册即隐藏，等 Open/Show 时才显示
            screen.CloseRequest += OnCloseRequest;   // 界面请求关闭时转发给本层
            screens.Add(id, screen);
        }

        /// <summary>注销界面。</summary>
        public virtual void Unregister(string id)
        {
            if (!screens.TryGetValue(id, out UIScreen screen))
            {
                return;
            }

            screen.CloseRequest -= OnCloseRequest;
            screens.Remove(id);
        }

        /// <summary>注销并销毁本层所有界面（真正移除，不只是隐藏）。</summary>
        public virtual void RemoveAll()
        {
            foreach (UIScreen screen in screens.Values)
            {
                if (screen != null) Destroy(screen.gameObject);
            }
            screens.Clear();
        }

        /// <summary>按 ID 显示界面。</summary>
        public void ShowById(string id, object props = null)
        {
            if (screens.TryGetValue(id, out UIScreen screen))
            {
                ShowScreen(screen, props);
            }
            else
            {
                Debug.LogError($"[UILayer] 未注册的界面: {id}");
            }
        }

        /// <summary>按 ID 隐藏界面。</summary>
        public void HideById(string id)
        {
            if (screens.TryGetValue(id, out UIScreen screen))
            {
                HideScreen(screen);
            }
            else
            {
                Debug.LogError($"[UILayer] 未注册的界面: {id}");
            }
        }

        /// <summary>隐藏本层所有界面。</summary>
        public virtual void HideAll(bool animate = true)
        {
            foreach (UIScreen screen in screens.Values)
            {
                screen.Hide(animate);
            }
        }

        /// <summary>判断某个 ID 是否已注册。</summary>
        public bool IsRegistered(string id) => screens.ContainsKey(id);

        /// <summary>判断某个 ID 是否正在显示。</summary>
        public bool IsVisible(string id) => screens.TryGetValue(id, out UIScreen s) && s.IsVisible;

        // 界面自己调用 Close() 时，转发为“隐藏本层里的这个界面”
        private void OnCloseRequest(UIScreen screen)
        {
            HideScreen(screen);
        }
    }
}
