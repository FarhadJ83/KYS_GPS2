using UnityEngine;

public class GateController : MonoBehaviour
{
    public Material whiteMaterial;
    public Material blackMaterial;

    public bool isBlackGate = false; // Starts as white by default

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateGateMaterial();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhiteBall") || other.CompareTag("BlackBall"))
        {
            BallColorState ballState = other.GetComponent<BallColorState>();

            if (ballState != null)
            {
                bool isMismatch = ballState.isBlack != isBlackGate;

                if (isMismatch)
                {
                    Debug.Log("❌ Blocked: Colors don't match!");

                    // Option A: Stop the ball
                    Vector3 rb = Vector3.zero;
                    if (rb != null)
                    {
                        rb = Vector3.zero;

                        // Optional bounce back
                        //Vector3 bounceBack = (other.transform.position - transform.position).normalized;
                        //rb.AddForce(bounceBack * 3f, ForceMode.Impulse);
                    }

                    // Option B: Enable physical blocking
                    Collider gateCollider = GetComponent<Collider>();
                    gateCollider.enabled = true;
                }
                else
                {
                    Debug.Log("✔ Allowed: Colors match!");
                }
            }
        }
    }
}
