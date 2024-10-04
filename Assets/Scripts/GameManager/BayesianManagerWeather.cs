using UnityEngine;
using System.Collections.Generic;

public class BayesianManagerWeather : MonoBehaviour
{
    // Lista que contiene las probabilidades combinadas de cada nodo
    private List<float> probabilidades = new List<float>();

    // Bandera para verificar si el enemigo está atacando
    private bool _enemyAttacking = false;

    // Referencia a la luz ambiental de la escena
    [SerializeField]
    private Light sceneLight;

    // Referencia al inventario del jugador
    [SerializeField]
    private GameObject playerInventory;

    // Función para calcular la probabilidad combinada
    private float CalculateProbability(float baseProbability, float acceptanceThreshold)
    {
        return Acceptance(acceptanceThreshold) ? baseProbability * acceptanceThreshold : baseProbability * (1 - acceptanceThreshold);
    }

    // Función que determina si se acepta una probabilidad con un valor umbral
    public bool Acceptance(float value)
    {
        return Random.Range(0, 1f) > value;
    }

    // Función para calcular la probabilidad basada en la luz ambiental actual de la escena
    public void DayTime()
    {
        // Suponiendo que la luz de la escena es más intensa durante el día
        float currentLightIntensity = sceneLight.intensity;  // Intensidad de la luz actual
        float maxLightIntensity = 1f;  // Máxima intensidad de luz que representa el día

        // Calcular probabilidad de día basada en la luz actual
        float probabilidad_dia = currentLightIntensity / maxLightIntensity;
        float probabilidad_noche = 1 - probabilidad_dia;

        probabilidades.Add(probabilidad_noche);  // Guardamos la probabilidad de que sea de noche
    }

    // Función para calcular la probabilidad basada en los objetos que el jugador tiene en su inventario
    public void Item()
    {
        // Por ejemplo, si el jugador tiene un objeto especial que aumenta las probabilidades
        int hasSpecialItem = playerInventory.transform.childCount;

        float probabilidad_item = hasSpecialItem > 2 ? 0.7f : 0.3f;  // Mayor probabilidad si el jugador tiene el objeto
        float probabilidad_item_final = CalculateProbability(probabilidad_item, 0.5f);

        probabilidades.Add(probabilidad_item_final);  // Guardamos la probabilidad final del ítem
    }

    // Función que calcula la probabilidad de un ataque enemigo
    public void EnemyAttack()
    {
        float probabilidad_clima = 1f;

        if (_enemyAttacking && probabilidades[0] > 0.5f)  // Condición si es probable que haya ataque
        {
            probabilidad_clima = CalculateProbability(probabilidad_clima, 0.7f);  // Luna sangrienta en caso de ataque
            probabilidad_clima = CalculateProbability(probabilidad_clima, 0.6f);  // Neblina
            probabilidad_clima = CalculateProbability(probabilidad_clima, 0.3f);  // Lluvia
        }
        else  // Si no hay ataque enemigo
        {
            probabilidad_clima = CalculateProbability(probabilidad_clima, 0.4f);  // Luna sangrienta en caso de no ataque
            probabilidad_clima = CalculateProbability(probabilidad_clima, 0.3f);  // Neblina en caso de no ataque
            probabilidad_clima = CalculateProbability(probabilidad_clima, 0.2f);  // Lluvia en caso de no ataque
        }

        probabilidades.Add(probabilidad_clima);  // Guardamos la probabilidad combinada de condiciones climáticas
    }

    // Función para calcular si ocurren diferentes condiciones climáticas simultáneamente
    public float WeatherCondition()
    {
        float condition = 1f;
        
        foreach (float probabilidad in probabilidades)
        {
            condition *= probabilidad;
        }

        // Separamos las condiciones para que más de una pueda ocurrir simultáneamente
        if (condition > 0.9f)
        {
            Debug.Log("Luna Sangrienta activada");
        }
        
        if (condition > 0.5f && condition <= 0.9f)
        {
            Debug.Log("Neblina activada");
        }

        if (condition > 0.2f && condition <= 0.5f)
        {
            Debug.Log("Lluvia activada");
        }

        return condition;
    }

    
}

//Script de puebra version 1 Ejemplo


// public class BayesianManagerWeather : MonoBehaviour
// {
//     //Primero debemos de obtener nuestros tres entradas o nodos que modificaran el clima
//     [SerializeField] private Light _sceneLight;
//     [SerializeField] private GameObject _playerInventory;
//     [SerializeField] private bool _enemyAttacking;

//     //Luego creamos una lista para almacenar los valores de los nodos anteriores
//     List<float> probabilidades;

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     //Creamos una funcion que reciba un umbral
//     public bool Acceptance(float value)
//     {
//         return Random.Range(0, 1f) > value;
//     }


//     public float BayesianWeather()
//     {
//         DayTime();
//         Item();
//         EnemyAttack();
//         return WeatherCondition();
//     }

//     //Aqui creamos las tablas de verdad para cada uno de los nodos
//     public void DayTime()
//     {
//         var probabilidad_clima = 1f;
//         //Si observamos que es de dia, bajamos la probabilidas que sea de luna sangrienta ,neblina y lluvia
//         if(_sceneLight > 0.6f)
//         {
//             //Decidimos nuestra tabla de verdad para los tres casos

