using UnityEngine;
using System;

public class RotateTowardsMove : MonoBehaviour
{
    public Transform targetTransform;
    public Mover mover;

    private void Start()
    {
        mover.OnMovementUpdated += RotateToMovement;
    }

    private void RotateToMovement(object sender, EventArgs e)
    {
        targetTransform.up = mover.CurrentMovement;
    }
}