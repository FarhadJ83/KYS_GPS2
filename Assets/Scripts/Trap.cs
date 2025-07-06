using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WhiteBall") || collision.gameObject.CompareTag("BlackBall"))
        {
            TrapManager.Instance.RespawnBall(collision.gameObject);
        }
    }
}
