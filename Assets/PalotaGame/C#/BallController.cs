using UnityEngine;

public class BallController : MonoBehaviour
{
    public float initialForce = 5f;
    public float speedIncreaseFactor = 1.1f; // Factor por el cual la velocidad aumenta con cada colisión
    private Rigidbody2D rb;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        // Detecta el clic/tap en la pelota
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(mousePos))
            {
                if (!isMoving)
                {
                    StartMovement();
                }
                else
                {
                    StopMovement();
                }
            }
        }
    }

    void StartMovement()
    {
        isMoving = true;
        rb.isKinematic = false;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.velocity = randomDirection * initialForce;
    }

    void StopMovement()
    {
        isMoving = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        // Aquí podrías agregar código para mostrar un mensaje o reiniciar el juego.
        Debug.Log("Juego Terminado");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Aumenta la velocidad con cada colisión
        rb.velocity *= speedIncreaseFactor;

        // Cambia el color y la forma de la pelota
        ChangeColorAndShape();
    }

    void ChangeColorAndShape()
    {
        // Cambia el color aleatoriamente
        spriteRenderer.color = new Color(Random.value, Random.value, Random.value);

        // Cambia la forma aleatoriamente (por ejemplo, cambiar la escala)
        float randomScale = Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }
}
