print("lua主函数脚本启动")
require("InitClassLua")
require("ItemData")
require("PlayerData")
require("BasePanel")
require("MainPanel")
require("BagPanel")
require("ItemGrid")

-- 每次进入热更新场景都调用一次，重新初始化 UI。
-- LuaEnv 常驻（LuaManager DontDestroyOnLoad）：切场景后 require 有缓存不会重跑，
-- 且上一个场景的 Canvas 已销毁、面板 panelObj 也指向已销毁对象，必须重置后再显示。
function InitScene()
    -- 重新绑定当前场景的 Canvas（旧场景的 Canvas 已随场景销毁）
    Canvas = GameObject.Find("Canvas").transform

    -- 清空共享控件表，避免残留旧场景已销毁的组件引用
    BasePanel.controls = {}

    -- 重置主面板：让 Init 重新实例化并重新绑定按钮事件
    MainPanel.panelObj = nil
    MainPanel.isInitEvent = false

    -- 重置背包面板（由主面板按钮再打开，也要清掉旧实例与状态）
    BagPanel.panelObj = nil
    BagPanel.isInitEvent = false
    BagPanel.nowType = -1
    BagPanel.items = {}

    MainPanel:Show("MainPanel")
end