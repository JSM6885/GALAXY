using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 흔드는 스크립트

public class Camera_Shake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)//진동 시간과 진동의 세기를 받아서 실행
    {
        Vector3 originalPos = transform.localPosition;//원래 위치를 저장
        float elapsed = 0.0f;//시간을 비교할 변수

        while(elapsed < duration)//진동시간이 다 되기전 까지
        {
            float x = Random.Range(-1f, 1f) * magnitude;//랜덤하게 
            float y = Random.Range(-1f, 1f) * magnitude;//좌표를 변경

            transform.localPosition = new Vector3(x, y, originalPos.z);//다시 원래 위치로 되돌림

            elapsed += Time.deltaTime;//시간을 체크

            yield return null;
        }
        transform.localPosition = originalPos;//다시 원래 위치로 복귀
    }

}
