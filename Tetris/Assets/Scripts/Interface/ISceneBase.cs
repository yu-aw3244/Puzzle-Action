
namespace Scene 
{
    /// <summary>
    /// シーン基底クラスのインターフェース
    /// </summary>
    public interface ISceneBase
    {
        /// <summary>
        /// シーンセットアップ処理
        /// </summary>
        public void SetUp();
        /// <summary>
        /// シーン削除処理
        /// </summary>
        public void Destroy();
        /// <summary>
        /// シーン繊維処理
        /// </summary>
        public void ChangeScene();
    }
}
