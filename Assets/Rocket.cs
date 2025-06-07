using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float velocity;
    public Transform enemy;
    public float lifeTime;

    private void Start()
    {
        StartCoroutine(LifeTime(lifeTime));
    }
    private void Update()
    {
        transform.position += transform.forward * velocity * Time.deltaTime;
        transform.LookAt(enemy.position);
    }
    public IEnumerator LifeTime(float cd)
    {
        yield return new WaitForSeconds(cd);
        Destroy(gameObject);
    }


}
