using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI textRoomName;
    public TextMeshProUGUI textRoomPlayer;
    public GameObject goRoomGroup;

    private string roomNameCreate;
    private string roomNameJoin;

    private void Awake()
    {
        // 連到 Photon 伺服器
        PhotonNetwork.ConnectUsingSettings();
    }

    public void InputFieldPlayerName(string value)
    {
        print("玩家名稱：" + value);

        // 將輸入名稱儲存到 Photon 伺服器
        PhotonNetwork.NickName = value;
    }

    public void InputFieldCreateRoomName(string value)
    {
        print("建立房間名稱：" + value);

        // 儲存建立房間名稱
        roomNameCreate = value;
    }

    public void InputFieldJoinRoomName(string value)
    {
        print("加入房間名稱：" + value);

        // 儲存加入房間名稱
        roomNameJoin = value;
    }

    public void ButtonCreateRoom()
    {
        print("創建房間");

        // 創建房間並決定房間人數
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(roomNameCreate, ro);
    }

    public void ButtonJoinRoom()
    {
        print("加入房間");

        PhotonNetwork.JoinRoom(roomNameJoin);
    }

    public void ButtonJoinRandomRoom()
    {
        print("加入隨機房間");

        PhotonNetwork.JoinRandomRoom();
    }

    // override 加上 空格 並選取 OnJoinRoom
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        print("成功加入房間");

        // 顯示房間並更新名稱與人數
        goRoomGroup.SetActive(true);
        textRoomName.text = PhotonNetwork.CurrentRoom.Name;
        textRoomPlayer.text = "房間人數：" + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
}
