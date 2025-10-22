using UnityEngine;

// [System.Serializable] �������� ��� ������ �� ���������� 
// ��� ���� � ��������� Unity, ���� �� � �������� ������ �������.
[System.Serializable]
public class DialogueLine
{
    [Header("Text")]
    public string characterName;
    [TextArea(3, 10)] // ������ ���� ������ ������ � ���������
    public string dialogueText;

    [Header("Visuals")]
    public Sprite background;
    public Sprite characterLeft;
    public Sprite characterCenter;
    public Sprite characterRight;

    [Header("Choices")]
    public bool hasChoices;

    // ֳ ������ ����������������, Ҳ���� ���� hasChoices = true
    public string[] choices;          // ����� ��� ������
    public int[] choiceJumpTo;      // �� ���� ������ ������ �������
    public int[] choiceScores;       // ������ ���� ���� �� ��� ����
}