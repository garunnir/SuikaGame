using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SimpleBlink : MonoBehaviour
{
    [SerializeField] private Image m_image;
    private bool m_isRev=false;
    [SerializeField][Range(0f,1f)]float blinkValue;

    void Start()
    {
        m_image = GetComponent<Image>();
        if(m_image == null )enabled = false;
    }
    private void OnEnable()
    {
        Color color = m_image.color;
        color.a = 0;
        m_image.color = color;
    }
    private void OnDisable()
    {
        
    }
    void Update()
    {
        if (m_isRev)
        {
            if (m_image.color.a < 0)
            {
                m_isRev = false;
                return;
            }
            Color color = m_image.color;
            color.a -= blinkValue;
            m_image.color = color;
        }
        else
        {
            if (m_image.color.a > 1)
            {
                m_isRev = true;
                return;
            }
            Color color = m_image.color;
            color.a += blinkValue;
            m_image.color = color;
        }
    }
}
