using UnityEngine;

public class MovingUnit : MonoBehaviour
{
    public float m_Speed = 100;

    private Rigidbody2D rb;

    private Vector2 destination;
    private Vector2 place;
    private Vector2 journey;
    private Vector2 gap;

    [HideInInspector]
    public bool Status;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        place = rb.transform.position;
    }

    public void PrepareToMove(Vector2 dest)
    {
        destination = dest;
        journey.x = destination.x - place.x;
        journey.y = destination.y - place.y;
        // скорость т.
        rb.velocity = new Vector2(journey.normalized.x * m_Speed * Time.deltaTime, journey.normalized.y * m_Speed * Time.deltaTime);
        Status = true;
    }

    void Update()
    {
        if (Status)
        {
            place = rb.transform.position;
            gap.x = Mathf.Abs(destination.x) - Mathf.Abs(place.x);
            gap.y = Mathf.Abs(destination.y) - Mathf.Abs(place.y);

            if (0.5f > Mathf.Abs(gap.x) && 0.5f > Mathf.Abs(gap.y))
            {
                rb.velocity = new Vector2(0.0f, 0.0f);
                Status = false;
                GetComponent<Unit>().p_back = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            Status = false;
        }
        if (collision.collider.CompareTag("Building"))
        {
            Status = false;
        }
        if (collision.collider.CompareTag("boss"))
        {
            Status = false;
        }
    }
}
   
