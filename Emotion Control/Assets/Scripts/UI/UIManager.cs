using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject tooltipPanel;
    public GameObject showTooltipButton;

    public void ShowHideTooltip(bool show)
    {
        tooltipPanel.SetActive(show);
        showTooltipButton.SetActive(!show);
    }
}