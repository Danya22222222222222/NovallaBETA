using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    [Header("���������� UI")]
    public Image targetImage;
    public TextMeshProUGUI displayText;
    public Animator targetAnimator;

    [Header("������ (�����������)")]
    public Button nextButton;
    public Button prevButton;

    [Header("������� ��� �����������")]
    public Sprite[] sprites;
    public string[] texts;
    public string[] animationTriggers;

    [Header("������������")]
    public int currentIndex = 0;

    void Start()
    {
        Debug.Log("ButtonController.Start � �������� ����...");
        Debug.Log($" targetImage = {(targetImage == null ? "NULL" : "ok")} , displayText = {(displayText == null ? "NULL" : "ok")}, sprites.Length = {(sprites == null ? 0 : sprites.Length)}");

        // ϳ������ �� ������ �� �������� ������ (�� ��� ���������, �� ������ ��������)
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButton);
            Debug.Log("ϳ������� OnNextButton �� nextButton");
        }
        if (prevButton != null)
        {
            prevButton.onClick.AddListener(OnPreviousButton);
            Debug.Log("ϳ������� OnPreviousButton �� prevButton");
        }

        UpdateContent();
    }

    public void OnNextButton()
    {
        Debug.Log("OnNextButton ���������. currentIndex = " + currentIndex);
        if (sprites != null && sprites.Length > 0)
            currentIndex = (currentIndex + 1) % sprites.Length;
        else
            currentIndex++;
        UpdateContent();
    }

    public void OnPreviousButton()
    {
        Debug.Log("OnPreviousButton ���������. currentIndex = " + currentIndex);
        if (sprites != null && sprites.Length > 0)
            currentIndex = (currentIndex - 1 + sprites.Length) % sprites.Length;
        else
            currentIndex = Mathf.Max(0, currentIndex - 1);
        UpdateContent();
    }

    private void UpdateContent()
    {
        Debug.Log("UpdateContent() ���������. currentIndex = " + currentIndex);

        // ��������
        if (targetImage != null && sprites != null && sprites.Length > 0)
        {
            int idx = Mathf.Clamp(currentIndex, 0, sprites.Length - 1);
            targetImage.sprite = sprites[idx];
            Debug.Log(" ������ sprite index = " + idx + " (" + (sprites[idx] != null ? sprites[idx].name : "NULL SPRITE") + ")");
        }
        else
        {
            Debug.Log(" targetImage ��� sprites �� ����� (targetImage==null? " + (targetImage == null) + ", sprites==null? " + (sprites == null) + ")");
        }

        // �����
        if (displayText != null && texts != null && texts.Length > 0)
        {
            int idx = Mathf.Clamp(currentIndex, 0, texts.Length - 1);
            displayText.text = texts[idx];
            Debug.Log(" ������ text index = " + idx + " -> " + texts[idx]);
        }
        else if (displayText != null)
        {
            displayText.text = "no texts";
            Debug.Log(" texts == null ��� ������");
        }

        // ������� (�����������)
        if (targetAnimator != null && animationTriggers != null && animationTriggers.Length > 0)
        {
            int idx = Mathf.Clamp(currentIndex, 0, animationTriggers.Length - 1);
            string trigger = animationTriggers[idx];
            targetAnimator.ResetTrigger(trigger);
            targetAnimator.SetTrigger(trigger);
            Debug.Log(" ��������� ������: " + trigger);
        }
    }
}
