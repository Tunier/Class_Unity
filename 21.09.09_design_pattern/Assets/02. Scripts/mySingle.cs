using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mySingle : MonoBehaviour
{
    private static mySingle instance = null;
    // 클래스 객체를 정적변수로 선언하고 null로 초기화한다.
    // 일반적으로 클래스 객체를 정적변수로 선언할때는 private로 선언한다.

    // 정적객체를 이용한 싱글톤을 구현
    // 싱글톤 : 프로그램이 동작하는 동안
    // 오직 하나만 존재해야하는 대상이 있을때
    // (씬이 전환되든, 다른 클래스에서 해당 대상을 사용해야 하든)
    // 모든 상황에서 공통된 하나만을 만들어서 사용하도록
    // 해주는 프로그램 구조(디자인 패턴)

    public int i = 0;
    public int i2 = 1;

    public static mySingle Instance
    {
        // 싱글톤으로 만든 클래스 객체 instance는
        // 클래스에 포함되어 있기 때문에
        // 사용하려면 클래스를 생성해야 한다.
        // 그런데 이미 생성된 클래스가 있으면
        // 중복생성을 막기위해 삭제하기 때문에
        // 포함된 instance를 사용할수 없다는 모순이 생긴다.
        // 때문에 이를 해결하기 위해서
        // 객체를 만들지 않아도 해당 변수에 접근할수 있도록
        // static으로 함수를 만든다.
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // static 변수가 null이라는 말은
            // 해당 함수를 들어온게 처음이란 뜻이기 때문에
            // static변수 instance에 자기 자신을 넣는다.
            // 다만 정적변수가 된건 클래스객체 뿐이기 때문에
            // 유니티 게임오브젝트는 씬이 넘어가면 삭제된다.
            // 그래서 클래스와 함께 게임오브젝트도 삭제되지 않도록
            // DonDestroyOnLoad에 해당 클래스 객체를 컴포넌트로 가지는
            // 게임오브젝트를 추가하여 삭제를 막는다.
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
            // 이미 instance가 존재한다면
            // 중복된 객체가 생성되지 않도록
            // 생성된 오브젝트를 삭제한다.
        }
    }

    public void Print()
    {
        print("i : " + i + "\n");
        print("i2 : " + i2 + "\n");
    }
}
