using UnityEngine;

public class AlienDog : MonoBehaviour
{
    public float followSpeed = 3f;
    public float attackSpeed = 2f;
    public int damage = 1;
    public float detectRange = 5f;

    private Transform player;
    private bool isPet;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (isPet)
        {
            FollowPlayer();
        }
        else if (dist < detectRange)
        {
            ChasePlayer();
        }
    }

    void FollowPlayer()
    {
        Vector2 target = player.position;
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            followSpeed * Time.deltaTime
        );
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            attackSpeed * Time.deltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        PlayerPetHandler pet = col.GetComponent<PlayerPetHandler>();

        // Feed the dog
        if (!isPet && pet != null && pet.hasFood)
        {
            isPet = true;
            pet.hasFood = false;
            gameObject.layer = LayerMask.NameToLayer("Player");
            Debug.Log("Dog became pet!");
            return;
        }

        // Damage if hostile
        if (!isPet)
            col.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }
}