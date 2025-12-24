using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    public XPSystem xp;
    public Slider slider;

    void Start()
    {
        slider.maxValue = xp.maxXP;
    }

    void Update()
    {
        slider.value = xp.currentXP;
    }
}