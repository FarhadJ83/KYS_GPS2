using UnityEngine;

public class BallColorState : MonoBehaviour
{
    public BallType ballType;
    public bool isBlack = false;

    public Material whiteMaterial;
    public Material blackMaterial;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateBallMaterial();
    }

    public void InvertColor()
    {
        isBlack = !isBlack;
        UpdateBallMaterial();
    }

    void UpdateBallMaterial()
    {
        rend.material = isBlack ? blackMaterial : whiteMaterial;
    }
}
