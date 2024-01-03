using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum VFX
    {
        Poof,
        HeavyShot,
    }
    int m_score;
    [SerializeField] RectTransform m_endline;
    [SerializeField] Transform m_CreatePos;
    [SerializeField] TMP_Text m_Text;
    [SerializeField] Transform m_cloneGroup;
    [SerializeField] Transform m_GameOverGroup;
    [SerializeField] TMP_Text m_Score;
    [SerializeField] BoxCollider2D[] boxcols;
    [SerializeField] Button m_heavyShot;
    [SerializeField] Image[] m_heavyShotRemain;
    [SerializeField] Transform[] m_VFXS;
    Transform[] m_HoldVFXS;
    [SerializeField] RectTransform[] m_BlockUIs;
    [SerializeField] Button m_Btn_Restart;
    [SerializeField] GameObject[] m_Guidelines;
    private bool m_heavyShotEnabled=true;
    public UnityAction GameStarted;
    public Transform GetCloneParent() { return m_cloneGroup; }
    public Transform GetCreatePos() =>m_CreatePos;
    public TMP_Text GetText() =>m_Text;
    public int GetScore() => m_score;
    public void AddScore(int score) => m_score+=score;
    public bool CalBlocKUIs()
    {
        foreach (var item in m_BlockUIs)
        {
            if(item.gameObject.activeSelf)
            if(RectTransformUtility.RectangleContainsScreenPoint(item, Input.mousePosition))return true;
        }
        return false;
    }
    public Transform GetHoldVFX(VFX vfx)
    {
        int idx = (int)vfx;
        return m_HoldVFXS[idx];
    }
    public void ShowScore(int score)
    {
        m_Text.text = "Score: " + score;
    }
    public void ShowScore()
    {
        m_Text.text = "Score: " + m_score;
    }
    public float[] CalLeftRight()
    {
        return new float[] { boxcols[0].transform.position.x + boxcols[0].offset.x + boxcols[0].size.x / 2, boxcols[1].transform.position.x + boxcols[1].offset.x - boxcols[0].size.x / 2 };
    }
    public Transform SetVFX(VFX vfx,Vector3 worldpos,Transform parent,bool active=true,bool isMulti=false)
    {
        int idx = (int)vfx;
        Transform tp = null;
        if(m_HoldVFXS==null) m_HoldVFXS = new Transform[m_VFXS.Length];
        if (m_HoldVFXS[idx] == null||isMulti)
        {
            tp= Instantiate(m_VFXS[idx]);
            m_HoldVFXS[idx] = tp;
        }
        else
        {
            tp = m_HoldVFXS[idx];
        }
        tp.gameObject.SetActive(active);
        tp.SetParent(parent);
        //if(isChild )
        //{
        //    tp.localPosition = Vector3.zero;
        //    tp.localScale = Vector3.one;
        //}
        //else
        //{
        //}
        if(parent != null)
        {
            Vector3 vector3 = worldpos;
            vector3.z = tp.position.z;
            tp.position = vector3;
        }
        tp.localScale = Vector3.one;

        return tp;
    }
    public Transform SetVFX(VFX vfx, Transform parent, bool active = true, bool isMulti = false)
    {
        int idx = (int)vfx;
        Transform tp = null;
        if (m_HoldVFXS == null) m_HoldVFXS = new Transform[m_VFXS.Length];
        if (m_HoldVFXS[idx] == null || isMulti)
        {
            tp = Instantiate(m_VFXS[idx]);
            m_HoldVFXS[idx] = tp;
        }
        else
        {
            tp = m_HoldVFXS[idx];
        }
        tp.gameObject.SetActive(active);
        if(active)
        {
            tp.SetParent(parent);
        }
        else
        {
            tp.SetParent(null);
        }
        //if(isChild )
        //{
        //    tp.localPosition = Vector3.zero;
        //    tp.localScale = Vector3.one;
        //}
        //else
        //{
        //}
        if (parent != null)
        {
            Vector3 vector3 = parent.position;
            vector3.z = tp.position.z;
            tp.position = vector3;
        }
        tp.localScale = Vector3.one;

        return tp;
    }
    void Start()
    {
        m_Btn_Restart.onClick.AddListener(() => { Restart(); });
        GameManager.Instance.gameController.GameStarted += () =>
        {

            foreach (var item in m_heavyShotRemain)
            {
                item.gameObject.SetActive(true);
            }
        };
        CalLeftRight();
        m_HoldVFXS = new Transform[m_VFXS.Length];
        SetVFX(VFX.HeavyShot, null);
        GetHoldVFX(VFX.HeavyShot).gameObject.SetActive(false);
        m_heavyShot.onClick.AddListener(() => { HeavyShotToggle(); });
        m_GameOverGroup.gameObject.SetActive(false);

    }
    Coroutine m_CurGameOver;
    public void HeavyShot(bool enable)
    {
        var cur = GameManager.Instance.mergeController.GetCurrent();
        if (enable)
        {
            SetVFX(VFX.HeavyShot, cur.transform);
            cur.GetRb().mass += 100;
            m_heavyShotEnabled = true;
        }
        else
        {
            if (!GetHoldVFX(VFX.HeavyShot)) return;
            SetVFX(VFX.HeavyShot, cur?.transform,false);
            m_heavyShotEnabled = false;
        }
    }
    public void HeavyShotToggle()
    {
        bool find = false;
        foreach(var cur in m_heavyShotRemain)
        {
            if (cur.gameObject.activeSelf) find = true;
        }
        if (!find) { return; }//하나라도 있거나 켜져있는상태면 통과
        if (GetHoldVFX(VFX.HeavyShot)!=null&&GetHoldVFX(VFX.HeavyShot).gameObject.activeSelf)
        {
            HeavyShot(false);
            for (int i = 0; i < m_heavyShotRemain.Length; i++)
            {
                if (m_heavyShotRemain[i].gameObject.activeSelf) continue;
                m_heavyShotRemain[i].gameObject.SetActive(true);
            }
        }
        else
        {
            MergeSender cu = GameManager.Instance.mergeController.GetCurrent();
            if (cu == null) { return; }
            HeavyShot(true);
            for (int i = 0; i < m_heavyShotRemain.Length; i++)
            {
                if (m_heavyShotRemain[i].gameObject.activeSelf)
                {
                    m_heavyShotRemain[i].gameObject.SetActive(false);
                    break;
                }
            }

        }
    }
    public void GameOver()
    {
        print("gameover");
        //PauseGame(true);
        if(m_CurGameOver==null)m_CurGameOver=StartCoroutine(Cor_GameOver());
    }
    public void Restart()
    {
        m_GameOverGroup.gameObject.SetActive(false);
        m_score = 0;
        GameStart();
    }
    public void GameStart()
    {
        CleanCloneGroup();
        GameStarted?.Invoke();
        PauseGame(false);
        GameManager.Instance.mergeController.GetBall(UnityEngine.Random.Range(0, 5));
        ShowScore();
    }
    public void CleanCloneGroup()
    {
        for (int i = 0; i<m_cloneGroup.childCount;i++)
        {
            m_cloneGroup.GetChild(i).gameObject.SetActive(false);
            Destroy(m_cloneGroup.GetChild(i).gameObject,0.1f);
        }
        GameManager.Instance.resourceController.GetBallList().Clear();
    }
    IEnumerator Cor_GameOver()
    {
        GameManager.isGameRunning = false;
        List<MergeSender> balls=GameManager.Instance.resourceController.GetBallList();
        foreach (MergeSender b in balls)
        {
            b.GetCol().enabled = false;
            Destroy(b.GetRb());
        }
        foreach (MergeSender b in balls)
        {
            yield return new WaitForSeconds(0.3f);
            b.gameObject.SetActive(false);
            Transform tp=SetVFX(VFX.Poof, b.transform.position,b.transform.parent,isMulti:true);
            GameManager.Instance.soundController.OneShot(SoundController.Audio.poof);
            tp.localScale /= 2;
            m_score += b.Getlv() * 20;
            ShowScore();
        }
        foreach(MergeSender b in balls)
        {
            Destroy(b,1f);
        }
        yield return new WaitForSeconds(1);
        m_GameOverGroup.gameObject.SetActive(true);
        m_Score.text = m_score.ToString();
        m_CurGameOver = null;
    }
    public void PauseGame(bool boolean)
    {
        if (boolean)
        {
            Time.timeScale = 0;
            GameManager.isGameRunning = false;
        }
        else
        {
            Time.timeScale = 1;
            GameManager.isGameRunning = true;
        }
    }
    int m_activatedPage=1;
    public void StartGuideline()
    {
#if UNITY_EDITOR
        PlayerPrefs.SetInt("showGuideline", 1);
#endif
        m_activatedPage = 1;
        if(!PlayerPrefs.HasKey("showGuideline")) PlayerPrefs.SetInt("showGuideline", 1);
        int check = PlayerPrefs.GetInt("showGuideline");
        if (check == 0) { m_Guidelines[0]?.SetActive(false); Restart(); }
        else 
        {
            m_Guidelines[0]?.SetActive(true);
            m_Guidelines[1]?.SetActive(true);
            PauseGame(true);
        } 
    }
    public void GetNextGuidePage()
    {
        if (++m_activatedPage == m_Guidelines.Length)
        {
            m_Guidelines[0]?.SetActive(false);
            Restart();
        }
        for (int i = 1; i < m_Guidelines.Length; i++)
        {
            if (m_activatedPage == i)
                m_Guidelines[m_activatedPage]?.SetActive(true);
            else m_Guidelines[i]?.SetActive(false);
        }
    }
    void Update()
    {
        //int y=0;
        //int n=0;
        //foreach(var item in EventManager.col2ms.Values)
        //{
        //    if (!item.GetRb().IsSleeping())
        //    {
        //        //m_endline.gameObject.SetActive(false);
        //        //return;
        //        y++;
        //    }
        //    else
        //    {
        //        n++;
        //    }
        //}
        //print("wake: " + y +"/"+ "sleep: " + n);
        //if (EventManager.col2ms.Count > 0)
        //    m_endline.gameObject.SetActive(true);
        //if (GameManager.Instance.eventManager.GetColCount() > 0 && !m_endline.gameObject.activeSelf)
        //{
        //    m_endline.gameObject.SetActive(true);
        //}
        //else if (GameManager.Instance.eventManager.GetColCount() == 0 && m_endline.gameObject.activeSelf)
        //{
        //    m_endline.gameObject.SetActive(false);
        //}
        if (!GameManager.isGameRunning) return;
        if (GameManager.Instance.eventManager.GetTime() > 5)
        {
            GameOver();
        }
        else if (GameManager.Instance.eventManager.GetTime() > 1 && !m_endline.gameObject.activeSelf)
        {
            m_endline.gameObject.SetActive(true);
        }
        else if (GameManager.Instance.eventManager.GetTime() < 1&&m_endline.gameObject.activeSelf)
        {
            m_endline.gameObject.SetActive(false);
        }
        else { 
        }
    }

}
