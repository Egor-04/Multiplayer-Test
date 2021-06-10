using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    [Header("Spawn At Start")]
    [Header("Spawn Points")]
    [SerializeField] private Transform[] _startSpawnPoints;

    [Header("Objects")]
    [SerializeField] private GameObject[] _startObjects;


    [Header("Spawn All Time")]
    [Header("Spawn Points")]
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Objects")]
    [SerializeField] private GameObject[] _objects;
    
    [Header("Timer")]
    [SerializeField] private float _timeIntervalToSpawn;
    private float _currentTime;

    private void Start()
    {
        SpawnAtStart();
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;
        CheckTime();
        SpawnAllTime();
    }

    private void SpawnAtStart()
    {
        for (int i = 0; i < _startSpawnPoints.Length; i++)
        {
            PhotonNetwork.Instantiate(_startObjects[i].name, _startSpawnPoints[i].position, Quaternion.identity);
        }
    }

    private void SpawnAllTime()
    {
        if (_currentTime <= 0f)
        {
            PhotonNetwork.Instantiate(_objects[Random.Range(0, _objects.Length)].name, _spawnPoints[Random.Range(0, _spawnPoints.Length)].position, Quaternion.identity);
            _currentTime += _timeIntervalToSpawn;
        }
    }

    private void CheckTime()
    {
        if (_currentTime <= 0f)
        {
            _currentTime = 0f;
        }
    }
}
