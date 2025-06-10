using UnityEngine;
using System.Collections;

public class BlockMover : MonoBehaviour
{
    public float moveDistance = 2.0f;
    public float moveSpeed = 2.0f;
    public Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    public IEnumerator MoveUp()
    {
        Vector3 targetPos = startPos + Vector3.up * moveDistance;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
    }

    public IEnumerator MoveDown()
    {
        Vector3 targetPos = startPos;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
    }
}
