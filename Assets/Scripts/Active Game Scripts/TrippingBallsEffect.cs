using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class TrippingBallsEffect : MonoBehaviour
{
    public Volume postProcessVolume;
    private LensDistortion lensDistortion;
    private ColorAdjustments colorAdjustments;

    private bool allowTrippyColors;

    void Start()
    {
        allowTrippyColors = GameManager.Instance.GetTrippyColorsOptionEnabled();

        // Get Lens Distortion effect from the Post Processing Volume
        if (postProcessVolume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.intensity.overrideState = true;
            lensDistortion.xMultiplier.overrideState = true;
            lensDistortion.yMultiplier.overrideState = true;
        }

        if(allowTrippyColors)
        {
            // Get Color Adjustments effect
            if (postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                colorAdjustments.hueShift.overrideState = true;
                colorAdjustments.saturation.overrideState = true;
            }
        }
    }

    void Update()
    {
        if (lensDistortion != null)
        {
            float time = Time.time;

            // Smooth looping warp effect
            lensDistortion.intensity.value = Mathf.Lerp(-0.1f, -.75f, Mathf.PingPong(time * 0.8f, 1));

            // X and Y multipliers oscillate independently for extra trippiness
            lensDistortion.xMultiplier.value = Mathf.Lerp(0.8f, 1f, Mathf.PingPong(time * 1.2f, 1));
            lensDistortion.yMultiplier.value = Mathf.Lerp(0.8f, 1f, Mathf.PingPong(time * 1.5f, 1));
        }

        if (colorAdjustments != null && allowTrippyColors)
        {
            float time = Time.time;

            // Shift colors over time (rainbow effect)
            colorAdjustments.hueShift.value = Mathf.PingPong(time * 50f, 360) - 180f;

            // Slightly boost saturation to make colors pop
            colorAdjustments.saturation.value = Mathf.PingPong(time * 2f, 50) - 25f;
        }
    }
}
