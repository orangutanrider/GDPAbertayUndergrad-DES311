using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExternalMovement : MonoBehaviour
{
    // This script handles things that are extrenal to, and yet, move the player
    // Stuff like bounce pads

    [Header ("Required References")]
    public PlayerMovement playerMovement;
    public Rigidbody2D playerRB2D;

    [Header("Parameters")]
    public BreatheInWhirlwindParameters whirlwindParameters;

    List<BreatheInWhirlwindScript> effectingWhirlwinds = new List<BreatheInWhirlwindScript>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == TagReferences.breatheInWhirlwindTag)
        {
            effectingWhirlwinds.Add(collision.gameObject.GetComponent<BreatheInWhirlwindScript>());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == TagReferences.breatheInWhirlwindTag)
        {
            effectingWhirlwinds.Remove(collision.gameObject.GetComponent<BreatheInWhirlwindScript>());
        }
    }

    void FixedUpdate()
    {
        // Whirlwind stuff
        if(effectingWhirlwinds.Count > 0 && playerMovement.Grounded == false)
        {
            playerMovement.GravityOverrideDuration = whirlwindParameters.gravityOverrideDuration;
        }
        foreach (BreatheInWhirlwindScript whirlwind in effectingWhirlwinds)
        {
            BreatheInWhirlwind(whirlwind);
        }
    }

    public void BreatheOutExplosion(Vector3 expolsionCenter, float explosionForceMagnitude)
    {
        if(playerMovement.Grounded == true)
        {
            playerRB2D.velocity = new Vector2(playerRB2D.velocity.x, 0);
        }

        Vector2 explosionPushDirection = (transform.position - expolsionCenter).normalized;
        Vector2 explosionForce = explosionPushDirection * explosionForceMagnitude;
        playerRB2D.AddForce(explosionForce, ForceMode2D.Impulse);
    }

    void BreatheInWhirlwind(BreatheInWhirlwindScript whirlwindScript)
    {
        // load
        float whirlwindYDrag = whirlwindParameters.yDrag;

        // y drag
        float yDragForce = playerRB2D.velocity.y * playerRB2D.velocity.y * whirlwindYDrag;
        float dragDirection = 1;
        if(playerRB2D.velocity.y > 0) { dragDirection = -1; }
        playerRB2D.AddForce(new Vector2(0, yDragForce * dragDirection), ForceMode2D.Force);

        // wind force
        Vector2 whirlwindPushForce = new Vector2(whirlwindScript.WindForce.x, 0);
        if(playerMovement.Grounded == false) 
        {
            whirlwindPushForce = whirlwindPushForce + new Vector2(0, whirlwindScript.WindForce.y);
        }
        playerRB2D.AddForce(whirlwindPushForce, ForceMode2D.Force);
    }
}
