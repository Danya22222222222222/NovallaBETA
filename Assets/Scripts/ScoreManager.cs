using UnityEngine;

// ScoreManager зберігає очки виборів та доступний з будь-якого скрипта як синглтон.
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // Поточні очки
    public int score = 0;

    // Налаштовувані масиви індексів хороших і поганих слайдів (можна змінити в інспекторі)
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

    // Викликайте цей метод коли гравець обирає слайд (передайте індекс слайду)
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

    // Додає значення до рахунку
    private void AddScore(int value)
    {
        score += value;
    }

    // Перевірка наявності значення в масиві (щоб уникати використання Linq)
    private bool IsInArray(int[] array, int value)
    {
        if (array == null) return false;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == value) return true;
        }
        return false;
    }

    // Повертає поточний рахунок
    public int GetScore()
    {
        return score;
    }

    // Скидає рахунок (наприклад при рестарті новели)
    public void ResetScore()
    {
        score = 0;
    }

    // Простий метод для визначення поганого фіналу (налаштовується порогом)
    public bool IsBadEnding(int threshold = 0)
    {
        return score <= threshold;
    }
}
