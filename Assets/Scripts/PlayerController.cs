using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    public CharacterController _character { get; set; }
    public Transform respawn;

    //Estadísticas
    private int vida = 3;
    private float velocidad = 7f;
    private Vector3 movimiento;

    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vida == 0)
        {
            //Game Over
            vida = 3;
            Respawn(respawn);
        }

        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical"); 

        movimiento = new Vector3(moveX, 0, moveZ);

        if (movimiento.magnitude > 1)
        {
            movimiento.Normalize();
        }
        _character.Move(movimiento * velocidad * Time.deltaTime);
    }

    private void Respawn(Transform respawnPoint)
    {
        transform.position = respawnPoint.position;
    }
}
