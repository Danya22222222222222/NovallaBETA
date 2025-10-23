using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoModeButton : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    public Color normalColor = Color.white;
    public Color activeColor = Color.green;

    void Update()
    {
        if (dialogueManager != null)
        {
            // «м≥нюЇмо кол≥р залежно в≥д стану
            if (dialogueManager.autoMode)
            {
                buttonImage.color = activeColor;
                buttonText.text = "AUTO ON";
            }
            else
            {
                buttonImage.color = normalColor;
                buttonText.text = "AUTO";
            }
        }
    }
}