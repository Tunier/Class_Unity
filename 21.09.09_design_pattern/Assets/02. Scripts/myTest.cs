using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class myTest : MonoBehaviour
{

    void Start()
    {
        mySingle.Instance.i = 100;
        mySingle.Instance.i2 = 200;
        mySingle.Instance.Print();
        // 클래스객체를 저장할 변수를 별도로 만들지 않았음에도
        // 클래스의 변수와 함수에 모두 접근이 가능하다.
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            var bullet = Pooling.Instance.GetBullet();

            bullet.transform.SetParent(gameObject.transform);
            
            bullet.transform.position = transform.position + new Vector3(0, 0, 1);

            bullet.transform.forward = transform.forward;
        }
    }
}
