using UnityEngine;

public class SlideManager : MonoBehaviour
{
    public GameObject[] slides;

    private int currentSlide = 0;

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
}
