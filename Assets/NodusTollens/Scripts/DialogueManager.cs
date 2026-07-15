using UnityEngine;

using TMPro;

// One flashcard: who is talking and what they say

[System.Serializable]

public class DialogueLine

{

    public string speakerName;

    [TextArea(2, 5)] public string sentence;

}

public class DialogueManager : MonoBehaviour

{

    [Header("UI References")]

    public TMP_Text nameText;

    public TMP_Text dialogueText;

    public GameObject choicePanel;   // stays empty until Part 4

    [Header("The Flashcard Decks")]

    public DialogueLine[] commonLines;   // everyone sees these

    public DialogueLine[] branchALines;  // shown if player picks choice A

    public DialogueLine[] branchBLines;  // shown if player picks choice B

    private DialogueLine[] currentDeck;

    private int index = 0;

    private int branch = 0; // 0 = common, 1 = A, 2 = B

    void Start()

    {

        // Load save if one exists, otherwise start fresh

        branch = PlayerPrefs.GetInt("SavedBranch", 0);

        index = PlayerPrefs.GetInt("SavedIndex", 0);

        currentDeck = DeckForBranch(branch);

        if (choicePanel != null) choicePanel.SetActive(false);

        ShowLine();

    }

    DialogueLine[] DeckForBranch(int b)

    {

        if (b == 1) return branchALines;

        if (b == 2) return branchBLines;

        return commonLines;

    }

    void ShowLine()

    {

        DialogueLine line = currentDeck[index];

        nameText.text = line.speakerName;

        dialogueText.text = line.sentence;

        SaveProgress();

    }

    // Hooked to the invisible full-screen button

    public void OnAdvanceClicked()

    {

        // If choices are on screen, ignore background clicks

        if (choicePanel != null && choicePanel.activeSelf) return;

        index++;

        // Ran out of cards in the common deck? Time to choose.

        if (branch == 0 && index >= currentDeck.Length)

        {

            if (choicePanel != null) choicePanel.SetActive(true);

            return;

        }

        // Ran out of cards in a branch? That is an ending.

        if (index >= currentDeck.Length)

        {

            nameText.text = "";

            dialogueText.text = "THE END (Branch " + (branch == 1 ? "A" : "B") + ")";

            return;

        }

        ShowLine();

    }

    // Hooked to the choice buttons in Part 4

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

