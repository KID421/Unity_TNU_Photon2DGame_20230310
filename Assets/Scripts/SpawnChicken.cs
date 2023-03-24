using UnityEngine;
using Photon.Pun;

namespace KID
{
    /// <summary>
    /// �ͦ��N��
    /// </summary>
    public class SpawnChicken : MonoBehaviour
    {
        [SerializeField, Header("�N��")]
        private GameObject prefabChicken;
        [SerializeField, Header("�ͦ��W�v"), Range(0, 5)]
        private float intervalSpawn = 2.5f;
        [SerializeField, Header("�ͦ��I")]
        private Transform[] spawnPoints;

        private void Awake()
        {
            // �p�G �O �@���D�����Ȥ� �~����ͦ�
            if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating("Spawn", 0, intervalSpawn);
            }
        }

        private void Spawn()
        {
            int random = Random.Range(0, spawnPoints.Length);
            PhotonNetwork.Instantiate(prefabChicken.name, spawnPoints[random].position, Quaternion.identity);
        }
    }
}
