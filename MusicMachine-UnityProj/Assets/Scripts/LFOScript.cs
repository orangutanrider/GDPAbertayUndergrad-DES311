using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFOScript : MonoBehaviour
{
    [Header("Parameters")]
    public LFOParameters lfoParameters;

    [Header("Debug")]
    [SerializeField] float lfoValue = 0;

    float lfoTimer = 0;
    public float LFOValue
    {
        get { return lfoValue; }
    }

    void Update()
    {
        // load parameters
        WaveShape waveShape = lfoParameters.waveShape;
        float frequency = lfoParameters.frequency;
        float amplitude = lfoParameters.amplitude;
        Texture2D texture = lfoParameters.texture;

        // do the stuff
        lfoTimer = lfoTimer + (Time.deltaTime * frequency);

        switch (waveShape) 
        {
            case WaveShape.Sin:
                lfoValue = Mathf.Sin(lfoTimer * Mathf.PI * 2) * amplitude;
                break;
            case WaveShape.Triangle:
                lfoValue = (Mathf.PingPong(lfoTimer, 1f) - 0.5f) * 2f * amplitude;
                break;
            case WaveShape.TextureBased:
                // it reads the texture as a linear line of pixels, and returns the greyscale of pixel it's at as a float times the amplitude
                float resolution = texture.width * texture.height;
                int pixelIndex = Mathf.FloorToInt(Mathf.PingPong(lfoTimer * resolution, resolution));
                Vector2Int pixelPosition = ConvertIndexToPixelPosition(texture, pixelIndex);
                Color pixel = texture.GetPixel(pixelPosition.x, pixelPosition.y);
                lfoValue = (pixel.grayscale * amplitude) - (0.5f * amplitude);
                break;
        }
    }

    Vector2Int ConvertIndexToPixelPosition(Texture2D texture, int pixelIndex)
    {
        // go along a row and then down one
        // effectively reading the texture like its a 1 dimensional array of pixels

        int rowIndex = Mathf.FloorToInt(pixelIndex / (texture.width + 1));
        int columnIndex = pixelIndex % (texture.width + 1);
        return new Vector2Int(rowIndex, columnIndex);
    }
}
