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
        // ������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ� ����.
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        // ���� �������� ������ ���۷� üũ.
        print(PhotonNetwork.SendRate);
        // ���� ���� ����.
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("�����Ϳ��� ��������.");
        print($"PhotonNetwork.InLoby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    /// </summary>
    public override void OnJoinedLobby()
    {
        print($"PhotonNetwork.InLoby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// �����忡 ����������� ȣ��Ǵ� �ݹ��Լ�.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print($"������ ����. {returnCode} : {message}");

        // ���� ������ �� ���� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4; // �� �ִ� ������ ��.
        ro.IsOpen = true; // ����/����� ����.
        ro.IsVisible = true; // �κ񿡼� �� ����� ���� �� ���� ������.

        PhotonNetwork.CreateRoom("Tunier's Room", ro);
    }

    /// <summary>
    /// �� ������ �Ϸ�Ǹ� ȣ��Ǵ� �ݹ� �Լ�
    /// </summary>
    public override void OnCreatedRoom()
    {
        print($"{PhotonNetwork.CurrentRoom.Name} �̶�� �� ����.");
    }

    /// <summary>
    /// �濡 ������ �Ŀ� ȣ��Ǵ� �ݹ� �Լ�
    /// </summary>
    public override void OnJoinedRoom()
    {
        print($"�� ���� ���� = {PhotonNetwork.InRoom}");
        print($"���� ���� �� = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            print($"������ ���� �̸� : {player.Value.NickName}");
        }
        
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation);
    }
}