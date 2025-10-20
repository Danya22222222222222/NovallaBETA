using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    [Header("Компоненти UI")]
    public Image backgroundImage;
    public Image characterImage;
    public TextMeshProUGUI displayText;
    public Animator targetAnimator;

    [Header("Кнопки (опціонально)")]
    public Button nextButton;
    public Button prevButton;

    [Header("Контент для перемикання")]
    public Sprite[] backgroundSprites;
    public Sprite[] characterSprites;
    public string[] texts;
    public string[] animationTriggers;

    [Header("Групи кнопок вибору слайду")]
    public GameObject slideSelectButtonsGroup;
    public int slideSelectButtonsGroupIndex;

    public GameObject slideSelectButtonsGroup2;
    public int slideSelectButtonsGroup2Index;

    public GameObject slideSelectButtonsGroup3;
    public int slideSelectButtonsGroup3Index;

    public GameObject slideSelectButtonsGroup4;
    public int slideSelectButtonsGroup4Index;

    [Header("Налаштування")]
    public int currentIndex = 0;

    [Header("Індивідуальні переходи для слайдів (опціонально)")]
    // Якщо значення -1, то перехід стандартний (currentIndex+1). Якщо >=0, то перехід на вказаний слайд.
    public int[] nextSlideOverrides;

    void Start()
    {
        currentIndex = 0; // завжди стартуємо з першого слайду
        Debug.Log("ButtonController.Start — перевірка полів...");
        Debug.Log($" backgroundImage = {(backgroundImage == null ? "NULL" : "ok")} , characterImage = {(characterImage == null ? "NULL" : "ok")}, displayText = {(displayText == null ? "NULL" : "ok")}");
        Debug.Log($" backgroundSprites.Length = {(backgroundSprites == null ? 0 : backgroundSprites.Length)}, characterSprites.Length = {(characterSprites == null ? 0 : characterSprites.Length)}, texts.Length = {(texts == null ? 0 : texts.Length)}");

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButton);
            Debug.Log("Підписано OnNextButton на nextButton");
        }
        if (prevButton != null)
        {
            prevButton.onClick.AddListener(OnPreviousButton);
            Debug.Log("Підписано OnPreviousButton на prevButton");
        }

        UpdateContent();
    }

    public void OnNextButton()
    {
        Debug.Log("OnNextButton натиснута. currentIndex = " + currentIndex);
        int nextIndex = currentIndex + 1;
        if (nextSlideOverrides != null && currentIndex < nextSlideOverrides.Length && nextSlideOverrides[currentIndex] >= 0)
        {
            nextIndex = nextSlideOverrides[currentIndex];
            Debug.Log($"OnNextButton: Override! Переходимо на слайд {nextIndex}");
        }
        currentIndex = nextIndex;
        UpdateContent();
    }

    public void OnPreviousButton()
    {
        Debug.Log("OnPreviousButton натиснута. currentIndex = " + currentIndex);
        currentIndex = Mathf.Max(0, currentIndex - 1);
        UpdateContent();
    }

    public void ShowSlide(int index)
    {
        Debug.Log($"ShowSlide({index}) викликано");
        if (backgroundSprites == null || backgroundSprites.Length == 0) return;
        if (index < 0 || index >= backgroundSprites.Length) return;
        currentIndex = index;
        UpdateContent();
    }

    private void UpdateContent()
    {
        Debug.Log("UpdateContent() викликано. currentIndex = " + currentIndex);

        // Фонове зображення
        if (backgroundImage != null && backgroundSprites != null && backgroundSprites.Length > 0)
        {
            int idx = currentIndex % backgroundSprites.Length;
            backgroundImage.sprite = backgroundSprites[idx];
            Debug.Log(" Задано background sprite index = " + idx + " (" + (backgroundSprites[idx] != null ? backgroundSprites[idx].name : "NULL SPRITE") + ")");
        }
        else
        {
            Debug.Log(" backgroundImage або backgroundSprites не готові");
        }

        // Зображення персонажа
        if (characterImage != null && characterSprites != null && characterSprites.Length > 0)
        {
            int idx = currentIndex % characterSprites.Length;
            characterImage.sprite = characterSprites[idx];
            Debug.Log(" Задано character sprite index = " + idx + " (" + (characterSprites[idx] != null ? characterSprites[idx].name : "NULL SPRITE") + ")");
        }
        else
        {
            Debug.Log(" characterImage або characterSprites не готові");
        }

        // Текст
        if (displayText != null && texts != null && texts.Length > 0)
        {
            int idx = Mathf.Clamp(currentIndex, 0, texts.Length - 1);
            displayText.text = texts[idx];
            Debug.Log(" Задано text index = " + idx + " -> " + texts[idx]);
        }
        else if (displayText != null)
        {
            displayText.text = "no texts";
            Debug.Log(" texts == null або порожні");
        }

        // Анімація (опціонально)
        if (targetAnimator != null && animationTriggers != null && animationTriggers.Length > 0)
        {
            int idx = Mathf.Clamp(currentIndex, 0, animationTriggers.Length - 1);
            string trigger = animationTriggers[idx];
            targetAnimator.ResetTrigger(trigger);
            targetAnimator.SetTrigger(trigger);
            Debug.Log(" Викликано тригер: " + trigger);
        }

        // Показ/приховування груп кнопок вибору слайду
        if (slideSelectButtonsGroup != null)
        {
            slideSelectButtonsGroup.SetActive(currentIndex == slideSelectButtonsGroupIndex);
        }
        if (slideSelectButtonsGroup2 != null)
        {
            slideSelectButtonsGroup2.SetActive(currentIndex == slideSelectButtonsGroup2Index);
        }
        if (slideSelectButtonsGroup3 != null)
        {
            slideSelectButtonsGroup3.SetActive(currentIndex == slideSelectButtonsGroup3Index);
        }
        if (slideSelectButtonsGroup4 != null)
        {
            slideSelectButtonsGroup4.SetActive(currentIndex == slideSelectButtonsGroup4Index);
        }
        if (slideSelectButtonsGroup2 != null)
        {
            slideSelectButtonsGroup2.SetActive(currentIndex >= 15);
        }
        if (slideSelectButtonsGroup3 != null)
        {
            slideSelectButtonsGroup3.SetActive(currentIndex >= 22);
        }
        if (slideSelectButtonsGroup4 != null)
        {
            slideSelectButtonsGroup4.SetActive(currentIndex >= 30);
        }
    }
}