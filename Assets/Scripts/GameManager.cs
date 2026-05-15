using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Array de cartas
    public GameObject[] cartas;

    // Cartas boca arriba
    private int[] cartas_boca_arriba = new int[2]; //el new int[2] es para inicializar el array con dos elementos, que representarán las cartas boca arriba

    // Estado del juego
    private string estado; // "inicial" o "descubierta"

    // Control bloqueo
    private bool bloqueado = false; // Evita que el jugador pueda interactuar mientras las cartas están girando

    // Marcador
    public TMP_Text textoMarcador; 

    // Botón reinicio
    public GameObject botonReinicio; // El botón de reinicio se muestra al finalizar el juego

    // Número de parejas
    private int parejas = 0;

    void Start()
    {
        // Buscar cartas
        cartas = GameObject.FindGameObjectsWithTag("Carta");

        // Asignar índices, esto es importante para que cada carta sepa su posición en el array y pueda informar al GameManager cuando sea clickeada
        for (int i = 0; i < cartas.Length; i++)
        {
            cartas[i].GetComponent<CartaScript>().indice = i;
        }

        // Inicializar
        cartas_boca_arriba[0] = -1;
        cartas_boca_arriba[1] = -1;

        estado = "inicial";

        // Ocultar botón
        botonReinicio.SetActive(false);

        // Mezclar cartas
        MezclarCartas();

        ActualizarMarcador();
    }

     public void CambiarEstado(int indice)
    {
        if (bloqueado) return;

        if (estado == "inicial")
        {
            cartas_boca_arriba[0] = indice;
            estado = "descubierta";
        }
        else if (estado == "descubierta")
        {

            cartas_boca_arriba[1] = indice;

            // Verificar si son pareja
            CartaScript carta1 = cartas[cartas_boca_arriba[0]].GetComponent<CartaScript>();
            CartaScript carta2 = cartas[cartas_boca_arriba[1]].GetComponent<CartaScript>();

            if (carta1.valor == carta2.valor)
            {
                parejas++;
                ActualizarMarcador();

                carta1.parejaEncontrada = true;
                carta2.parejaEncontrada = true;

                if (parejas == cartas.Length / 2)
                {
                    botonReinicio.SetActive(true);
                }
    
                ReiniciarEstado();
            }
            else
            {
                StartCoroutine(GirarCartas(carta1, carta2));
            }
        }
    }
    // Coroutine para girar cartas
    IEnumerator GirarCartas(CartaScript c1, CartaScript c2)
    {
        bloqueado = true;

        // Esperar 1 segundo
        yield return new WaitForSeconds(1f);

        // Girar cartas
        c1.Girar();
        c2.Girar();

        bloqueado = false;

        ReiniciarEstado();
    }

    void ReiniciarEstado() // Reinicia el estado para la siguiente jugada, el -1 indica que no hay carta boca arriba
    {
        cartas_boca_arriba[0] = -1;
        cartas_boca_arriba[1] = -1;

        estado = "inicial";
    }

    void ActualizarMarcador()
    {
        textoMarcador.text = "Parejas: " + parejas;
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarga la escena actual para reiniciar el juego
    }

    void MezclarCartas()
    {
        for (int i = 0; i < cartas.Length; i++) 
        {
            Vector3 temp = cartas[i].transform.position; //el vector3 temp se utiliza para almacenar temporalmente la posición de la carta actual antes de intercambiarla con otra carta aleatoria

            int randomIndex = Random.Range(i, cartas.Length); //se genera un índice aleatorio entre el índice actual (i) y el último índice del array de cartas (cartas.Length). Esto asegura que cada carta se mezcle con una carta que aún no ha sido mezclada, evitando así que una carta se mezcle consigo misma o con una carta ya mezclada.

            cartas[i].transform.position = cartas[randomIndex].transform.position; //la posición de la carta actual (cartas[i]) se establece en la posición de la carta aleatoria seleccionada (cartas[randomIndex]), lo que efectivamente intercambia las posiciones de las dos cartas.

            cartas[randomIndex].transform.position = temp; //la posición de la carta aleatoria seleccionada (cartas[randomIndex]) se establece en la posición almacenada en temp, que es la posición original de la carta actual (cartas[i]). Esto completa el intercambio de posiciones entre las dos cartas.
        }
    }
}
