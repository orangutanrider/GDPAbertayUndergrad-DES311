using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerWindowScript : MonoBehaviour
{
    public bool BUTTONGetSprites = false;

    [Header("Required References")]
    public SequencerScript sequencerScript;
    public SpriteRenderer[] windowLightSpriteRenderers;
    public SpriteRenderer windowBackgroundSpriteRenderer;

    [Header("Parameters")]
    public SequencerWindowParameters windowParameters;
    [Space]
    public BeatType triggerOnBeatType = BeatType.A;
    public int triggerOnIndex = 0;

    bool on = false;
    Vector3 baseScale = new Vector3(1, 1, 1);

    #region Tools
    void OnValidate()
    {
        GetSpritesButton();
    }

    void GetSpritesButton()
    {
        if(BUTTONGetSprites == false) { return; }
        BUTTONGetSprites = false;

        windowLightSpriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
    }
    #endregion  

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // load window paramaters (this is to just make te code more readable)
        Color offColor = windowParameters.offColor;
        Color onColor = windowParameters.onColor;
        float flickerScaleEffector = windowParameters.flickerScaleEffector;
        float baseOnValue = windowParameters.baseOnValue;

        float flicker = baseOnValue + Mathf.Abs(sequencerScript.FlickerLFOValue);
        bool isOn = true;

        foreach (SpriteRenderer windowLightSpriteRenderer in windowLightSpriteRenderers)
        {
            // do the stuff 
            Color windowLightColor = new Color(windowLightSpriteRenderer.color.r, windowLightSpriteRenderer.color.g, windowLightSpriteRenderer.color.b, windowLightSpriteRenderer.color.a);
           
            if (on == false)
            {
                isOn = false;
                windowLightSpriteRenderer.color = new Color(windowLightColor.r, windowLightColor.g, windowLightColor.b, 0);
                continue;
            }
            windowLightSpriteRenderer.color = new Color(windowLightColor.r, windowLightColor.g, windowLightColor.b, flicker);
        }
        if (isOn == false)
        {
            windowBackgroundSpriteRenderer.color = offColor;
            return;
        }

        windowBackgroundSpriteRenderer.color = new Color(onColor.r, onColor.g, onColor.b, flicker);

        transform.localScale = baseScale + (new Vector3(1, 1, 1) * (flicker * flickerScaleEffector));
    }

    public void RecieveRelayedBeatTrigger(RhythmBeat beat)
    {
        if(beat.beatType != triggerOnBeatType || beat.triggeredAtIndex != triggerOnIndex)
        {
            on = false;
            return;
        }

        on = true;
    }
}
