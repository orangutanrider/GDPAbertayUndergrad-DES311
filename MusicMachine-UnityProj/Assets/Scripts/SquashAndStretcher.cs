using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretcher : MonoBehaviour
{
    bool squashAndStretching = false;
    float lerpTimer = 0;
    float lerpTime = 0;
    float lerpA = 1f;
    float lerpB = 1f;

    ScaleMethod scaleMethod = ScaleMethod.ExpandAndShrink;
    LerpType lerpType = LerpType.Elastic;

    public enum ScaleMethod
    {
        ExpandAndShrink,
        SquashAndStretchX,
        SquashAndStretchY
    }

    public enum LerpType
    {
        Elastic,
        Bounce
    }

    void Update()
    {
        if(squashAndStretching == false)
        {
            return;
        }

        if(lerpTimer >= lerpTime)
        {
            transform.localScale = new Vector2(1, 1);
            squashAndStretching = false;
            return;
        }

        lerpTimer = lerpTimer + Time.deltaTime;

        Vector2 newScale = new Vector2(1, 1);
        switch (lerpType)
        {
            case LerpType.Bounce:
                newScale = BounceLerp(lerpA, lerpB, lerpTimer / lerpTime, scaleMethod);
                break;
            case LerpType.Elastic:
                newScale = ElasticLerp(lerpA, lerpB, lerpTimer / lerpTime, scaleMethod);
                break;
        }
        transform.localScale = newScale;
    }

    public void StartSquashAndStretch(float duration, float baseScale, float endScale, ScaleMethod method, LerpType type)
    {
        squashAndStretching = true;
        lerpTimer = 0;
        lerpTime = duration;
        lerpA = baseScale;
        lerpB = endScale;

        lerpType = type;
        scaleMethod = method;
    }

    Vector2 BounceLerp(float a, float b, float t, ScaleMethod method)
    {
        float bounceT = Mathf.Lerp(0f, 1f, LerpFunctions.EaseOutBounce(t));
        if(t >= 0.5f)
        {
            bounceT = Mathf.Lerp(1f, 0f, LerpFunctions.EaseInBounce(t));
        }

        Vector2 returnVector = new Vector2(1, 1);
        switch (method)
        {
            case ScaleMethod.ExpandAndShrink:
                returnVector = ExpandAndShrink(bounceT, a, b);
                break;
            case ScaleMethod.SquashAndStretchY:
                returnVector = SquashAndStretchY(bounceT, a, b);
                break;
            case ScaleMethod.SquashAndStretchX:
                returnVector = SquashAndStretchX(bounceT, a, b);
                break;
        }
        return returnVector;
    }

    Vector2 ElasticLerp(float a, float b, float t, ScaleMethod method)
    {
        float elasticT = Mathf.Lerp(0f, 1f, LerpFunctions.EaseOutElastic(t));
        if (t >= 0.5f)
        {
            elasticT = Mathf.Lerp(1f, 0f, LerpFunctions.EaseInElastic(t));
        }

        Vector2 returnVector = new Vector2(1, 1);
        switch (method)
        {
            case ScaleMethod.ExpandAndShrink:
                returnVector = ExpandAndShrink(elasticT, a, b);
                break;
            case ScaleMethod.SquashAndStretchY:
                returnVector = SquashAndStretchY(elasticT, a, b);
                break;
            case ScaleMethod.SquashAndStretchX:
                returnVector = SquashAndStretchX(elasticT, a, b);
                break;
        }
        return returnVector;
    }

    Vector2 ExpandAndShrink(float t, float a = 1, float b = 2)
    {
        float newScale = Mathf.Lerp(a, b, t);
        return new Vector2(newScale, newScale);
    }

    Vector2 SquashAndStretchY(float t, float a = 1, float b = 2)
    {
        float newYScale = Mathf.Lerp(a, b, t);
        float newXScale = 1 / newYScale;

        return new Vector2(newXScale, newYScale);
    }
    Vector2 SquashAndStretchX(float t, float a = 1, float b = 2)
    {
        float newXScale = Mathf.Lerp(a, b, t);
        float newYScale = 1 / newXScale;

        return new Vector2(newXScale, newYScale);
    }
}
