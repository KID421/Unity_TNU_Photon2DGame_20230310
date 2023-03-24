using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

namespace KID
{
    /// <summary>
    /// 玩家控制器
    /// </summary>
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("移動速度"), Range(0, 10)]
        private float speed = 3.5f;
        [Header("檢查地板資料")]
        [SerializeField] private Vector3 groundOffset;
        [SerializeField] private Vector3 groundSize;
        [SerializeField, Header("跳躍高度"), Range(0, 1000)]
        private float jump = 30f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "開關走路";
        private bool isGround;
        private Transform childCanvas;
        private TextMeshProUGUI textChicken;
        private int countChicken;
        private int countChickenMax = 10;
        private CanvasGroup groupGame;
        private TextMeshProUGUI textWinner;
        private Button btnBackToLobby;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position + groundOffset, groundSize);
        }

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();

            // 取得第一個子物件
            childCanvas = transform.GetChild(0);

            // 如果 不是自己的物件 關閉元件
            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);

            textChicken = transform.Find("畫布玩家名稱/烤雞數量").GetComponent<TextMeshProUGUI>();
            groupGame = GameObject.Find("畫布遊戲介面").GetComponent<CanvasGroup>();
            textWinner = GameObject.Find("勝利者").GetComponent<TextMeshProUGUI>();

            btnBackToLobby = GameObject.Find("返回遊戲大廳").GetComponent<Button>();
            btnBackToLobby.onClick.AddListener(() =>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("遊戲大廳");
                }
            });
        }

        private void Start()
        {
            GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

        private void Update()
        {
            CheckGround();
            Move();
            Jump();
            BackToTop();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains("烤雞"))
            {
                Destroy(collision.gameObject);

                textChicken.text = (++countChicken).ToString();

                if (countChicken >= countChickenMax) Win();
            }
        }

        /// <summary>
        /// 回到場景上方
        /// </summary>
        private void BackToTop()
        {
            if (transform.position.y < -20)
            {
                rig.velocity = Vector3.zero;
                transform.position = new Vector3(1.5f, 15, 0);
            }
        }

        /// <summary>
        /// 獲勝
        /// </summary>
        private void Win()
        {
            groupGame.alpha = 1;
            groupGame.interactable = true;
            groupGame.blocksRaycasts = true;

            textWinner.text = "獲勝玩家：" + photonView.Owner.NickName;

            DestroyObject();
        }

        /// <summary>
        /// 刪除物件
        /// </summary>
        private void DestroyObject()
        {
            GameObject[] chickens = GameObject.FindGameObjectsWithTag("烤雞");

            for (int i = 0; i < chickens.Length; i++) Destroy(chickens[i]);

            Destroy(FindObjectOfType<SpawnChicken>().gameObject);
        }

        [PunRPC]
        private void RPCUpdateName()
        {
            transform.Find("畫布玩家名稱/名稱介面").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void Move()
        {
            // A ←：-1
            // D →：+1
            // 沒按：0
            float h = Input.GetAxis("Horizontal");
            rig.velocity = new Vector2(speed * h, rig.velocity.y);
            ani.SetBool(parWalk, h != 0);

            if (Mathf.Abs(h) < 0.1f) return;
            transform.eulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
            childCanvas.localEulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
        }

        /// <summary>
        /// 檢查地板
        /// </summary>
        private void CheckGround()
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffset, groundSize, 0);
            isGround = hit;
        }

        /// <summary>
        /// 跳躍
        /// </summary>
        private void Jump()
        {
            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                rig.AddForce(new Vector2(0, jump));
            }
        }
    }
}
