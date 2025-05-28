using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Knockback : MonoBehaviour
{
    private Rigidbody rb;
    private float strength = 60f;
    private float delay = 0.15f;
    public UnityEvent OnBegin, OnDone;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.linearVelocity = Vector3.zero;
        OnDone?.Invoke();
    }

    public void PlayFeedback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector3 direction = (transform.position - sender.transform.position).normalized;
        direction.y = 0.1f;
        rb.AddForce(direction * strength, ForceMode.Impulse);
        StartCoroutine(Reset());
    }
}