using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage; // 한 번에 줄 데미지량
    public float damageRate; // 데미지를 가하는 주기 (초 단위)

    // 데미지를 받을 수 있는 객체를 저장할 리스트
    List<IDamagalbe> things = new List<IDamagalbe>();

    void Start()
    {
        // 일정한 간격으로 DealDamage()를 호출하여 지속적인 데미지를 가함
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    // 리스트에 있는 모든 객체에게 데미지를 줌
    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage); // IDamagalbe 인터페이스를 통해 데미지 적용
        }
    }

    // 캠프파이어의 트리거 범위에 객체가 들어오면 실행됨
    void OnTriggerEnter(Collider other)
    {
        // IDamagalbe 인터페이스를 가진 객체인지 확인하고, 맞다면 리스트에 추가
        if (other.TryGetComponent(out IDamagalbe damagalbe))
        {
            things.Add(damagalbe);
        }
    }

    // 캠프파이어의 트리거 범위에서 객체가 나가면 실행됨
    private void OnTriggerExit(Collider other)
    {
        // IDamagalbe 인터페이스를 가진 객체인지 확인하고, 맞다면 리스트에서 제거
        if (other.TryGetComponent(out IDamagalbe damagable))
        {
            things.Remove(damagable);
        }
    }
}
