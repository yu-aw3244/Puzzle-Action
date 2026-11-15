using System.Collections.Generic;
using UnityEngine;

public enum ShapeType
{
    Straight,
    T,
    L,
    L_Turn,
    Z,
    Z_Turn,
}

/// <summary>
/// 地形クラス
/// 地形を構成するブロックの管理を行う
/// </summary>
public class Shape : MonoBehaviour
{
    [SerializeField]
    private List<Block> _blockList = new();
    [SerializeField]
    private Rigidbody _rigidbody = null;

    private readonly float _movePower = 100.0f;

    public void Setup()
    {
        Move();
    }

    private void Move()
    {
        _rigidbody.AddForce(transform.forward * _movePower);
    }
}
