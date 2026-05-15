using UnityEngine;

public class CartaScript : MonoBehaviour
{
// Sprites
    public Sprite anverso;
    public Sprite reverso;

    // Valor de la carta
    public int valor;

    // Índice
    public int indice;

    // Saber si está boca arriba
    private bool cartas_boca_arriba = false;

    // Saber si ya fue encontrada
    public bool parejaEncontrada = false;
 
    // SpriteRenderer
    private SpriteRenderer sr;

    // Referencia al GameManager
    private GameObject gameManager;

    // Awake es llamado antes de Start y es ideal para inicializar componentes
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    // Start es llamado antes del primer frame update
    void Start()
    {
        sr.sprite = reverso;
    }

    // Función para girar la carta
    public void Girar()
    {
        cartas_boca_arriba = !cartas_boca_arriba;// Cambiar el estado de true a false o de false a true

        if (cartas_boca_arriba)
        {
            sr.sprite = anverso;
        }
        else
        {
            sr.sprite = reverso;
        }
    }

    // Obtener estado
    public bool EstaBocaArriba()
    {
        return cartas_boca_arriba;
    }

    // OnMouseDown es un evento de Unity que se llama cuando el usuario hace clic en el objeto con el mouse
    void OnMouseDown()
    {   
        // Si ya está descubierta permanentemente
        if (parejaEncontrada)
        {
            return;
        }

        // Si ya está boca arriba
        if (cartas_boca_arriba)
        {
            return;
        }

        // Girar la carta
        Girar();
        // Avisar al GameManager para que actualice el estado
        gameManager.GetComponent<GameManager>().CambiarEstado(indice);
    }
}
