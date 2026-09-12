MainPanel = {}
--需要做实例化面板对象，处理点击等逻辑
MainPanel.panelObj = nil
MainPanel.btnRole = nil
MainPanel.btnSkill = nil

function MainPanel:Init()
    if self.panelObj == nil then
        --实例化面板对象，找到对应组件，为组件加上事件逻辑
        local prefab = ABMgr.Instance:LoadRes("ui","MainPanel")
        self.panelObj = Instantiate(prefab)
        self.panelObj.transform:SetParent(Canvas,false)

        self.btnRole = self.panelObj.transform:Find("BtnRole"):GetComponent(typeof(Button))
        --self.btnRole.onClick:AddListener(self.BtnRoleClick),这样写BtnRoleClick没有参数传入，相当于nil
        self.btnRole.onClick:AddListener(function()
            self:BtnRoleClick()
        end)
    end
end

function MainPanel:Show()
    self:Init()
    self.panelObj:SetActive(true)
end 

function MainPanel:Hide()
    self.panelObj:SetActive(false)
end 

function MainPanel:BtnRoleClick()
    BagPanel:Show()
end 

print("主面板加载完成")