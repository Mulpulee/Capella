using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveNemo : MonoBehaviour
{
    [SerializeField] private float openY;
    [SerializeField] private float closedY;
    private float waiting = 0.7f;
    private float speed = 0.3f;
    private Coroutine moveCoroutine;

    private Vector3 movePos;

    public void StartMovingBlackSquare(Vector3 pos)
    {
        if (moveCoroutine != null) return;

        movePos = pos;
        moveCoroutine = StartCoroutine(MoveBlackSquareDown());
    }

    private IEnumerator MoveBlackSquareDown()
    {
        while (transform.position.y > closedY)
        {
            transform.Translate(0, -speed, 0);
            yield return null;
        }

        Camera.main.transform.position = movePos;
        yield return new WaitForSeconds(waiting);

        while (openY > transform.position.y)
        {
            transform.Translate(0, speed, 0);
            yield return null;
        }

        moveCoroutine = null;
    }
}