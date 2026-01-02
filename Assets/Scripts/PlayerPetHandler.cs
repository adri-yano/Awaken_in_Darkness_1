using UnityEngine;
using UnityEngine.UI;

public class PlayerPetHandler : MonoBehaviour
{
    public bool hasFood;
    public Image boneIcon;

    void Update()
    {
        if (boneIcon != null)
            boneIcon.enabled = hasFood;
    }
}