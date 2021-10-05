using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    readonly string version = "1.0";
    string userId = "Tunier";

    void Awake()
    {
        // 마스터 클라이언트의 씬 자동 동기화 옵션 설정.
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        // 포톤 서버와의 데이터 전송률 체크.
        print(PhotonNetwork.SendRate);
        // 포톤 서버 접속.
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("마스터에게 접속했음.");
        print($"PhotonNetwork.InLoby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// 로비에 접속 후 호출되는 콜백 함수
    /// </summary>
    public override void OnJoinedLobby()
    {
        print($"PhotonNetwork.InLoby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// 방입장에 실패했을경우 호출되는 콜백함수.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print($"방접속 실패. {returnCode} : {message}");

        // 새로 생성할 방 정보 설정
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4; // 방 최대 접속자 수.
        ro.IsOpen = true; // 공개/비공개 유무.
        ro.IsVisible = true; // 로비에서 룸 목록을 노출 할 건지 말건지.

        PhotonNetwork.CreateRoom("Tunier's Room", ro);
    }

    /// <summary>
    /// 방 생성이 완료되면 호출되는 콜백 함수
    /// </summary>
    public override void OnCreatedRoom()
    {
        print($"{PhotonNetwork.CurrentRoom.Name} 이라는 방 생성.");
    }

    /// <summary>
    /// 방에 입장한 후에 호출되는 콜백 함수
    /// </summary>
    public override void OnJoinedRoom()
    {
        print($"방 접속 유무 = {PhotonNetwork.InRoom}");
        print($"접속 유저 수 = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            print($"접속한 유저 이름 : {player.Value.NickName}");
        }
        
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation);
    }
}