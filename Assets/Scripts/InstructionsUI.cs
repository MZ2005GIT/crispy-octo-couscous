using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    public GameObject instructionsPanel;

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }
}
