using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // "Instance" - це статична змінна, яка дозволяє отримати
    // доступ до цього скрипта з будь-якого іншого скрипта
    // через ScoreManager.Instance
    public static ScoreManager Instance;

    public int score = 0;

    void Awake()
    {
        // Це класичний патерн "синглтон"
        if (Instance == null)
        {
            // Якщо ще немає ScoreManager, цей стає головним
            Instance = this;
            // Не знищувати цей об'єкт при переході між сценами
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Якщо ScoreManager ВЖЕ існує, знищити цей дублікат
            Destroy(gameObject);
        }
    }

    // Метод для отримання рахунку (ви його використовуєте в EndDialogue)
    public int GetScore()
    {
        return score;
    }

    // Можна додати метод для скидання рахунку, наприклад, для нової гри
    public void ResetScore()
    {
        score = 0;
    }
}