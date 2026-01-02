using UnityEngine;

public class AlienFood : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerPetHandler p = col.GetComponent<PlayerPetHandler>();
        if (p == null) return;

        p.hasFood = true;
        Destroy(gameObject);
    }
}
