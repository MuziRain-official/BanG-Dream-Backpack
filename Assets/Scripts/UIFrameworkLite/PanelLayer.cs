namespace MiniUI
{
    /// <summary>
    /// 面板层：常驻界面的管理（比如 HUD、血条、小地图）。
    /// 和窗口不同，面板没有“历史栈”，可以多个同时显示，简单显隐即可。
    /// </summary>
    public class PanelLayer : UILayer
    {
        public override void ShowScreen(UIScreen screen, object props = null)
        {
            screen.Show(props);
        }

        public override void HideScreen(UIScreen screen)
        {
            screen.Hide();
        }
    }
}
