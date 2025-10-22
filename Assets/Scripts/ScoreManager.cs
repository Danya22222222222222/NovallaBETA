using UnityEngine;

// ScoreManager ������ ���� ������ �� ��������� � ����-����� ������� �� ��������.
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // ������ ����
    public int score = 0;

    // ������������ ������ ������� ������� � ������� ������ (����� ������ � ���������)
    public int[] goodSlides = new int[] { 19, 28 };
    public int[] badSlides = new int[] { 21, 30 };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���������� ��� ����� ���� ������� ����� ����� (��������� ������ ������)
    public void RegisterSlideChoice(int slideIndex)
    {
        if (IsInArray(badSlides, slideIndex))
        {
            AddScore(-1);
            Debug.Log($"Bad choice selected (slide {slideIndex}). Score: {score}");
        }
        else if (IsInArray(goodSlides, slideIndex))
        {
            AddScore(1);
            Debug.Log($"Good choice selected (slide {slideIndex}). Score: {score}");
        }
        else
        {
            Debug.Log($"Neutral choice (slide {slideIndex}). Score: {score}");
        }
    }

    // ���� �������� �� �������
    private void AddScore(int value)
    {
        score += value;
    }

    // �������� �������� �������� � ����� (��� ������� ������������ Linq)
    private bool IsInArray(int[] array, int value)
    {
        if (array == null) return false;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == value) return true;
        }
        return false;
    }

    // ������� �������� �������
    public int GetScore()
    {
        return score;
    }

    // ����� ������� (��������� ��� ������� ������)
    public void ResetScore()
    {
        score = 0;
    }

    // ������� ����� ��� ���������� �������� ������ (������������� �������)
    public bool IsBadEnding(int threshold = 0)
    {
        return score <= threshold;
    }
}
