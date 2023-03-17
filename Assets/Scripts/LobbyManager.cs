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
        // �s�� Photon ���A��
        PhotonNetwork.ConnectUsingSettings();
    }

    public void InputFieldPlayerName(string value)
    {
        print("���a�W�١G" + value);

        // �N��J�W���x�s�� Photon ���A��
        PhotonNetwork.NickName = value;
    }

    public void InputFieldCreateRoomName(string value)
    {
        print("�إߩж��W�١G" + value);

        // �x�s�إߩж��W��
        roomNameCreate = value;
    }

    public void InputFieldJoinRoomName(string value)
    {
        print("�[�J�ж��W�١G" + value);

        // �x�s�[�J�ж��W��
        roomNameJoin = value;
    }

    public void ButtonCreateRoom()
    {
        print("�Ыةж�");

        // �Ыةж��èM�w�ж��H��
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(roomNameCreate, ro);
    }

    public void ButtonJoinRoom()
    {
        print("�[�J�ж�");

        PhotonNetwork.JoinRoom(roomNameJoin);
    }

    public void ButtonJoinRandomRoom()
    {
        print("�[�J�H���ж�");

        PhotonNetwork.JoinRandomRoom();
    }

    // override �[�W �Ů� �ÿ�� OnJoinRoom
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        print("���\�[�J�ж�");

        // ��ܩж��ç�s�W�ٻP�H��
        goRoomGroup.SetActive(true);
        textRoomName.text = PhotonNetwork.CurrentRoom.Name;
        textRoomPlayer.text = "�ж��H�ơG" + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
}
