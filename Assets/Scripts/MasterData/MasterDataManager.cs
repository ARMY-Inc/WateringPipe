using UnityEngine;

namespace ARMY.MasterData
{
    public class MasterDataManager
    {
        private static MasterDataManager instance;
        public static MasterDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MasterDataManager();
                    instance.Init();
                }
                return instance;
            }
        }

        public MasterDataSO Data { get; private set; } = null;


        public void Init()
        {
            var origin = Resources.Load<MasterDataSO>("MasterData/MainMasterData");
            Data = ScriptableObject.Instantiate<MasterDataSO>(origin);
            Data.Init();
        }
    }
}
