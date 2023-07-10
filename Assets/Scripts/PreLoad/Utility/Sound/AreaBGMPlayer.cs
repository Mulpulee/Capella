using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class AreaBGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip BGMClip;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.ins.PlayBGM(BGMClip);
        }
    }
}
