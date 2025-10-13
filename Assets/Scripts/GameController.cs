using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    [Header("Компоненти UI")]
    public Image targetImage;
    public TextMeshProUGUI displayText;
    public Animator targetAnimator;

    [Header("Кнопки (опціонально)")]
    public Button nextButton;
    public Button prevButton;

    [Header("Контент для перемикання")]
    public Sprite[] sprites;
    public string[] texts;
    public string[] animationTriggers;

    [Header("Налаштування")]
    public int currentIndex = 0;

    void Start()
    {
        Debug.Log("ButtonController.Start — перевірка полів...");
        Debug.Log($" targetImage = {(targetImage == null ? "NULL" : "ok")} , displayText = {(displayText == null ? "NULL" : "ok")}, sprites.Length = {(sprites == null ? 0 : sprites.Length)}");

        // Підписка на кнопки як запасний варіант (ще раз перевірити, що кнопки працюють)
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
        if (sprites != null && sprites.Length > 0)
            currentIndex = (currentIndex + 1) % sprites.Length;
        else
            currentIndex++;
        UpdateContent();
    }

    public void OnPreviousButton()
    {
        Debug.Log("OnPreviousButton натиснута. currentIndex = " + currentIndex);
        if (sprites != null && sprites.Length > 0)
            currentIndex = (currentIndex - 1 + sprites.Length) % sprites.Length;
        else
            currentIndex = Mathf.Max(0, currentIndex - 1);
        UpdateContent();
    }

    private void UpdateContent()
    {
        Debug.Log("UpdateContent() викликано. currentIndex = " + currentIndex);

        // Картинка
        if (targetImage != null && sprites != null && sprites.Length > 0)
        {
            int idx = Mathf.Clamp(currentIndex, 0, sprites.Length - 1);
            targetImage.sprite = sprites[idx];
            Debug.Log(" Задано sprite index = " + idx + " (" + (sprites[idx] != null ? sprites[idx].name : "NULL SPRITE") + ")");
        }
        else
        {
            Debug.Log(" targetImage або sprites не готові (targetImage==null? " + (targetImage == null) + ", sprites==null? " + (sprites == null) + ")");
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
    }
}
