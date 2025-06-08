using TMPro;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI health;
    public GameObject rocketIcon;
    public TextMeshProUGUI fuel;

    [Header("Vida")]
    public int vida = 100;
    private bool da�oRecibido = false;
    public GameObject panelGameOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UI();
    }

    private void UI()
    {
        if (da�oRecibido)
        {
            vida = Mathf.Max(vida - 1, 0);
            da�oRecibido = false;
        }

        if (health != null)
        {
            health.text = vida.ToString();
        }

        if (vida <= 0 && panelGameOver != null && !panelGameOver.activeSelf)
        {
            panelGameOver.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet") || other.CompareTag("rocket"))
        {
            da�oRecibido = true;
        }
    }
}
