using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DimensionCollapse;
public class Temp_Attack : MonoBehaviour
{
    public RangedWeapon gun;
    public RangedWeapon gun2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (gun != null)
            {
                gun.Attack();
            }
            if (gun2 != null)
            {
                gun2.Attack();
            }

        }

        //临时代码
        if (Input.GetKeyDown(KeyCode.R))
        {

            if (gun != null)
            {
                gun.newReload();
            }
            if (gun2 != null)
            {
                gun2.newReload();
            }
        }
    }
    private void OnGUI()
    {
        if (gun != null)
        {
            GUILayout.TextArea(gun.CurrentChanger + "/" + gun.AlternativeCharger, 200);
        }
        if (gun2 != null)
        {
            GUILayout.TextArea(gun2.CurrentChanger + "/" + gun2.AlternativeCharger, 200);
        }

    }
}
11.27晚更新 V0.03 —— 完善了Wiley粒子特效，修复了若干Bug
Bug修复：
1.某种特定角度子弹穿过墙壁。原因：墙壁没有厚度，使用Cube即可。
2.爆炸动画有一帧不明残留。原因：爆炸子粒子特效设置了Loop属性
功能改进：
1.枪上也会有伤害属性，现在枪和子弹都有伤害属性，并且子弹的伤害是由枪在发射的时候赋予的。这样改的原因是不同的枪即使用同样的子弹，伤害也会不一样，这样也方便以后给枪加上增加伤害的Buff，也符合实际。
2.在实例化子弹前加SetActive(true )，不这样的话，当子弹的Prefab激活栏中不打勾时，就会出现获取不到子弹生命周期的情况，原因是Prefab不打勾不会在Instantiate后调用Awake()！
已知Bug：
1.Wiley的爆炸伤害判定无效。
2.站在人物头上打不到人。
3.枪口伸到人物身体里会触发两次伤害判定？