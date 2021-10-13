using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour
{
    GameObject player;

    public List<GameObject> wayPoints = new List<GameObject>();


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoint"));
    }

    public void OnClickHuman()
    {
        StartFade(0);
    }

    public void OnClickElven()
    {
        StartFade(1);
    }

    public void OnClickGoblin()
    {
        StartFade(2);
    }

    public void OnClickUndead()
    {
        StartFade(3);
    }

    public void OnClickExit()
    {
        GameManager.Instance.wayPointUI.SetActive(false);
    }

    public void moveWaypoint(int _waynumber)
    {
        //플레이어가 서있는 웨이포인트와 이동하려는 웨이포인트 거리가 일정이상 멀어야 작동
        //플레이어가 서있는 웨이포인트가 가까우면 같은 포인트라고 인식하도록
        if (Vector3.Distance(player.transform.position, wayPoints[_waynumber].transform.position) > UIManager.Instance.recognitionRange)
        {
            player.transform.position = wayPoints[_waynumber].transform.position;
        }

    }

    public void StartFade(int _waynumber)
    {
        if (Vector3.Distance(player.transform.position, wayPoints[_waynumber].transform.position) > UIManager.Instance.recognitionRange)
        {
            StartCoroutine(UIManager.Instance.FadeCoroutine(0.5f, _waynumber));
        }
    }
}
