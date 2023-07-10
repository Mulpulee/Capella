using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Extensions
{
    public enum UITransitionType
    {
        Scale,
        Fade,
    }

    public static class UIExtensions
    {

        public static void Show(this Image image)
        {
            image.color = Color.white;
        }

        public static void Hide(this Image image)
        {
            image.color = Color.clear;
            image.sprite = null;
        }

        public static void SetAlpha(this Image image, Single pAlpha)
        {
            Color temp = image.color;
            temp.a = pAlpha;
            image.color = temp;
        }

        public static void Show(this CanvasGroup canvasGroup, Single pTransitionDuration = 1, UITransitionType pTransitionType = UITransitionType.Scale,Action pTransitionCallback = null)
        {
            canvasGroup.DOKill();
            canvasGroup.transform.DOKill();
            canvasGroup.transform.localScale = new Vector3(1,1,1);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            if(pTransitionDuration == 0)
            {
                canvasGroup.alpha = 1;
                return;
            }

            switch (pTransitionType)
            {
                case UITransitionType.Scale:
                    {
                        canvasGroup.transform.localScale = new Vector3(0, 0, 0);
                        canvasGroup.DOFade(1, pTransitionDuration);
                        canvasGroup.transform.DOScale(1, pTransitionDuration).OnComplete(() =>
                        {
                            canvasGroup.transform.localScale = new Vector3(1, 1, 1);
                            pTransitionCallback?.Invoke();
                        });
                    }
                    break;
                case UITransitionType.Fade:
                    canvasGroup.DOFade(1, pTransitionDuration).OnComplete(() =>
                    {
                        pTransitionCallback?.Invoke();
                    });
                    break;
                default:
                    break;
            }
        }

        public static void Hide(this CanvasGroup canvasGroup, Single pTransitionDuration, UITransitionType pTransitionType = UITransitionType.Scale, Action pTransitionCallback = null)
        {
            canvasGroup.DOKill();
            canvasGroup.transform.DOKill();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            if (pTransitionDuration == 0)
            {
                canvasGroup.alpha = 0;
                return;
            }

            switch (pTransitionType)
            {
                case UITransitionType.Scale:
                    {
                        canvasGroup.DOFade(0, pTransitionDuration);
                        canvasGroup.transform.DOScale(0, pTransitionDuration).OnComplete(() =>
                        {
                            canvasGroup.transform.localScale = new Vector3(1, 1, 1);
                            pTransitionCallback?.Invoke();
                        });
                    }
                    break;
                case UITransitionType.Fade:
                    canvasGroup.DOFade(0, pTransitionDuration).OnComplete(() =>
                    {
                        pTransitionCallback?.Invoke();
                    });
                    break;
                default:
                    break;
            }
        }
        public static Boolean isActive(this CanvasGroup canvasGroup)
        {
            return canvasGroup.alpha == 1;
        }
    }

}
