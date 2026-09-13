Object:subClass("ItemGrid")

ItemGrid.obj = nil
ItemGrid.image = nil
ItemGrid.text = nil

function ItemGrid:Init(father,posx,posy)
    local prefab = ABMgr.Instance:LoadRes("ui","ItemGrid")
    self.obj = Instantiate(prefab)
    self.obj.transform:SetParent(father,false)
    self.obj.transform.localPosition = Vector3(posx,posy,0)

    self.image = self.obj.transform:Find("img"):GetComponent(typeof(Image))
    self.text = self.obj.transform:Find("txt"):GetComponent(typeof(Text))
end 

function ItemGrid:InitData(data)
    local itemData = ItemData[data.id]
    local strs = string.split(itemData.icon,"_")
    local spriteAtlas = ABMgr.Instance:LoadRes("ui",strs[1])
    self.image.sprite = spriteAtlas:GetSprite(strs[2])
    self.text.text = data.num
end 

function ItemGrid:Destroy()
    GameObject.Destroy(self.obj)
    self.obj = nil
end

