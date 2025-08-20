using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject activeGate;
    public GameObject switchedGate;

    private bool isGateActive = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WhiteBall") || collision.gameObject.CompareTag("BlackBall"))
        {
            InvertGate();

            Collider plateCollider = GetComponent<Collider>();
            plateCollider.isTrigger = true;
        }
    }

    public void InvertGate()
    {
        isGateActive = !isGateActive;

        activeGate.SetActive(isGateActive);
        switchedGate.SetActive(!isGateActive);
    }
}
