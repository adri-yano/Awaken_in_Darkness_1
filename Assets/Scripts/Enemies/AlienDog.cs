using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AlienDog : MonoBehaviour
{
    public float followSpeed = 6f;
    public float followDistance = 1.2f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isPet;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isPet)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isMoving", false);
            return;
        }

        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (distance > followDistance)
        {
            float dir = Mathf.Sign(player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(dir * followSpeed, 0);
            anim.SetBool("isMoving", true);
            Flip(dir);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }

    void Flip(float dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        PlayerPetHandler pet = col.GetComponent<PlayerPetHandler>();
        if (pet != null && pet.hasFood)
        {
            isPet = true;
            pet.hasFood = false;
        }
    }
}