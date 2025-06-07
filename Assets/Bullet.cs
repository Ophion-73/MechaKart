using UnityEngine;
using System.Collections;


public class Bullet : MonoBehaviour
{
    public float velocity;
    public float  lifeTime;

    private void Start()
    {
        StartCoroutine(LifeTime(lifeTime));
    }
    private void Update()
    {
        transform.position += transform.forward * velocity * Time.deltaTime;
        
    }
    public IEnumerator LifeTime(float cd)
    {
        yield return new WaitForSeconds(cd);
        Destroy(gameObject);
    }
}
