using UnityEngine;

public class XPSystem : MonoBehaviour
{
    public int maxXP = 100;
    public int currentXP = 0;

    public void AddXP(int amount)
    {
        currentXP += amount;
        currentXP = Mathf.Clamp(currentXP, 0, maxXP);
    }
}
