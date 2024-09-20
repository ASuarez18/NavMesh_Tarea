using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    //Atributos

    //Patrullaje
    [SerializeField] List<Transform> puntosPatrullaje;
    private int currentPatrolPoint;

    //Enemigo
    public NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        Patrullar(puntosPatrullaje);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Función para patrullar
    public void Patrullar(List<Transform> puntos)
    {
        //Verificamos que la lista no este vacia o que tenga un solo elemento
        if(puntos.Count <= 1) return;
        int newPatrolPoint;
        do
        {
            newPatrolPoint = Random.Range(0, puntos.Count);
        } while (newPatrolPoint == currentPatrolPoint);
        currentPatrolPoint = newPatrolPoint;
        agent.SetDestination(puntos[currentPatrolPoint].position);
            
    }

    public void Perseguir(Transform player)
    {
        agent.SetDestination(player.position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Punto"){
            Patrullar(puntosPatrullaje);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player"){
            Perseguir(other.transform);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player"){
            Debug.Log("Attack -10 life");
        }
    }
}
