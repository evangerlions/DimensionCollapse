using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionCollapse
{
    public class Bullet : MonoBehaviour
    {
        public String BulletName; //子弹的名称
        public int ID; //子弹的ID
        public float BulletLifeTime; //子弹的生命周期，超过这个时间的子弹，将进行删除
        public float damage; //子弹的伤害值
        private float currentBulletLifeTime; //用于记录当前子弹生命时间
        private bool isFirst; //用于记录这个子弹是否处于即将发射状态
        private LinkedList<Bullet> bulletList; //这个链表和RangedWeapon中武器子弹列表相同，用于更新子弹状态

        private Rigidbody bulletRigidbody; //辅助归零子弹速度和角速度，节省系统开销
        private Transform ini; //调试用
        void Awake()
        {
            //子弹没有碰撞体的话，警告!
            if (this.gameObject.GetComponent<Collider>() == null)
            {
                Debug.Log("子弹没有添加碰撞体！");
            }
            //子弹没有刚体的话，警告！
            if (this.gameObject.GetComponent<Rigidbody>() == null)
            {
                Debug.Log("子弹没有添加刚体！");
            }
            bulletRigidbody = GetComponent<Rigidbody>();
            isFirst = true;
            this.gameObject.SetActive(false);
            //     Debug.Log("awake执行!");
        }
        //测试证明调用SetActive不会重新调用Start()
        void Start()
        {
            //   Debug.Log("Start调用！");
        }
        void Update()
        {
            //   Debug.Log("Update调用");
            if (isFirst)
            { //如果这个子弹是即将发射状态，那么初始化它的生命周期
                currentBulletLifeTime = 0;
                isFirst = false;
            }
            currentBulletLifeTime += Time.deltaTime;
            isOverLifeTime(); //检测是否超过生命周期
        }

        //检测是否超过生命周期,超过就删除此子弹
        private void isOverLifeTime()
        {
            if (currentBulletLifeTime > BulletLifeTime)
            {
                this.gameObject.SetActive(false);
                Bullet tempBullet = this;
                bulletList.RemoveLast();
                bulletList.AddFirst(tempBullet);
                //      Debug.Log("超过生命周期而删除");
            }
        }

        //检测是否进行了碰撞，进行了就删除此子弹
        private void OnCollisionEnter(Collision other)
        {

            this.gameObject.SetActive(false);
            Bullet tempBullet = this;
            bulletList.RemoveLast();
            bulletList.AddFirst(tempBullet);
       /*     foreach (ContactPoint contact in other.contacts)
            {
                Debug.DrawLine(ini.position, contact.point, Color.black, 20000, false);
                Debug.DrawRay(contact.point, contact.normal, Color.yellow);
            } */
         //   Debug.Log("触发了碰撞而删除");
        }

        public void setInitTransform(Transform initTransform)
        {
            //    Debug.Log("init执行!");
            ini = initTransform;
            this.transform.position = initTransform.position;
            this.transform.rotation = initTransform.rotation;
            this.transform.eulerAngles = initTransform.eulerAngles;

            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;

            isFirst = true;
        }
        //获取子弹生命周期，给武器调用
        public float getBulletLifeTime()
        {
            return BulletLifeTime;
        }
        public void setBulletList(LinkedList<Bullet> bulletList)
        {
            this.bulletList = bulletList;
        }
    }
}