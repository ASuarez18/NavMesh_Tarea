using UnityEngine;

public class RainController : MonoBehaviour
{
    // Referencia al sistema de part�culas de la lluvia
    public ParticleSystem rainParticles;
    
    // Intensidad de la lluvia (emission rate)
    public float rainIntensity = 1000f;

    // Booleano para controlar si la lluvia est� activa
    private bool isRaining = true;

    void Update()
    {
        // Si pulsas la tecla "R", alterna la lluvia entre encendida y apagada
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRain();
        }
    }

    // M�todo para encender o apagar la lluvia
    public void ToggleRain()
    {
        isRaining = !isRaining;

        if (isRaining)
        {
            rainParticles.Play(); // Activa la lluvia
        }
        else
        {
            rainParticles.Stop(); // Detiene la lluvia
        }
    }
}
