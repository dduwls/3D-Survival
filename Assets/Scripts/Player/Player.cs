using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // PlayerController 컴포넌트를 참조하기 위한 변수
    // 이 변수를 통해 Player 스크립트에서 PlayerController의 기능을 사용할 수 있음.
    public PlayerController controller;
    public PlayerCondition condition;

    private void Awake()
    {
        // CharacterManager의 싱글톤 인스턴스를 통해 현재 Player 객체를 등록
        // 이를 통해 게임 내에서 CharacterManager.Instance.Player를 통해 플레이어에 접근할 수 있음
        CharacterManager.Instance.Player = this; // Player 스크립트 말함

        // 현재 GameObject에 부착된 PlayerController 컴포넌트를 가져와 저장
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
