Object:subClass("BasePanel")

BasePanel.panelObj = nil
--模拟一个字典，键为控件名，值为控件本身
BasePanel.controls = {}

BasePanel.isInitEvent = false

function BasePanel:Init(name)
    if self.panelObj == nil then
        --实例化
        local prefab = ABMgr.Instance:LoadRes("ui",name)
        self.panelObj = Instantiate(prefab)
        self.panelObj.transform:SetParent(Canvas,false)
        --找组件
        local allcontorls = self.panelObj:GetComponentsInChildren(typeof(CS.UnityEngine.EventSystems.UIBehaviour))
        for i = 0,allcontorls.Length-1 do
            local controlName = allcontorls[i].name
            --按照规定的名字去找，避免不必要的组件
            if string.find(controlName,"btn") ~= nil or
               string.find(controlName,"tog") ~= nil or
               string.find(controlName,"img") ~= nil or
               string.find(controlName,"sv") ~= nil or
               string.find(controlName,"txt") ~= nil then
                --避免出现一个对象上挂载多个UI控件，出现覆盖问题，所以要存在一个表里
                --利用反射得到控件类名
                --{ btnRole = {Image = 控件,Button = 控件} }
                local typeName = allcontorls[i]:GetType().Name
                if self.controls[allcontorls[i].name] ~= nil then
                    self.controls[controlName][typeName] = allcontorls[i]
                else
                    self.controls[controlName] = {[typeName] = allcontorls[i]}
                end
            end
        end
    end
end 
--根据控件名与控件类型名得到控件
function BasePanel:GetControl(name,typeName)
    if self.controls[name] ~= nil then
        local sameNameControls = self.controls[name]
        if sameNameControls[typeName] ~= nil then
            return sameNameControls[typeName]
        end
    end
    return nil
end

function BasePanel:Show(name)
    self:Init(name)
    self.panelObj:SetActive(true)
end 

function BasePanel:Hide()
    self.panelObj:SetActive(false)
end 