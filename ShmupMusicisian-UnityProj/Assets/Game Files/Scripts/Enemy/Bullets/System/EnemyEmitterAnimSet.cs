using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEmitterAnimSet : MonoBehaviour
{
    public List<EnemyBulletEmitter> aList = new List<EnemyBulletEmitter>();
    public List<EnemyBulletEmitter> bList = new List<EnemyBulletEmitter>();

    public void CallA(int index)
    {
        aList[index].Emit();
    }

    public void CallB(int index)
    {
        bList[index].Emit();
    }

    public void Empty() { }
}
