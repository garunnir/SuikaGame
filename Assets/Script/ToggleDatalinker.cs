using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleDatalinker : MonoBehaviour
{
    //������ ���¸� �������´�.
    Toggle target;
    //������
    //���⿡�� �� �ذ� �� ���� ���µ���.
    //�ٸ� ��ũ��Ʈ���� Ư�� ������ �����´�.

    //Ư�� �� ������Ƽ�� ���°� ���ϸ� �� ���¸� ison �����ϰ� �ϰ�ʹ�.
    private void Awake()
    {
        bool onGyro = Input.gyro.enabled;
        target = GetComponent<Toggle>();
        target.isOn= onGyro;
    }
}
