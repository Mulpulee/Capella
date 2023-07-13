using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDate : MonoBehaviour
{
    [SerializeField] private Text prev;
    [SerializeField] private Text next;

    private SoundManagerEx sm;

    private void Start()
    {
        sm = GameObject.FindObjectOfType<SoundManagerEx>();
    }

    public IEnumerator ChangeAnim(Action callback)
    {
        int day = GameObject.FindObjectOfType<GameManagerEx>().GetDay();
        prev.text = (day - 1).ToString();
        next.text = day.ToString();

        yield return new WaitForSeconds(5f);

        sm.OnSfx(10);
        StartCoroutine(MoveCoroutine(transform, new Vector3(-167, 70), 300f));

        yield return new WaitForSeconds(5f);

        callback.Invoke();
    }

    private IEnumerator MoveCoroutine(Transform _transform, Vector3 pos, float duration)
    {
        float dir = (pos.y - _transform.localPosition.y) / duration;
        while ((dir > 0 && _transform.localPosition.y <= pos.y)
            || (dir < 0 && _transform.localPosition.y >= pos.y))
        {
            _transform.localPosition += new Vector3(0, dir, 0);
            yield return null;
        }

        _transform.localPosition = pos;

        yield return null;
    }
}
