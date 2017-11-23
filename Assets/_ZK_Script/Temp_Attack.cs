using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DimensionCollapse;
public class Temp_Attack : MonoBehaviour
{
    public Weapon gun;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            gun.Attack();
        }
     
      //临时代码
            if (Input.GetKeyDown(KeyCode.R))
            {
                RangedWeapon gun2 =(RangedWeapon) gun;
                gun2.newReload();    
            }
    }
}
