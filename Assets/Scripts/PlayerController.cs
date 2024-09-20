using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController _character { get; set; }
    public Transform respawn;

    // Estadísticas
    public int vida {get; set;}
    private float velocidad = 7f;
    private Vector3 movimiento;

    // Variables para la gravedad
    private float velocidadVertical = 0f;
    private const float gravedad = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        vida = 3;
        _character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (vida == 0)
        {
            // Game Over
            vida = 3;
            Respawn(respawn);
        }

        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical"); 

        // Movimiento en el plano horizontal
        movimiento = new Vector3(moveX, 0, moveZ);

        if (movimiento.magnitude > 1)
        {
            movimiento.Normalize();
        }

        // Comprobar si estamos en el suelo
        if (_character.isGrounded)
        {
            velocidadVertical = 0f; // Reiniciar la velocidad vertical al tocar el suelo
        }
        else
        {
            // Aplicar gravedad si no estamos en el suelo
            velocidadVertical += gravedad * Time.deltaTime;
        }

        // Añadir la componente vertical (gravedad)
        movimiento.y = velocidadVertical;

        // Mover al personaje
        _character.Move(movimiento * velocidad * Time.deltaTime);
    }

    private void Respawn(Transform respawnPoint)
    {
        gameObject.SetActive(false);
        transform.position = respawnPoint.position;
        gameObject.SetActive(true);
    }
}
