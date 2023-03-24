using UnityEngine;
using Photon.Pun;

namespace KID
{
    /// <summary>
    /// 場景控制：管理玩家進入後物件的生成
    /// </summary>
    public class SceneControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("玩家預製物")]
        private GameObject prefabPlayer;

        private void Awake()
        {
            InitializePlayer();
        }

        /// <summary>
        /// 初始化玩家
        /// </summary>
        private void InitializePlayer()
        {
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(-5f, 5f);
            pos.y = 3f;
            PhotonNetwork.Instantiate(prefabPlayer.name, pos, Quaternion.identity);
        }
    }
}
