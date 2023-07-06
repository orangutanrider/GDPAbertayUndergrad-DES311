using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyHittable : MonoBehaviour
{
    // these are linked to status changers, the status changers themselves contain a list of these each
    public EnemyStatusChanger StatusChanger { get; set; }
}
