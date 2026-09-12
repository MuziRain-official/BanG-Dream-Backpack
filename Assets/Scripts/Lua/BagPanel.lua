BagPanel = {}

BagPanel.panelObj = nil
BagPanel.btnClose = nil
BagPanel.togEquip = nil
BagPanel.togItem = nil
BagPanel.togGem = nil
BagPanel.svBag = nil
BagPanel.Content = nil

BagPanel.items = {}
BagPanel.nowType = -1

function BagPanel:Init()
    if self.panelObj == nil then
        local prefab = ABMgr.Instance:LoadRes("ui","BagPanel")
        self.panelObj = Instantiate(prefab)
        self.panelObj.transform:SetParent(Canvas,false)
        
        self.btnClose = self.panelObj.transform:Find("CloseBtn"):GetComponent(typeof(Button))
        
        local group =  self.panelObj.transform:Find("TogGroup")
        self.togEquip = group:Find("TogEquip"):GetComponent(typeof(Toggle))
        self.togItem = group:Find("TogItem"):GetComponent(typeof(Toggle))
        self.togGem = group:Find("TogGem"):GetComponent(typeof(Toggle))
        
        self.svBag = self.panelObj.transform:Find("Scroll View"):GetComponent(typeof(ScrollRect))
        self.Content = self.svBag.transform:Find("Viewport"):Find("Content")
        
        self.btnClose.onClick:AddListener(function()
            self:Hide()
        end)
        
        self.togEquip.onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(1)
            end
        end)
        self.togItem.onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(2)
            end
        end)
        self.togGem.onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(3)
            end
        end)
    end
    
end 

function BagPanel:Show()
    self:Init()
    self.panelObj:SetActive(true)
    if self.nowType == -1 then
        self:ChangeType(1)
    end
end 

function BagPanel:Hide()
    self.panelObj:SetActive(false)
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
