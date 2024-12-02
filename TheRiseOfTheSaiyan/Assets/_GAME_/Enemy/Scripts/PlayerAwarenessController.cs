using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }

    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField]
    private float _playerAwarenessDistance;

    private Transform _player;

    [SerializeField] private float _attackAwarenessDistance = 1.5f;
    public bool WithinAttackRange { get; private set; }

    public Transform PlayerTransform => _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player_Controller>().transform;
    }

    void Update()
    {
        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;
        float distance = enemyToPlayerVector.magnitude;

        AwareOfPlayer = distance <= _playerAwarenessDistance;
        WithinAttackRange = distance <= _attackAwarenessDistance;
    }
}
