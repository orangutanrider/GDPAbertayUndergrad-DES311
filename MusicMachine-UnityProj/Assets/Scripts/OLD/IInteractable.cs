using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public int InteractionPriority { get; set; }

    public void InteractionEvent(InteractionController playerInteractionController);
}
