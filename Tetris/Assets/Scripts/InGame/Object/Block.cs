using Unity.VisualScripting;
using UnityEngine;

public enum BlockType 
{
    None,
    Normal,
    swamp,
}

/// <summary>
/// 地形を構成するブロック
/// </summary>
public class Block : MonoBehaviour
{
    private Rigidbody _myShapeRigidbody = null;
    public Vector3 MoveDirection { get; private set; }
    public BlockType Type { get; private set; }
    public bool IsStopping { get; private set; }

    public void Setup(Rigidbody myShapeRigidbody, Vector3 moveDirection)
    {
        _myShapeRigidbody = myShapeRigidbody;
        MoveDirection = moveDirection;
    }

    public void OnCollisionEnter(Collision collision)
    {
        _myShapeRigidbody.isKinematic = false;
    }

    public virtual void Move() { }
    public virtual void OnDestroy() { }
}
