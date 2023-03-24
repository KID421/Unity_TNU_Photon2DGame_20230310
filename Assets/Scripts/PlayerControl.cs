using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

namespace KID
{
    /// <summary>
    /// ���a���
    /// </summary>
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("���ʳt��"), Range(0, 10)]
        private float speed = 3.5f;
        [Header("�ˬd�a�O���")]
        [SerializeField] private Vector3 groundOffset;
        [SerializeField] private Vector3 groundSize;
        [SerializeField, Header("���D����"), Range(0, 1000)]
        private float jump = 30f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "�}������";
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

            // ���o�Ĥ@�Ӥl����
            childCanvas = transform.GetChild(0);

            // �p�G ���O�ۤv������ ��������
            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);

            textChicken = transform.Find("�e�����a�W��/�N���ƶq").GetComponent<TextMeshProUGUI>();
            groupGame = GameObject.Find("�e���C������").GetComponent<CanvasGroup>();
            textWinner = GameObject.Find("�ӧQ��").GetComponent<TextMeshProUGUI>();

            btnBackToLobby = GameObject.Find("��^�C���j�U").GetComponent<Button>();
            btnBackToLobby.onClick.AddListener(() =>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("�C���j�U");
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
            if (collision.name.Contains("�N��"))
            {
                Destroy(collision.gameObject);

                textChicken.text = (++countChicken).ToString();

                if (countChicken >= countChickenMax) Win();
            }
        }

        /// <summary>
        /// �^������W��
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
        /// ���
        /// </summary>
        private void Win()
        {
            groupGame.alpha = 1;
            groupGame.interactable = true;
            groupGame.blocksRaycasts = true;

            textWinner.text = "��Ӫ��a�G" + photonView.Owner.NickName;

            DestroyObject();
        }

        /// <summary>
        /// �R������
        /// </summary>
        private void DestroyObject()
        {
            GameObject[] chickens = GameObject.FindGameObjectsWithTag("�N��");

            for (int i = 0; i < chickens.Length; i++) Destroy(chickens[i]);

            Destroy(FindObjectOfType<SpawnChicken>().gameObject);
        }

        [PunRPC]
        private void RPCUpdateName()
        {
            transform.Find("�e�����a�W��/�W�٤���").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Move()
        {
            // A ���G-1
            // D ���G+1
            // �S���G0
            float h = Input.GetAxis("Horizontal");
            rig.velocity = new Vector2(speed * h, rig.velocity.y);
            ani.SetBool(parWalk, h != 0);

            if (Mathf.Abs(h) < 0.1f) return;
            transform.eulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
            childCanvas.localEulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
        }

        /// <summary>
        /// �ˬd�a�O
        /// </summary>
        private void CheckGround()
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffset, groundSize, 0);
            isGround = hit;
        }

        /// <summary>
        /// ���D
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
