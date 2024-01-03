using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]ResourceController m_rc;
    [SerializeField]MergeController m_mc;
    [SerializeField]GameController m_gameController;
    [SerializeField]EventManager m_eventManager;
    [SerializeField]SoundController m_soundController;
    public static bool isGameRunning = true;
    public static bool isFirstRun = true;
    public ResourceController resourceController=>m_rc;
    public MergeController mergeController=>m_mc;
    public GameController gameController=>m_gameController;
    public EventManager eventManager =>m_eventManager;
    public SoundController soundController=>m_soundController;

    [SerializeField] Toggle m_ShowGuideline;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        m_ShowGuideline.onValueChanged.AddListener((x)=> { PlayerPrefs.SetInt("showGuideline", x ? 0 : 1); });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
