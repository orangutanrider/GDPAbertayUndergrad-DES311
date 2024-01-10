using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalYSorter : MonoBehaviour
{
    [System.Serializable]
    public class SpriteRenderersWithOffset
    {
        public List<SpriteRenderer> spriteRenderers;
        public int offset = 0;
    }

    #region Parameters
    [Header("Select SpriteRenderer(s)")]
    public List<SpriteRenderersWithOffset> spriteRenderers = new List<SpriteRenderersWithOffset>();

    [Header("Parameters")]
    public bool movable = false;[Tooltip("If it isn't movable, the sorting order will only be updated once (on start)")]
    public float sortingPointOffset = 0;
    [Space]
    public bool displayGizmos = false;
    public bool setSortingOrderButton = false;
    #endregion

    #region Variables
    public const float sortingOrderMultiply = -1000;
    // sorting orders are int numbers so the yPosition has to be multiplied to get enough granularity
    // sorting orders are capped at 32767 and -32767, so the multiply can't be too large

    public const float gizmoRadius = 0.1f;
    #endregion

    // This draws debug visuals 
    void OnDrawGizmos()
    {
        if (displayGizmos == false)
        {
            return;
        }
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + sortingPointOffset), gizmoRadius);
        Gizmos.DrawLine(new Vector3(-1000, transform.position.y + sortingPointOffset), new Vector3(1000, transform.position.y + sortingPointOffset, 0));
    }

    // this gets triggered whenever the component is interacted with via us (not the player)
    // it executes while in edit mode
    void OnValidate()
    {
        // this is a hacky cheap way of making a button
        if (setSortingOrderButton == true)
        {
            setSortingOrderButton = false;
            SetAllSpriteRendererSortingOrdersTo(CalculateSortingOrder(transform.position.y));
        }
    }

    void Start()
    {
        SetAllSpriteRendererSortingOrdersTo(CalculateSortingOrder(transform.position.y));
    }

    void Update()
    {
        if (movable == true)
        {
            SetAllSpriteRendererSortingOrdersTo(CalculateSortingOrder(transform.position.y));
        }
    }

    int CalculateSortingOrder(float yPosition)
    {
        return Mathf.RoundToInt((yPosition + sortingPointOffset) * sortingOrderMultiply);
    }

    void SetAllSpriteRendererSortingOrdersTo(int newSortingOrder)
    {
        foreach (SpriteRenderersWithOffset spriteRenderersWithOffset in spriteRenderers)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderersWithOffset.spriteRenderers)
            {
                spriteRenderer.sortingOrder = newSortingOrder + spriteRenderersWithOffset.offset;
            }
        }
    }
}
