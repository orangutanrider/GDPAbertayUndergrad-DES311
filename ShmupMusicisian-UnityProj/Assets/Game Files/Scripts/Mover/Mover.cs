using UnityEngine;
using System;

public abstract class Mover : MonoBehaviour
{
    public abstract event EventHandler OnMovementUpdated;

    public abstract Vector2 CurrentMovement { get; }

    // A move input recieved and acted upon per update
    public abstract void UpdateMove(Vector2 moveInput);

    // A move input recieved once
    // The movement caused should be as if UpdateMove() kept getting called in the same direction 
    public abstract void OneShotMove(Vector2 moveInput);
}