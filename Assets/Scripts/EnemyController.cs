using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Atributos
    // Patrullaje
    [SerializeField] List<Transform> puntosPatrullaje;
    private int currentPatrolPoint;
    Transform puntoFinal;

    // Enemigo
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        Patrullar(puntosPatrullaje);
    }

    // Función para patrullar
    public void Patrullar(List<Transform> puntos)
    {
        // Verificamos que la lista no esté vacía o que tenga un solo elemento
        if (puntos.Count <= 1) return;
        Debug.Log("Entro");
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

    // Modificamos el método para aceptar un GameObject
    public void ReiniciarBusqueda(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            // Dame la distancia al punto más cercano
            float distance = float.MaxValue;
            int index = 0;
            foreach (var punto in puntosPatrullaje)
            {
                index ++;
                float newDistance = Vector3.Distance(punto.position, transform.position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    puntoFinal = punto;
                }
            }
            currentPatrolPoint = index;
            agent.SetDestination(puntoFinal.position);
        }
    }

    // Triggers
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Punto"))
        {
            Patrullar(puntosPatrullaje);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Perseguir(other.transform);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReiniciarBusqueda(other.gameObject); // Pasar el GameObject del jugador
        }
    }

    // Collisions
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.vida -= 1;

            if (playerController.vida == 0)
            {
                Debug.Log("Llamar");
                ReiniciarBusqueda(other.gameObject); // Pasar el GameObject del jugador
            }
        }
    }
}
