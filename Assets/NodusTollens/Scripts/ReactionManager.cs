using UnityEngine;
using UnityEngine.UI;

public class ReactionManager : MonoBehaviour
{                                        // <- house opens
    public Image reactionImage;          // fields live here
    public Sprite happySprite;
    public Sprite heartbrokenSprite;

    public void ShowHappy()              // methods live here too
    {
        reactionImage.sprite = happySprite;
        reactionImage.gameObject.SetActive(true);
    }

    public void ShowHeartbroken()
    {
        reactionImage.sprite = heartbrokenSprite;
        reactionImage.gameObject.SetActive(true);
    }
}                                        // <- house closes, LAST line, nothing after