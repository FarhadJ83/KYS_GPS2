using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject whiteGate;
    public GameObject blackGate;

    private bool isWhiteActive = true;

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
        isWhiteActive = !isWhiteActive;

        whiteGate.SetActive(isWhiteActive);
        blackGate.SetActive(!isWhiteActive);
    }
}
