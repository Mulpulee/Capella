using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatorComponent : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private Func<Single> m_timeMethod;

    private Dictionary<Int32, AnimationData> m_animationByKey;
    private Int32 m_nowState;
    private Single m_elapsed;
    private Single m_targetSeconds;
    private Int32 m_nowSpriteIndex;

    private Int32 m_beforeKey;
    private Boolean m_isAnimationForced;
    private Boolean m_isAnimationLocked;

    private void Awake()
    {
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_timeMethod = () => Time.deltaTime;

        m_animationByKey = new Dictionary<Int32, AnimationData>();
        m_nowState = -1;
    }

    private void Update()
    {
        if (m_nowState == -1)
        {
            Debug.LogError($"AnimatorComponent :: animator is not initialized!! {gameObject.name}");
            return;
        }

        if (m_nowSpriteIndex == -1)
            return;

        m_elapsed += m_timeMethod.Invoke();

        if (m_elapsed > m_targetSeconds)
        {
            if (m_nowSpriteIndex == -3)
            {
                return;
            }

            if (m_nowSpriteIndex == -2)
            {
                ChangeState(m_beforeKey);
                m_isAnimationForced = false;
                return;
            }

            m_elapsed = 0;
            m_nowSpriteIndex++;

            if (m_nowSpriteIndex >= m_animationByKey[m_nowState].sprites.Count)
            {
                if (m_isAnimationLocked)
                {
                    m_targetSeconds = m_animationByKey[m_nowState].exitTime;
                    m_nowSpriteIndex = -3;
                    return;
                }

                if (m_isAnimationForced)
                {
                    m_targetSeconds = m_animationByKey[m_nowState].exitTime;
                    m_nowSpriteIndex = -2;
                    return;
                }

                if (!m_animationByKey[m_nowState].loop)
                {
                    m_nowSpriteIndex = -1;
                    return;
                }
                else
                    m_nowSpriteIndex = 0;
            }

            m_spriteRenderer.sprite = m_animationByKey[m_nowState].sprites[m_nowSpriteIndex];
        }
    }

    public void AddAnimationData<T>(T pKey, AnimationData pData) where T : struct
    {
        Int32 key = Utility.UnsafeUtility.EnumToInt32<T>(pKey);

        m_animationByKey.Add(key, pData);
    }

    public void AddAnimationData<T>(List<Pair<T, AnimationData>> pDatas) where T : struct
    {
        foreach (var item in pDatas)
        {
            AddAnimationData<T>(item.primary, item.secondary);
        }
    }

    public void Initialize<T>(T pInitialState) where T : struct
    {
        ChangeState<T>(pInitialState);
    }

    public void ChangeState<T>(T pKey) where T : struct
    {
        if (m_isAnimationForced || m_isAnimationLocked)
            return;

        Int32 key = Utility.UnsafeUtility.EnumToInt32<T>(pKey);

        if (!m_animationByKey.ContainsKey(key))
            return;

        if (m_nowState == key)
            return;

        m_nowState = key;
        m_elapsed = 0;
        m_nowSpriteIndex = 0;
        m_targetSeconds = 1f / m_animationByKey[key].spritePerSeconds;
        m_spriteRenderer.sprite = m_animationByKey[m_nowState].sprites[m_nowSpriteIndex];
    }

    public void PlayOnce<T>(T pKey) where T : struct
    {
        if (m_isAnimationForced || m_isAnimationLocked)
            return;

        Int32 key = Utility.UnsafeUtility.EnumToInt32<T>(pKey);

        if (m_nowState == key)
            return;

        m_beforeKey = m_nowState;

        m_nowState = key;
        m_elapsed = 0;
        m_nowSpriteIndex = 0;
        m_targetSeconds = 1f / m_animationByKey[key].spritePerSeconds;
        m_spriteRenderer.sprite = m_animationByKey[m_nowState].sprites[m_nowSpriteIndex];

        m_isAnimationForced = true;
    }

    public void PlayLocked<T>(T pKey) where T : struct
    {
        if (m_isAnimationLocked)
            return;

        Int32 key = Utility.UnsafeUtility.EnumToInt32<T>(pKey);

        if (m_nowState == key)
            return;

        m_beforeKey = m_nowState;

        m_nowState = key;
        m_elapsed = 0;
        m_nowSpriteIndex = 0;
        m_targetSeconds = 1f / m_animationByKey[key].spritePerSeconds;
        m_spriteRenderer.sprite = m_animationByKey[m_nowState].sprites[m_nowSpriteIndex];

        m_isAnimationLocked = true;
    }

    public void Unlock()
    {
        m_isAnimationLocked = false;
        m_nowSpriteIndex = 0;
        ChangeState(m_beforeKey);
    }

    public AnimatorComponent Initialize<T>(T pInitialState,AnimationSet pAnimationSet) where T : struct
    {
        foreach (var item in pAnimationSet.animations)
        {
            AddAnimationData<T>(Utility.EnumUtility.GetEnumByName<T>(item.primary), item.secondary);
        }

        Initialize<T>(pInitialState);

        return this;
    }

    public void SetTimeMethod(Func<Single> pMethod)
    {
        m_timeMethod = pMethod;
    }
}