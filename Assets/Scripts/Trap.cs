using UnityEngine;

public class Trap : MonoBehaviour
{
    public AudioClip trapSound;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WhiteBall") || collision.gameObject.CompareTag("BlackBall"))
        {
            TrapManager.Instance.RespawnBall(collision.gameObject);
            GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(trapSound);
        }
    }
}
