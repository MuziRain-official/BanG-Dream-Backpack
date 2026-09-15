--首先应该把json表从AB包中加载出来
local txt = ABMgr.Instance:LoadRes("json", "ItemData")
--获取json的文本信息进行解析
local itemList = Json.decode(txt.text)
--得到一个类似于list的表，需要通过一张新表去转存，才能用id得到对应信息
ItemData = {}
for _,v in pairs(itemList) do
    ItemData[v.id] = v
end

print("物品数据加载完成")