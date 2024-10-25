using UnityEngine;

public class FogController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            ActivateFog();
        if (Input.GetKeyDown(KeyCode.H))
            DeactivateFog();
    }

    public static void ActivateFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.gray;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = .15f;
    }

    public static void DeactivateFog()
    {
        RenderSettings.fog = false;
    }
}
