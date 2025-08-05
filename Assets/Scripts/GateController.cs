using UnityEngine;

public class GateController : MonoBehaviour
{
    public Material whiteMaterial;
    public Material blackMaterial;

    public bool isBlackGate = false; // Starts as white by default
    public CharacterMovement blackBall;
    public CharacterMovement whiteBall;

    private Renderer rend;
    public bool isMismatch = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateGateMaterial();
        blackBall = GameObject.FindGameObjectWithTag("BlackBall").GetComponent<CharacterMovement>();
        whiteBall = GameObject.FindGameObjectWithTag("WhiteBall").GetComponent<CharacterMovement>();
    }

    public void InvertGate()
    {
        isBlackGate = !isBlackGate;
        UpdateGateMaterial();
    }

    private void UpdateGateMaterial()
    {
        rend.material = isBlackGate ? blackMaterial : whiteMaterial;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WhiteBall") || collision.gameObject.CompareTag("BlackBall"))
        {
            BallColorState ballState = collision.gameObject.GetComponent<BallColorState>();

            if (ballState != null)
            {
                isMismatch = ballState.isBlack != isBlackGate;

                if (isMismatch)
                {
                    Debug.Log("❌ Blocked: Colors don't match!");

                    // Stop the ball
                    Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;

                        // Optional bounce
                        Vector3 bounceDir = (collision.transform.position - transform.position).normalized;
                        rb.AddForce(bounceDir * 3f, ForceMode.Impulse);
                    }

                    // Optional: Keep collider solid to block the ball
                }
                else
                {
                    Debug.Log("✔ Allowed: Colors match!");

                    // Let the ball through (optional)
                    BoxCollider gateCollider = GetComponent<BoxCollider>();
                    if (gateCollider != null)
                    {
                        gateCollider.isTrigger = true; // Make passable
                    }
                }
            }
        }
    }
}
