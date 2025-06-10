using UnityEngine;
using System.Collections;

public class BlockMovementManager : MonoBehaviour
{
    public BlockMover block1;
    public BlockMover block2;
    public BlockMover block3;
    public BlockMover block4;
    public float holdTime = 2.0f;
    public float jumpTime = 0.5f;

    void Start()
    {
        StartCoroutine(CoordinateBlocks());
    }

    IEnumerator CoordinateBlocks()
    {
        while (true)
        {
            yield return StartCoroutine(block2.MoveUp());
            yield return StartCoroutine(block4.MoveUp());
            yield return new WaitForSeconds(jumpTime);
            yield return StartCoroutine(block1.MoveDown());
            yield return StartCoroutine(block3.MoveDown());
            yield return new WaitForSeconds(holdTime);

            yield return StartCoroutine(block1.MoveUp());
            yield return StartCoroutine(block3.MoveUp());
            yield return new WaitForSeconds(jumpTime);
            yield return StartCoroutine(block2.MoveDown());
            yield return StartCoroutine(block4.MoveDown());
            yield return new WaitForSeconds(holdTime);
        }
    }
}
