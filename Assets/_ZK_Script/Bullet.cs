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
        private Transform flying; //子弹飞行特效，如果有的话在子弹飞行中显示
        private Transform explosion; //子弹爆炸特效，如果有的话在子弹碰撞到其他物体时显示
        private float BulletRealLifeTime;    //子弹的实际生命周期，如果子弹在碰撞后还需要显示爆炸动画的话，那么需要将子弹的原始生命周期加上这个动画的持续时间
        private Boolean isDead; //记录子弹是否已经死亡，防止因超过生命周期而删除的子弹重复多次显示渲染粒子特效
        void Awake()
        {
            initThisBullet();
            //Debug.Log("awake执行!");
        }
        private void initThisBullet()
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

            //绑定特效
            flying = this.transform.Find("Particle_Flying");
            explosion = this.transform.Find("Particle_Explosion");
            //先把爆炸的碰撞体隐藏掉，不然爆炸的碰撞体也会在子弹飞行中引发碰撞检测函数,并顺便计算实际生命周期
            if (explosion != null)
            {
                explosion.GetComponent<Rigidbody>().isKinematic = true; //不设置遵循动力学的话，不能发射
                explosion.GetComponent<Collider>().enabled = false;
                BulletRealLifeTime = BulletLifeTime + explosion.GetComponent<ParticleSystem>().main.duration;
            }
            else
            {
                BulletRealLifeTime = BulletLifeTime;
            }
            this.gameObject.SetActive(false);

        }

        //测试证明调用SetActive不会重新调用Start()
        void Start()
        {
            //Debug.Log("Start调用！");
        }
        void Update()
        {
            //Debug.Log("Update调用");
            if (isFirst)
            { //如果这个子弹是即将发射状态，那么初始化它的生命周期，并显示飞行特效
                currentBulletLifeTime = 0;
                isFirst = false;
                //显示飞行粒子特效,如果有的话
                if (flying != null)
                {
                    flying.GetComponent<ParticleSystem>().Play();
                }
            }
            currentBulletLifeTime += Time.deltaTime;
            if (!isDead) //活着的子弹才进行是否超过生命周期判定
            {
                isOverLifeTime(); //检测是否超过生命周期
            }
        }

        //检测是否超过生命周期,超过就删除此子弹,如果有爆炸特效的话，触发爆炸特效
        private void isOverLifeTime()
        {
            if (currentBulletLifeTime > BulletLifeTime)
            {
                isDead = true;
                StopFlyingAndExplode();
                StartCoroutine(delayDelete(BulletRealLifeTime - BulletLifeTime));
            }
            //Debug.Log("超过生命周期而删除");
        }


        //检测是否进行了碰撞，进行了就删除此子弹,如果有爆炸特效的话，触发爆炸特效
        private void OnCollisionEnter(Collision other)
        {
            StopFlyingAndExplode();
            StartCoroutine(delayDelete(BulletRealLifeTime - BulletLifeTime));
            //Debug.Log("触发了碰撞而删除");
        }

        private IEnumerator delayDelete(float delayTime)
        {
            //等待延迟时间
            for (float i = 0; i < delayTime; i += Time.deltaTime)
            {
                yield return 0;
            }
            this.gameObject.SetActive(false);
            Bullet tempBullet = this;
            bulletList.RemoveLast();
            bulletList.AddFirst(tempBullet);
        }
        public void setInitTransform(Transform initTransform)
        {
            this.transform.position = initTransform.position;
            this.transform.rotation = initTransform.rotation;
            this.transform.eulerAngles = initTransform.eulerAngles;

            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;
            bulletRigidbody.isKinematic = false; //让子弹可以受力而运动
            flying.gameObject.SetActive(true);
            isDead = false;
            isFirst = true;
        }
        //获取子弹生命周期，给武器调用
        public float getBulletRealLifeTime()
        {
            return BulletRealLifeTime;
        }
        public void setBulletList(LinkedList<Bullet> bulletList)
        {
            this.bulletList = bulletList;
        }
        //停止飞行粒子特效并且触发爆炸特效
        private void StopFlyingAndExplode()
        {
            bulletRigidbody.isKinematic = true; //让子弹停止,很关键的代码~不然要加很多的代码来让爆炸位置不动~            
            if (flying != null)
            {
                flying.GetComponent<ParticleSystem>().Stop();
                flying.gameObject.SetActive(false);
                if (explosion != null)
                {
                    explosion.GetComponent<Collider>().enabled = true;
                    explosion.GetComponent<ParticleSystem>().Play();
                    explosion.GetComponent<Collider>().enabled = false;
                }
            }
        }

    }
}