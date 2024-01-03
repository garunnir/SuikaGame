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
    //데이터 상태를 연결짓는다.
    Toggle target;
    //포인터
    //여기에서 다 해결 할 수는 없는듯함.
    //다른 스크립트에서 특정 변수를 가져온다.

    //특정 불 프로퍼티의 상태가 변하면 그 상태를 ison 적용하게 하고싶다.
    private void Awake()
    {
        bool onGyro = Input.gyro.enabled;
        target = GetComponent<Toggle>();
        target.isOn= onGyro;
    }
}
