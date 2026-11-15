using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    private readonly Dictionary<ShapeType, List<bool>> _shapeList = new()
    {
        {
            ShapeType.Straight,
            new()
            {
                false, true, false,
                false, true, false,
                false, true, false ,
            }
        },
        {
            ShapeType.T,
            new()
            {
                true,  true, true,
                false, true, false,
                false, true, false,
            }
        },
        {
            ShapeType.L,
            new()
            {
                true, false, false,
                true, false, false,
                true, true,  false,
            }
        },
        {
            ShapeType.L_Turn,
            new()
            {
                false, false, true,
                false, false, true,
                false, true,  true,
            }
        },
        {
            ShapeType.Z,
            new()
            {
                true,  true,  false,
                false, true,  true,
                false, false, false,
            }
        },
        {
            ShapeType.Z_Turn,
            new()
            {
                false, true,  true ,
                true,  true,  false,
                false, false, false,
            }
        },
    };


    //TODO: 動的なブロック形状生成を実装時にアクティブ
    //public Shape CreateShape(ShapeType type)
    //{
    //    GameObject shape = new("Shape");
    //    shape.AddComponent<Shape>();

    //    List<Block> blocks = _shapeList[type].Select((block, index) =>
    //    {
    //        return BlockCreator.Instance.CreateBlock(
    //            type: block ? BlockType.Normal : BlockType.None,
    //            shapeParent: shape.transform);
    //    }).ToList();

    //    return new(blocks: blocks);
    //}
}
