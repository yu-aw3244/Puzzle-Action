using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    public enum BlockShape
    {
        I_Shape,
        T_Shape,
        Z_Shape,
        Turn_Z_Shape,
    }

    [Serializable]
    public class BlockData
    {
        [SerializeField]
        private BlockShape _shapeType;
        [SerializeField]
        private GameObject _prefab;

        public BlockShape ShapeType => _shapeType;
        public GameObject Prefab => _prefab;
    }

    [SerializeField]
    private List<GameObject> _sponePositionList;
    [SerializeField]
    private List<BlockData> _blockShapeList;
    [SerializeField]
    private GameObject _blockParent;
    [SerializeField]
    private Transform _basePoint;
    [SerializeField]
    private float _sponeInterval = 5.0f;

    /// <summary>
    /// スポーン処理の経過時間を秒単位で保持するタイマー。
    /// </summary>
    private float _sponeTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sponeTimer = _sponeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        CreateShapeBlock();
    }

    private void CreateShapeBlock()
    {
        _sponeTimer -= Time.deltaTime;

        if (_sponeTimer > 0.0f)
            return;

        int shapeNum = UnityEngine.Random.Range(0, Enum.GetValues(typeof(BlockShape)).Length);
        if (Enum.IsDefined(typeof(BlockShape), shapeNum))
        {
            CreateShapeBlock((BlockShape)shapeNum);
        }
        else
        {
            Debug.Log($"{nameof(BlockFactory)} 未定義の値: {shapeNum}");
        }
        _sponeTimer = _sponeInterval;
    }

    /// <summary>
    /// 指定された形状のブロックを作成して初期化する。
    /// </summary>
    /// <param name="shapeType">作成するブロックの形状を指定する BlockShape 値。</param>
    private void CreateShapeBlock(BlockShape shapeType)
    {
        BlockData blockData = _blockShapeList.Find(x => x != null && x.ShapeType == shapeType);

        if (blockData == null)
        {
            Debug.LogError($"形状 {shapeType} のBlockDataが登録されていません。", this);
            return;
        }

        if (blockData.Prefab == null)
        {
            Debug.LogError($"形状 {shapeType} のPrefabが未設定、または破棄されています。"
                + "Project内のPrefabアセットを設定してください。",this);
            return;
        }

        if (_sponePositionList == null || _sponePositionList.Count == 0)
        {
            Debug.LogError("スポーン位置が登録されていません。", this);
            return;
        }

        GameObject spawnPoint = _sponePositionList[UnityEngine.Random.Range(0, _sponePositionList.Count)];

        if (spawnPoint == null || _basePoint == null || _blockParent == null)
        {
            Debug.LogError(
                "スポーン位置・基準点・ブロックの親のいずれかが未設定、または破棄されています。",
                this);
            return;
        }
        ;
        Vector3 spawnPosition = spawnPoint.transform.position;
        Vector3 direction = _basePoint.position - spawnPosition;

        var spawnObject = Instantiate(
                            blockData.Prefab,
                            spawnPosition,
                            Quaternion.LookRotation(direction.normalized),
                            _blockParent.transform
                            );

        spawnObject.GetComponent<Shape>().Setup();
    }
}
