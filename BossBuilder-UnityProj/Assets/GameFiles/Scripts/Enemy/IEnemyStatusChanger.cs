using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStatusChanger
{
    void ApplyDamage(AtEnemyDamageData damageData);
}
