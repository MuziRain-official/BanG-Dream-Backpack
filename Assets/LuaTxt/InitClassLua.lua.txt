
require("Object")
require("SplitTools")
Json = require("JsonUtility")

GameObject = CS.UnityEngine.GameObject
Instantiate = CS.UnityEngine.Object.Instantiate
Resources = CS.UnityEngine.Resources
Transform = CS.UnityEngine.Transform
RectTransform = CS.UnityEngine.RectTransform
SpriteAtlas = CS.UnityEngine.SpriteAtlas
TextAsset = CS.UnityEngine.TextAsset
Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2
UI = CS.UnityEngine.UI
Canvas = GameObject.Find("Canvas").transform
Image = UI.Image
Text = UI.Text
Button = UI.Button
Toggle = UI.Toggle
ScrollRect = UI.ScrollRect
ABMgr = CS.ABResManager

print("已完成初始化加载")