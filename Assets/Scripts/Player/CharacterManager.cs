using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // 싱글톤 인스턴스를 저장할 변수
    private static CharacterManager instance;

    // 외부에서 CharacterManager의 인스턴스에 접근할 수 있는 프로퍼티
    public static CharacterManager Instance
    {
        get
        {
            // instance가 null이면 새로운 GameObject를 생성하고 CharacterManager를 추가
            if (instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return instance;
        }
    }

    // 현재 관리 중인 Player 객체
    public Player player;

    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    public void Awake()
    {
        // 싱글톤 패턴 구현: instance가 비어있다면 현재 객체를 인스턴스로 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 삭제되지 않도록 설정
        }
        else
        {
            // 이미 존재하는 인스턴스가 있다면, 현재 객체를 파괴하여 중복 생성을 방지
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }  
    }
}