//             //En este caso si es de dia digamos que la aceptacion o probabilidad para luna sangrienta es de 10% osea 0.9
//             Acceptance(0.9)?probabilidad_clima*=0.9f:probabilidad_clima*=0.1f;
//             //En este caso si es de dia digamos que la aceptacion o probabilidad para neblina es de 50% osea 0.5
//             Acceptance(0.5)?probabilidad_clima*=0.5f:probabilidad_clima*=0.5f;
//             //En este caso si es de dia digamos que la aceptacion o probabilidad para lluvia es de 40& osea 0.6
//             Acceptance(0.6)?probabilidad_clima*=0.6f:probabilidad_clima*=0.4f;

//             //Agregamos la probabilidad en conjunto en la lista
//             probabilidades.Add(probabilidad_clima);
//             return;
//         }
//         //Ahora observamos que es de noche , subimos las probabilidades que sea luna sangrienta , neblina y lluva
//         else
//         {
//             //Decidimos nuestra tabla de verdad para los tres casos
//             //En este caso si es de noche digamos que la aceptacion o probabilidad para luna sangrienta es de 50% osea 0.5
//             Acceptance(0.5)?probabilidad_clima*=0.5f:probabilidad_clima*=0.5f;
//             //En este caso si es de noche digamos que la aceptacion o probabilidad para neblina es de 70% osea 0.3
//             Acceptance(0.3)?probabilidad_clima*=0.7f:probabilidad_clima*=0.3f;
//             //En este caso si es de noche digamos que la aceptacion o probabilidad para lluvia es de 60% osea 0.4
//             Acceptance(0.4)?probabilidad_clima*=0.6:probabilidad_clima*=0.4f;

//             //Agregamos la probabilidad en conjunto en la lista
//             probabilidades.Add(probabilidad_clima);
//             return;
//         }
//     }

//     public void Item()
//     {
//         //Contamos cuantos hijos o items tiene el inventario del jugador, entre mas tenga aumenta la probabilidad de lo contrario no
//         int numItems = _playerInventory.transform.childCount;
//         var probabilidad_clima = 1f;
//         if(numItems > 3)
//         {
//             //En este caso si eel juagdor tiene mas 3 o mas items la aceptacion o probabilidad para luna sangrienta es de 70% osea 0.3
//             Acceptance(0.3)?probabilidad_clima*=0.7f:probabilidad_clima*=0.3f;
//             //En este caso si eel juagdor tiene mas 3 o mas items la aceptacion o probabilidad para neblina es de 50% osea 0.5
//             Acceptance(0.7)?probabilidad_clima*=0.3f:probabilidad_clima*=0.7f;
//             //En este caso si el jugador tiene 3 0 mas items la aceptacion o probabilidad para el clima es de 90% osea 0.1
//             Acceptance(0.1)?probabilidad_clima*=0.9f:probabilidad_clima*=0.1f;

//             //Agregamos la probabilidad en conjunto en la lista
//             probabilidades.Add(probabilidad_clima);
//             return;
//         }
//         else
//         {
//             //En este caso si es de dia digamos que la aceptacion o probabilidad para luna sangrienta es de 20% osea 0.8
//             Acceptance(0.8)?probabilidad_clima*=0.2f:probabilidad_clima*=0.8f;
//             //En este caso si es de dia digamos que la aceptacion o probabilidad para neblina es de 80% osea 0.2
//             Acceptance(0.2)?probabilidad_clima*=0.8f:probabilidad_clima*=0.2f;
//             //En este caso si es de dia digamos que la aceptacion o probabilidad para lluvia es de 10% osea 0.9
//             Acceptance(0.9)?probabilidad_clima*=0.1f:probabilidad_clima*=0.9f;

//             //Agregamos la probabilidad en conjunto en la lista
//             probabilidades.Add(probabilidad_clima);
//             return;
//         }
//     }

//     public void EnemyAttack()
//     {
//         //Este nodo depende de la probabilidad del pimer nodo (DayTime)
//         if(probabilidades[0] > 0.5)
//         {
//             //En este caso como este nodo depende del primer vemos que si es muy probable que sea de noche el criterio de aceptacion que este siendo atacado es de 70% y para la luna 
//             Acceptance(0.3)?probabilidad_clima*=0.f:probabilidad_clima*=0.6f;
//             //En este caso si el enemigo esta atacando la aceptacion o probabilidad para neblina es de 60% osea 0.4
//             Acceptance(0.4)?probabilidad_clima*=0.6f:probabilidad_clima*=0.4f;
//             //En este caso si el enemigo esta atacando la aceptacion o probabilidad para lluvia es de 30% osea 0.7
//             Acceptance(0.7)?probabilidad_clima*=0.3f:
//         }
//     }

//     public float WeatherCondition()
//     {
//         //Aca se calcula la condicion de clima segun la multiplicacion de las probabilidades de cada uno de los nodos
//         float condition = 1f;
//         foreach(float probabilidad in probabilidades)
//         {
//             condition *= probabilidad;
//         }

//         //Aqui jugamos con el valor de la probabilidad que obtuvimos y activamos los tres tipos diferentes de climas
//         if(condition > 0.9)
//         {
//             //Si la condicion es muy alta, activamos el clima luna sangrienta
//             return 1f;
//         }
//         else if(condition > 0.5)
//         {
//             //Si la condicion es media, activamos el clima neblina
//             return 0.5f;
//         }
//         else
//         {
//             //Si la condicion es baja, activamos el clima lluvia
//             return 0f;
//         }
//     }

// }
