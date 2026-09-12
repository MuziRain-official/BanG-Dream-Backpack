PlayerData = {}

PlayerData.equips = {}
PlayerData.items = {}
PlayerData.gems = {}

function PlayerData:Init()
    --道具信息只存道具id与道具数量
    table.insert(self.equips,{id = 1,num = 1})
    table.insert(self.equips,{id = 2,num = 1})

    table.insert(self.items,{id = 3,num = 2})
    table.insert(self.items,{id = 4,num = 2})

    table.insert(self.gems,{id = 5,num = 3})
    table.insert(self.gems,{id = 6,num = 3})
end

PlayerData:Init()
print("玩家数据加载完成")