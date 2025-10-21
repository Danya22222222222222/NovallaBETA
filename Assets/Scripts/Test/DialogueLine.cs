using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 6)]
    public string dialogueText;

    public Sprite background;
    public Sprite characterLeft;
    public Sprite characterCenter;
    public Sprite characterRight;

    public bool hasChoices;
    public string[] choices;
    public int[] choiceJumpTo; // індекси для переходу
}