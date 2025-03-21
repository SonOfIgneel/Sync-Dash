using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxSpeed = 25f;
    public float speedIncreaseRate = 0.1f;
    public float jumpForce = 12f;
    public GameManager manager;
    private Rigidbody rb;
    public bool isGrounded, collided = false;
    public ParticleSystem particles;
    public GameObject collectedOrb;

    #region Setting up rigid body properties, moving the player with gradual increase in speed along with jump functionality
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -50f, 0);

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        moveSpeed = Mathf.Min(moveSpeed + (speedIncreaseRate * Time.deltaTime), maxSpeed);

        if (manager.isGameRunning)
        {
            manager.score += (speedIncreaseRate * Time.deltaTime);
            manager.scoreText.text = manager.score.ToString("F2");
        }

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveSpeed);

        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, moveSpeed);
            isGrounded = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.down * 0.2f;
        }
    }
    #endregion

    #region Handle interactable objects
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("obj: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            collided = true;
            GameManager.instance.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == collectedOrb)
            return;
        if (other.gameObject.CompareTag("Orbs"))
        {
            particles.Play();
            manager.score += 5;
            manager.collectSound.Play();
            Debug.Log("Orbs collected");
            GameObject.Find("Orbs Spawner").GetComponent<OrbManager>().CollectOrb(other.gameObject);
        }
    }
    #endregion
}
