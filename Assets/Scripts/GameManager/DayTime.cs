using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    //Obtensmoa la luz de la escena
    public Light sceneLight;

    //Obtenemos la duracion del dia en segundos
    public float dayDuration = 300f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LightControl();
    }

    public void LightControl()
    {
        //Calculamos la intensidad de la luz

        //Esta funcion es la funcion de las ondas sin que va desde 0 a 1 siendo 1 como el punto maximo del dia
        float lighIntensity = 0.5f + Mathf.Sin(Time.time * 2.0f * Mathf.PI/dayDuration) / 2.0f;
        //Modificamos la luz de acuerdo a la intensidad
        GetComponent<Light>().intensity = lighIntensity; 
    }
}
