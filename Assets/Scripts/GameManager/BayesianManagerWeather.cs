using UnityEngine;
using System.Collections.Generic;

public class BayesianManagerWeather : MonoBehaviour
{
    //Primero debemos de obtener nuestros tres entradas o nodos que modificaran el clima
    public float _sceneLight;
    [SerializeField] private GameObject _playerInventory;
    public bool _enemyAttacking;

    //Variables de probabilidad
    bool x1,x2,x3;

    //Manager de estados
    string _switch;

    //Luego creamos una lista para almacenar los valores de los nodos anteriores
    List<float> probabilidades;

    //Lista de estados activos
    public List<string> estados;

    //Variables de entorno
    [SerializeField] public Light _sceneLightColor;
    [SerializeField] public ParticleSystem _rain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Creamos una funcion que reciba un umbral
    public bool Acceptance(float value)
    {
        return Random.Range(0f, 1f) > value;
    }


    public void BayesianWeather()
    {
        EnemyAttack();
        DayTime();
        Item();
        LunaSangre();
        Neblina();
        Lluvia();
        SelectorEstado();
    }

    
    public void EnemyAttack()
    {
        float probabilidad_clima = 1f;
        if(_enemyAttacking)
        {
            x1 = Acceptance(0.5f);
        }
        else
        {
            x1 = Acceptance(0.2f);
        }
    }

    //Aqui creamos las tablas de verdad para cada uno de los nodos
    public void DayTime()
    {
        
        if(_sceneLight > 0.6f && x1)
        {
            x2 = Acceptance(0.4f);
        }
        else if(_sceneLight> 0.6f && !x1)
        {
            x2 = Acceptance(0.9f);
        }
        else if(_sceneLight < 0.6 && x1)
        {
            x2 = Acceptance(0.7f);
        }
        else
        {
            x2 = Acceptance(0.8f);
        }
    }

    public void Item()
    {
        if(_playerInventory.transform.childCount > 0 && x1)
        {
            x3 = Acceptance(0.8f);
        }
        else if(_playerInventory.transform.childCount > 0 &&!x1)
        {
            x3 = Acceptance(0.4f);
        }
        else if(_playerInventory.transform.childCount == 0 && x1)
        {
            x3 = Acceptance(0.5f);
        }
        else
        {
            x3 = Acceptance(0.1f);
        }
    }

    // Calculo de estados
    public void LunaSangre()
    {
        if (x2 && x3)
        {
            if (Acceptance(0.2f)) 
            {
                estados.Add("LunaSangre");
            }
        }
        else if (x2 && !x3)
        {
            if (Acceptance(0.4f)) 
            {
                estados.Add("LunaSangre");
            }
        }
        else if (!x2 && x3)
        {
            if (Acceptance(0.6f)) 
            {
                estados.Add("LunaSangre");
            }
        }
        else
        {
            if (Acceptance(0.8f)) 
            {
                estados.Add("LunaSangre");
            }
        }
    }


    // Cálculo de Neblina
    public void Neblina()
    {
        if (x2)
        {
            if (Acceptance(0.4f))
            {
                estados.Add("Neblina");
            }
        }
        else
        {
            if (Acceptance(0.6f))
            {
                estados.Add("Neblina");
            }
        }
    }

    // Cálculo de Lluvia
    public void Lluvia()
    {
        if (x2 && x3)
        {
            if (Acceptance(0.7f))
            {
                estados.Add("Lluvia");
            }
        }
        else if (x2 && !x3)
        {
            if (Acceptance(0.2f))
            {
                estados.Add("Lluvia");
            }
        }
        else if (!x2 && x3)
        {
            if (Acceptance(0.6f))
            {
                estados.Add("Lluvia");
            }
        }
        else
        {
            if (Acceptance(0.4f))
            {
                estados.Add("Lluvia");
            }
        }
    }

    public void SelectorEstado(){
        _switch = estados[Random.Range(0,estados.Count)];
        switch(_switch)
        {
            case "LunaSangre":
                _sceneLightColor.color = Color.red;
                break;
            case "Neblina":
                RenderSettings.fog = true;
                RenderSettings.fogColor = Color.gray;
                RenderSettings.fogMode = FogMode.Exponential;
                RenderSettings.fogDensity = .15f;
                break;
            case "Lluvia":
                _rain.Play();
                break;
        }
    }
    
}
