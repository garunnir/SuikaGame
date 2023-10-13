using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ResourceController : MonoBehaviour
{
    [SerializeField] MergeSender[] m_balls;
    MergeSender[] m_holdballs;
    List<MergeSender> m_CurBallList=new List<MergeSender>();
    public List<MergeSender> GetBallList() => m_CurBallList;
    public UnityAction IsLoadDone;
    public Texture2D[] m_textures;
    private void Start()
    {
        CreateBalls();
        if(!File.Exists(Application.persistentDataPath+"/0.png"))
        SaveTextrues();
        else
        {
            GetTextures();
            SetTextures();
            IsLoadDone?.Invoke();
        }

    }
    private void CreateBalls()
    {
        m_holdballs=new MergeSender[m_balls.Length];
        for (int i = 0; i < m_balls.Length; i++)
        {
            m_balls[i].Setlv(i);
            m_holdballs[i] = Instantiate(m_balls[i]);
            m_holdballs[i].gameObject.SetActive(false);
        }
    }
    public MergeSender GetBallRaw(int idx)
    {
        return m_holdballs[idx];
    }
    public MergeSender GetBall(int idx)
    {
        MergeSender mobj= Instantiate(m_holdballs[idx]);
        m_CurBallList.Add(mobj);
        mobj.gameObject.SetActive(true);
        mobj.transform.SetParent(GameManager.Instance.gameController.GetCloneParent());
        //mobj.GetMat().mainTexture = new Texture2D(1,1);
        return mobj;
    }
    public int GetBallCount()
    {
        return m_holdballs.Length;
    }

    public void GetTextures()
    {
        //������ �ؽ���
        //�ȵ���̵忡�� �Ⱦ��� �ؽ��ĸ� ����
        //�ش� ��Ʈ���� �ε����ؽ��ĸ� ����
        m_textures = new Texture2D[m_holdballs.Length];
        for (int i = 0;i < m_holdballs.Length; i++)
        {
            if (!File.Exists(Application.persistentDataPath + "/" + i + ".png")) continue;
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + i + ".png");
            Texture2D tmpt =new Texture2D(1,1);
            tmpt.LoadImage(bytes);
            m_textures[i] = tmpt;
        }
    }
    public void SetTextures()
    {
        for (int i = 0; i < m_holdballs.Length; i++)
        {
            if (m_textures[i] != null)
            m_holdballs[i].GetMat().mainTexture = m_textures[i];
        }
    }
    public void SaveTextrues()
    {
        #if !UNITY_EDITOR
        if (NativeGallery.CanSelectMultipleFilesFromGallery())
        {
            NativeGallery.GetImagesFromGallery((x) => {
                GetTex(x);
            }, "�ؽ��ĵ��� ������ 10�� �̻�!", "image/*");
        }
        else
        {
            print("�Ƴ� �����Ұ� ���µ���?");
        }
        void GetTex(string[] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                string img = x[i];
                File.Copy(img, Application.persistentDataPath + "/" + i + ".png", true);
                GetTextures();
                SetTextures();
                GameManager.Instance.gameController.Restart();
            }
        }

#endif
    }
    
}
