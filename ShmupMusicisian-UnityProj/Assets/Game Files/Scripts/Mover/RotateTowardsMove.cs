using UnityEngine;
using System;

public class RotateTowardsMove : MonoBehaviour
{
    [Header("Required References")]
    public Mover mover;

    private void Start()
    {
        mover.OnMovementUpdated += RotateToMovement;
    }

    private void RotateToMovement(object sender, EventArgs e)
    {
        transform.up = mover.CurrentMovement;
    }
}