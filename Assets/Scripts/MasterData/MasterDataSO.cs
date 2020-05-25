using UnityEngine;

namespace ARMY.MasterData
{
    [CreateAssetMenu(menuName = "ARMY/Create MasterData")]
    public class MasterDataSO : ScriptableObject
    {
        [Tooltip("How fast the pipes rotate (Default 180.0f")] public float pipeRotateSpeed = 180.0f;
        [Range(0,3)]
        [Tooltip("How we wait until the pipe turn blue (Default 1.0f")] public float pipeWaterSpeed = 1.0f;

        public void Init()
        {
        }
    }
}