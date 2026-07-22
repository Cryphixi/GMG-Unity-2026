using UnityEngine;

using UnityEngine.UI;

using UnityEngine.SceneManagement;

using TMPro;

public enum EffectBurstType { None, Glitch, Water }

[System.Serializable]

public class DialogueLine

{

    public string speakerName;

    [TextArea(2, 5)] public string sentence;

    [Header("Art (empty = keep current)")]

    public Sprite characterSprite;

    public bool hideCharacter;

    public Sprite background;

    [Header("Horror effects")]

    public EffectBurstType playEffect;  // one-shot burst on this line

    public Sprite shadeOverlay;         // mood tint that STAYS

    public bool clearShade;             // remove the tint on this line

}

public class DialogueManager : MonoBehaviour

{

    [Header("UI References")]

    public TMP_Text nameText;

    public TMP_Text dialogueText;

    public GameObject choicePanel;

    public Image characterImage;

    public Image backgroundImage;

    public EffectsManager effects;      // NEW: drag GameManager itself here

    [Header("The Flashcard Decks")]

    public DialogueLine[] commonLines;

    public DialogueLine[] branchALines;

    public DialogueLine[] branchBLines;

    [Header("Ending screen text")]

    public string endingATitle = "ENDING A";

    public string endingBTitle = "ENDING B";

    private DialogueLine[] currentDeck;

    private int index = 0;

    private int branch = 0;

    private bool atEnding = false;

    void Start()

    {

        branch = PlayerPrefs.GetInt("SavedBranch", 0);

        index = PlayerPrefs.GetInt("SavedIndex", 0);

        currentDeck = DeckForBranch(branch);

        if (index >= currentDeck.Length) index = 0; // safety if decks shrank

        if (choicePanel != null) choicePanel.SetActive(false);

        // SAVE RESTORE: silently replay art/shade from earlier lines

        // so Continue shows the right background, character, and tint

        for (int i = 0; i < index; i++)

            ApplyVisuals(currentDeck[i], false);

        ShowLine();

    }

    DialogueLine[] DeckForBranch(int b)

    {

        if (b == 1) return branchALines;

        if (b == 2) return branchBLines;

        return commonLines;

    }

    void ApplyVisuals(DialogueLine line, bool includeBurst)

    {

        if (line.background != null && backgroundImage != null)

            backgroundImage.sprite = line.background;

        if (characterImage != null)

        {

            if (line.hideCharacter)

                characterImage.gameObject.SetActive(false);

            else if (line.characterSprite != null)

            {

                characterImage.sprite = line.characterSprite;

                characterImage.gameObject.SetActive(true);

            }

        }

        if (effects != null)

        {

            if (line.clearShade) effects.ClearShade();

            else if (line.shadeOverlay != null) effects.ShowShade(line.shadeOverlay);

            if (includeBurst)

            {

                if (line.playEffect == EffectBurstType.Glitch) effects.PlayGlitch();

                else if (line.playEffect == EffectBurstType.Water) effects.PlayWater();

            }

        }

    }

    void ShowLine()

    {

        DialogueLine line = currentDeck[index];

        nameText.text = line.speakerName;

        dialogueText.text = line.sentence;

        ApplyVisuals(line, true);

        SaveProgress();

    }

    public void OnAdvanceClicked()

    {

        if (choicePanel != null && choicePanel.activeSelf) return;

        // Clicking on the ending screen: wipe save, back to menu

        if (atEnding)

        {

            PlayerPrefs.DeleteKey("SavedIndex");

            PlayerPrefs.DeleteKey("SavedBranch");

            SceneManager.LoadScene("MainMenu");

            return;

        }

        index++;

        if (branch == 0 && index >= currentDeck.Length)

        {

            if (choicePanel != null) choicePanel.SetActive(true);

            return;

        }

        if (index >= currentDeck.Length)

        {

            atEnding = true;

            nameText.text = "";

            dialogueText.text = (branch == 1 ? endingATitle : endingBTitle)

                + "\n\n(click to return to the menu)";

            return;

        }

        ShowLine();

    }

    public void ChooseBranch(int chosenBranch)

    {

        branch = chosenBranch;

        currentDeck = DeckForBranch(branch);

        index = 0;

        choicePanel.SetActive(false);

        ShowLine();

    }

    void SaveProgress()

    {

        PlayerPrefs.SetInt("SavedBranch", branch);

        PlayerPrefs.SetInt("SavedIndex", index);

        PlayerPrefs.Save();

    }

}

