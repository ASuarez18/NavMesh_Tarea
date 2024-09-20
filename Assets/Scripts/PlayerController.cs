using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Atributos

    //Movimiento
    public CharacterController player {get; set;}
    public Transform respawn;

    //Estadisticas
    private int vida = 3;
    private float velocidad = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if(vida == 0){
            //Game Over
            vida = 3;
            Respawn(respawn);
        }
    }

    private void Respawn(Transform respawnPoint)
    {
        transform.position = respawnPoint.position;
    }
}
