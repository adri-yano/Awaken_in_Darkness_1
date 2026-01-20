using UnityEngine;

public class AlienFood : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        PlayerPetHandler p = col.GetComponent<PlayerPetHandler>();
        if (p == null) return;

        p.hasFood = true;          // player holds bone
        Destroy(gameObject);       // bone disappears
    }
}