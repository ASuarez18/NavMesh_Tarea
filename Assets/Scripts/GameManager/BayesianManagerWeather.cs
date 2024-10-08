using UnityEngine;
using System.Collections.Generic;

public class BayesianManagerWeather : MonoBehaviour
{
    //Primero debemos de obtener nuestros tres entradas o nodos que modificaran el clima
    public float _sceneLight;
    public bool _enemyAttacking;

    //Variables de probabilidad
    private bool x1,x2,x3;

    //Manager de estados
    private string _switch;

    //Luego creamos una lista para almacenar los valores de los nodos anteriores
    private List<float> probabilidades;

    //Lista de estados activos
    private List<string> estados;

    //banderas
    private bool neblina, lluvia;

    //Duracion de la neblina y lluvia
    private float neblinaDuration , lluviaDuration;

    //Variables de entorno
    [SerializeField] public Light _sceneLightColor;
    [SerializeField] public ParticleSystem _rain;
    [SerializeField] public GameObject _playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        estados = new List<string>();
        //Inicializa las variables y contadores
        neblinaDuration = 10f;
        lluviaDuration = 10f;
        neblina = false;
        lluvia = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(neblina)
        {
            neblinaDuration -= Time.deltaTime;
        }
        if(lluvia)
        {
            lluviaDuration -= Time.deltaTime;
        }
        if(neblinaDuration <= 0f)
        {
            neblina = false;
            neblinaDuration = 10f;
            RenderSettings.fog = false;
        }
        if(lluviaDuration <= 0f)
        {
            lluvia = false;
            lluviaDuration = 10f;
            _rain.Stop();
        }
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
        Debug.Log(x1);
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
        Debug.Log(x2);
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
        Debug.Log(x3);
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
        if(estados.Count != 0)
        {
            _switch = estados[Random.Range(0,estados.Count)];
        }
       
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
                neblina = true;
                break;
            case "Lluvia":
                _rain.Play();
                lluvia = true;
                break;
            default:
                Debug.Log("No hay ningun estado");
                break;
        }
    }
    
}
