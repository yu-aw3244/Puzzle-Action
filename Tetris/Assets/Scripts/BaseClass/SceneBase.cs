using Scene;
using UnityEngine;

/// <summary>
/// シーン基底クラス
/// </summary>
public class SceneBase : MonoBehaviour, ISceneBase
{
    public virtual void SetUp() { }

    public virtual void Destroy() { }

    public virtual void ChangeScene() { }
}
