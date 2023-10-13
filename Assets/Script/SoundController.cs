using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    public enum Audio
    {
        creek,
    }
    AudioSource m_AudioSource;
    [SerializeField]AudioClip[] m_clips;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    public void OneShot(Audio idx)
    {
        m_AudioSource.PlayOneShot(m_clips[(int)idx]);
    }
}
