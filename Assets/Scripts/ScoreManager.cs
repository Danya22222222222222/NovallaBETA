using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // "Instance" - �� �������� �����, ��� �������� ��������
    // ������ �� ����� ������� � ����-����� ������ �������
    // ����� ScoreManager.Instance
    public static ScoreManager Instance;

    public int score = 0;

    void Awake()
    {
        // �� ��������� ������ "��������"
        if (Instance == null)
        {
            // ���� �� ���� ScoreManager, ��� ��� ��������
            Instance = this;
            // �� ��������� ��� ��'��� ��� ������� �� �������
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���� ScoreManager ��� ����, ������� ��� �������
            Destroy(gameObject);
        }
    }

    // ����� ��� ��������� ������� (�� ���� ������������� � EndDialogue)
    public int GetScore()
    {
        return score;
    }

    // ����� ������ ����� ��� �������� �������, ���������, ��� ���� ���
    public void ResetScore()
    {
        score = 0;
    }
}