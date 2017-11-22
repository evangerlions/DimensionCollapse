using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionCollapse
{
    public abstract class Item : MonoBehaviour
    {
        String Name; //物品名称
        int ID; //物品ID
        bool Picked; //标识符，物品被捡起为true，没有被捡起为false
    }
}
