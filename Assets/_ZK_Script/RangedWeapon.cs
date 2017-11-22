using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DimensionCollapse;

namespace DimensionCollapse
{
    public class RangedWeapon : DimensionCollapse.Weapon
    {
        float InitialV; //发射子弹的初始速度
        float Interval; //武器的射速
        int CurrentChanger; //武器正在使用的子弹数量
        int CurrentChangerCapacity; //武器正在使用的子弹最大容量
        int AlternativeCharger; //武器备用子弹数量
        int AlternativeChargerCapacity; //武器备用子弹最大容量

        public override void Attack()
        {
            GameObject fireBullet = GameObject.FindGameObjectWithTag("FireBullet");
            if (fireBullet != null)
            {
                fireBullet.transform.position = this.transform.TransformPoint(0, 0, 3);
                Vector3 force = fireBullet.transform.TransformDirection(0, 2, 10);

                //给这个火焰子弹添加刚体
                if (fireBullet.GetComponent<Rigidbody>() == null)
                {
                    fireBullet.gameObject.AddComponent<Rigidbody>();
                }
                fireBullet.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("找不到火焰子弹");
            }
        }

    }
}
