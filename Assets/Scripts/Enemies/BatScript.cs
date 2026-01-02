using UnityEngine;

public class CaveBat : MonoBehaviour
{
    public float detectRange = 4f;
    public float diveSpeed = 6f;
    public int damage = 1;

    private Vector3 startPos;
    private Transform player;
    private bool diving;

    void Start()
    {
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < detectRange)
            diving = true;

        if (diving)
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                diveSpeed * Time.deltaTime
            );
        else
            transform.position = startPos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            diving = false;
        }
    }
}
