using UnityEngine;
using Photon.Pun;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private float _minX, _minY, _minZ, _maxX, _maxY, _maxZ;

    private void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Vector3 randomPosition = new Vector3(Random.Range(_minX, _minY), Random.Range(_maxX, _maxY), Random.Range(_minZ, _maxZ));
        PhotonNetwork.Instantiate(_playerPrefab.name, randomPosition, Quaternion.identity);
    }
}