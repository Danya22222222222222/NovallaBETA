using UnityEngine;
using UnityEngine.SceneManagement; // Для переходу в меню

public class SlideManager : MonoBehaviour
{
    public GameObject[] slides;

    private int currentSlide = 0;

    // Масиви слайдів для кожної кінцівки
    public int[] goodEndingSlides = new int[] { 35, 36 };
    public int[] badEndingSlides = new int[] { 37, 38, 39, 40, 41, 42, 43, 44 };
    public int[] neutralEndingSlides = new int[] { 45, 46 };

    void Start()
    {
        ShowSlide(0);
    }

    public void ShowSlide(int index)
    {
        if (slides == null || slides.Length == 0)
        {
            Debug.LogWarning("SlideManager: no slides assigned.");
            return;
        }

        if (index < 0 || index >= slides.Length)
        {
            Debug.LogWarning($"SlideManager: index out of range: {index}");
            return;
        }

        for (int i = 0; i < slides.Length; i++)
        {
            if (slides[i] != null)
                slides[i].SetActive(i == index);
        }

        currentSlide = index;
    }

    public void NextSlide()
    {
        if (slides == null || slides.Length == 0) return;
        int next = (currentSlide + 1) % slides.Length;
        ShowSlide(next);
    }

    public void PrevSlide()
    {
        if (slides == null || slides.Length == 0) return;
        int prev = (currentSlide - 1 + slides.Length) % slides.Length;
        ShowSlide(prev);
    }

    public void PlayEnding()
    {
        int score = ScoreManager.Instance.GetScore();

        if (score >= 2)
        {
            StartEnding(goodEndingSlides);
        }
        else if (score == 0)
        {
            StartEnding(badEndingSlides);
        }
        else
        {
            StartEnding(neutralEndingSlides);
        }
    }

    private void StartEnding(int[] endingSlides)
    {
        StartCoroutine(PlayEndingCoroutine(endingSlides));
    }

    private System.Collections.IEnumerator PlayEndingCoroutine(int[] endingSlides)
    {
        foreach (int slideIndex in endingSlides)
        {
            ShowSlide(slideIndex);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // Повернення в меню після останнього слайду
        SceneManager.LoadScene("Menu");
    }

    public void SetSlideOnBranch(int index)
    {
        Debug.Log($"SetSlideOnBranch({index}) викликано");
        ShowSlide(index);
    }
}
