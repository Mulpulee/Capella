using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveNemo : MonoBehaviour
{
    public GameObject blackSquare;
    public GameObject whiteSquare;
    public Button moveButton;

    private float blackY;
    private float whiteY;
    private float waiting;
    private Coroutine moveCoroutine;

    public void Start()
    {
        waiting = 2f;
        blackY = blackSquare.transform.position.y;
        whiteY = whiteSquare.transform.position.y;

        moveButton.onClick.AddListener(StartMovingBlackSquare);
    }

    private void StartMovingBlackSquare()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveBlackSquareDown());
    }

    private IEnumerator MoveBlackSquareDown()
    {
        while (blackSquare.transform.position.y > whiteY)
        {
            blackSquare.transform.Translate(0, -1, 0);
            yield return null;
        }
        yield return new WaitForSeconds(waiting);

        while (blackY > blackSquare.transform.position.y)
        {
            blackSquare.transform.Translate(0, 1, 0);
            yield return null;
        }
    }

}