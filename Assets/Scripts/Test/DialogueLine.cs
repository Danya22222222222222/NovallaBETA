using UnityEngine;

// [System.Serializable] дозволяє нам бачити та редагувати 
// цей клас в інспекторі Unity, коли він є частиною іншого скрипта.
[System.Serializable]
public class DialogueLine
{
    [Header("Text")]
    public string characterName;
    [TextArea(3, 10)] // Робить поле тексту більшим в інспекторі
    public string dialogueText;

    [Header("Visuals")]
    public Sprite background;
    public Sprite characterLeft;
    public Sprite characterCenter;
    public Sprite characterRight;

    [Header("Choices")]
    public bool hasChoices;

    // Ці масиви використовуються, ТІЛЬКИ ЯКЩО hasChoices = true
    public string[] choices;          // Текст для кнопок
    public int[] choiceJumpTo;      // На який ІНДЕКС репліки перейти
    public int[] choiceScores;       // Скільки очок дати за цей вибір
}