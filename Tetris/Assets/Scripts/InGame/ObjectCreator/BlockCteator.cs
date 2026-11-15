using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlockCreator 
{
    private static BlockCreator instance;
    public static BlockCreator Instance => instance ??= new();
    private static string _blockPrefabPath = "Assets/Resource/Example/Prefab/Block.prefab";

    public Block CreateBlock(BlockType type, Transform shapeParent)
    {
        Block block = Resources.Load<Block>(_blockPrefabPath);
        block.transform.parent = shapeParent;

        if (block == null)
        {

            return null;
        }

        return block;
    }
}
