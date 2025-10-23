using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image backgroundImage;
    public Image characterLeft;
    public Image characterCenter;
    public Image characterRight;
    public GameObject dialogueBox;
    public GameObject continueIcon;
    public GameObject choicesPanel;
    public Button[] choiceButtons;

    [Header("Dialogue Data")]
    public DialogueLine[] dialogueLines;

    [Header("Settings")]
    public int mainDialogueEndIndex = 34;
    public float typingSpeed = 0.05f;
    public bool autoMode = false;
    public float autoDelay = 2f;

    [Header("Endings (indices inside dialogueLines array)")]
    public int[] goodEnding = new int[] { 35, 36 };
    public int[] badEnding = new int[] { 37, 38, 39, 40, 41, 42, 43, 44 };
    public int[] neutralEnding = new int[] { 45, 46 };

    private int currentLine = 0;
    private bool isTyping = false;
    private bool canContinue = true;
    private Coroutine typingCoroutine;

    // Ending state
    private bool isPlayingEnding = false;
    private int[] currentEnding = null;
    private int endingPos = 0;

    void Start()
    {
        choicesPanel.SetActive(false);
        DisplayLine(currentLine);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                CompleteText();
            }
            else if (canContinue)
            {
                NextLine();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ToggleAutoMode();
        }
    }

    void DisplayLine(int index)
    {
        if (index >= dialogueLines.Length)
        {
            Debug.LogError("Dialogue index out of range! Index: " + index);
            return;
        }

        DialogueLine line = dialogueLines[index];

        if (line.background != null)
            backgroundImage.sprite = line.background;

        SetCharacter(characterLeft, line.characterLeft);
        SetCharacter(characterCenter, line.characterCenter);
        SetCharacter(characterRight, line.characterRight);

        nameText.text = line.characterName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(line.dialogueText));

        if (line.hasChoices)
        {
            ShowChoices(line);
        }
        else
        {
            choicesPanel.SetActive(false);
        }
    }

    void SetCharacter(Image img, Sprite sprite)
    {
        if (sprite != null)
        {
            img.sprite = sprite;
            img.color = Color.white;
        }
        else
        {
            img.color = new Color(1, 1, 1, 0);
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        canContinue = false;
        dialogueText.text = "";
        continueIcon.SetActive(false);

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        CompleteText();
    }

    void CompleteText()
    {
        isTyping = false;
        dialogueText.text = dialogueLines[currentLine].dialogueText;

        if (!dialogueLines[currentLine].hasChoices)
        {
            continueIcon.SetActive(true);
            canContinue = true;

            if (autoMode)
            {
                StartCoroutine(AutoContinue());
            }
        }
    }

    IEnumerator AutoContinue()
    {
        yield return new WaitForSeconds(autoDelay);
        if (autoMode && canContinue)
            NextLine();
    }

    void NextLine()
    {
        // якщо граЇмо к≥нц≥вку
        if (isPlayingEnding && currentEnding != null)
        {
            endingPos++;
            if (endingPos >= currentEnding.Length)
            {
                SceneManager.LoadScene("Menu");
                return;
            }

            currentLine = currentEnding[endingPos];
            DisplayLine(currentLine);
            return;
        }

        // ѕерев≥рка на к≥нець основного д≥алогу ѕ≈–≈ƒ переходом
        if (currentLine >= mainDialogueEndIndex)
        {
            EndDialogue();
            return;
        }

        // ѕерев≥р€Їмо чи поточний слайд маЇ nextLine
        DialogueLine currentDialogueLine = dialogueLines[currentLine];

        // якщо Ї nextLine - переходимо туди
        if (currentDialogueLine.nextLine >= 0)
        {
            currentLine = currentDialogueLine.nextLine;
            DisplayLine(currentLine);
            return;
        }

        // «вичайний л≥н≥йний перех≥д (т≥льки €кщо немаЇ nextLine)
        currentLine++;
        DisplayLine(currentLine);
    }

    void ShowChoices(DialogueLine line)
    {
        canContinue = false;
        continueIcon.SetActive(false);
        choicesPanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < line.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = line.choices[i];

                int choiceIndex = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnChoiceSelected(int choiceIndex)
    {
        DialogueLine line = dialogueLines[currentLine];

        // ƒодаЇмо бали за виб≥р
        if (ScoreManager.Instance != null && line.choiceScores != null && choiceIndex < line.choiceScores.Length)
        {
            int scoreChange = line.choiceScores[choiceIndex];
            ScoreManager.Instance.score += scoreChange;
            Debug.Log($"Choice made at line {currentLine}. Choice index: {choiceIndex}, Score change: {scoreChange}, Total score: {ScoreManager.Instance.score}");
        }

        choicesPanel.SetActive(false);

        // ѕереходимо на слайд куди веде цей виб≥р
        currentLine = line.choiceJumpTo[choiceIndex];
        DisplayLine(currentLine);
    }

    public void ToggleAutoMode()
    {
        autoMode = !autoMode;
        Debug.Log("Auto Mode: " + (autoMode ? "ON" : "OFF"));
    }

    void EndDialogue()
    {
        Debug.Log("End of main dialogue reached");

        if (ScoreManager.Instance != null)
        {
            int score = ScoreManager.Instance.GetScore();
            Debug.Log("Final score is: " + score);

            if (score >= 2)
            {
                currentEnding = goodEnding;
                Debug.Log("Playing Good Ending");
            }
            else if (score <= -1)
            {
                currentEnding = badEnding;
                Debug.Log("Playing Bad Ending");
            }
            else
            {
                currentEnding = neutralEnding;
                Debug.Log("Playing Neutral Ending");
            }

            if (currentEnding != null && currentEnding.Length > 0)
            {
                isPlayingEnding = true;
                endingPos = 0;
                currentLine = currentEnding[endingPos];
                DisplayLine(currentLine);
                return;
            }
        }

        dialogueBox.SetActive(false);
        Debug.Log("Dialogue ended (no ending played)");
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("CurrentLine", currentLine);
        PlayerPrefs.Save();
        Debug.Log("Game saved");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("CurrentLine"))
        {
            currentLine = PlayerPrefs.GetInt("CurrentLine");
            DisplayLine(currentLine);
            Debug.Log("Game loaded");
        }
    }
}