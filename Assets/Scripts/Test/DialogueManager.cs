using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public float typingSpeed = 0.05f;
    public bool autoMode = false;
    public float autoDelay = 2f;

    private int currentLine = 0;
    private bool isTyping = false;
    private bool canContinue = true;
    private Coroutine typingCoroutine;

    void Start()
    {
        choicesPanel.SetActive(false);
        DisplayLine(currentLine);
    }

    void Update()
    {
        // Продовження діалогу натисканням миші або Space
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Пропуск анімації друку
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                CompleteText();
            }
            else if (canContinue)
            {
                NextLine();
            }
        }

        // Авторежим
        if (Input.GetKeyDown(KeyCode.A))
        {
            ToggleAutoMode();
        }
    }

    void DisplayLine(int index)
    {
        if (index >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueLines[index];

        // Встановлення фону
        if (line.background != null)
            backgroundImage.sprite = line.background;

        // Встановлення персонажів
        SetCharacter(characterLeft, line.characterLeft);
        SetCharacter(characterCenter, line.characterCenter);
        SetCharacter(characterRight, line.characterRight);

        // Встановлення імені
        nameText.text = line.characterName;

        // Запуск друкарського ефекту
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(line.dialogueText));

        // Вибори
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
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(line.choiceJumpTo[choiceIndex]));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnChoiceSelected(int jumpToIndex)
    {
        choicesPanel.SetActive(false);
        currentLine = jumpToIndex;
        DisplayLine(currentLine);
    }

    void ToggleAutoMode()
    {
        autoMode = !autoMode;
        Debug.Log("Auto Mode: " + (autoMode ? "ON" : "OFF"));
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        Debug.Log("Діалог завершено");
        // Тут можна завантажити наступну сцену або показати меню
    }

    // Публічні методи для кнопок
    public void SaveGame()
    {
        PlayerPrefs.SetInt("CurrentLine", currentLine);
        PlayerPrefs.Save();
        Debug.Log("Гру збережено");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("CurrentLine"))
        {
            currentLine = PlayerPrefs.GetInt("CurrentLine");
            DisplayLine(currentLine);
            Debug.Log("Гру завантажено");
        }
    }
}