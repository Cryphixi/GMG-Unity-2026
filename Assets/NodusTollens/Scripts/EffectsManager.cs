using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EffectsManager : MonoBehaviour
{
    [Header("Overlay objects")]
    public Image shadeImage;   // ShadeOverlay
    public Image burstImage;   // EffectBurst

    [Header("Animation frames (drag in numeric order!)")]
    public Sprite[] glitchFrames;  // Glitch_Pack1..8
    public Sprite[] waterFrames;   // Water_Line_Pack1..7

    [Header("Feel")]
    public float frameTime = 0.07f; // seconds per frame
    public int loops = 2;           // how many times a burst repeats

    void Start()
    {
        if (shadeImage != null) shadeImage.gameObject.SetActive(false);
        if (burstImage != null) burstImage.gameObject.SetActive(false);
    }

    public void PlayGlitch() { StartCoroutine(PlayBurst(glitchFrames)); }
    public void PlayWater()  { StartCoroutine(PlayBurst(waterFrames)); }

    IEnumerator PlayBurst(Sprite[] frames)
    {
        if (frames == null || frames.Length == 0 || burstImage == null) yield break;
        burstImage.gameObject.SetActive(true);
        for (int l = 0; l < loops; l++)
        {
            foreach (Sprite f in frames)
            {
                burstImage.sprite = f;
                yield return new WaitForSeconds(frameTime);
            }
        }
        burstImage.gameObject.SetActive(false);
    }

    public void ShowShade(Sprite s)
    {
        if (shadeImage == null || s == null) return;
        shadeImage.sprite = s;
        shadeImage.gameObject.SetActive(true);
    }

    public void ClearShade()
    {
        if (shadeImage != null) shadeImage.gameObject.SetActive(false);
    }
}

