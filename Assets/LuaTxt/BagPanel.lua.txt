BasePanel:subClass("BagPanel")

BagPanel.Content = nil
BagPanel.items = {}
BagPanel.nowType = -1

function BagPanel:Init(name)
    self.base.Init(self,name)
    
    if self.isInitEvent == false then
        self.Content = self:GetControl("svBag","ScrollRect").transform:Find("Viewport"):Find("Content")
        
        self:GetControl("btnClose","Button").onClick:AddListener(function()
            self:Hide()
        end)

        self:GetControl("togEquip","Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(1)
            end
        end)
        self:GetControl("togItem","Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(2)
            end
        end)
        self:GetControl("togGem","Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(3)
            end
        end)
        
        self.isInitEvent = true
    end
    
end 

function BagPanel:Show(name)
    self.base.Show(self,name)
    self.panelObj:SetActive(true)
    if self.nowType == -1 then
        self:ChangeType(1)
    end
end 


function BagPanel:ChangeType(type)
    print("当前界面为"..type)
    if self.nowType == type then
        return
    end

    for i = 1,#self.items do
        self.items[i]:Destroy()
    end
    self.items = {}

    local nowItems = nil
    if type == 1 then
        nowItems = PlayerData.equips
    elseif type == 2 then
        nowItems = PlayerData.items
    else
        nowItems = PlayerData.gems
    end

    for i = 1,#nowItems do
        --local grid = {}
        --local prefab = ABMgr.Instance:LoadRes("ui","ItemGrid")
        --grid.obj = Instantiate(prefab)
        --grid.obj.transform:SetParent(self.Content,false)
        --grid.obj.transform.localPosition = Vector3((i-1)%4 * 150,math.floor((i-1)/4)*150)
        --
        --grid.image = grid.obj.transform:Find("Image"):GetComponent(typeof(Image))
        --grid.text = grid.obj.transform:Find("Text"):GetComponent(typeof(Text))
        --
        --local data = ItemData[nowItems[i].id]
        --local strs = string.split(data.icon,"_")
        --local spriteAtlas = ABMgr.Instance:LoadRes("ui",strs[1])
        --grid.image.sprite = spriteAtlas:GetSprite(strs[2])
        --grid.text.text = nowItems[i].num
        
        --根据数据创建格子对象，实例化和初始化，设置数量，图标
        local grid = ItemGrid:new()
        grid:Init(self.Content,(i-1)%4 * 150,math.floor((i-1)/4)*150)
        grid:InitData(nowItems[i])
        
        table.insert(self.items,grid)
    end
end 
