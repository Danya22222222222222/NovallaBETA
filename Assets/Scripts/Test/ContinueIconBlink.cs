using UnityEngine;
using UnityEngine.UI;

public class ContinueIconBlink : MonoBehaviour
{
    public float blinkSpeed = 1f;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}