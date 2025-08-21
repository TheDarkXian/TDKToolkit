using UnityEngine;
using UnityEngine.Playables;
namespace TDKToolkit
{

    public class TDKResourcesLoad : Singleton<TDKResourcesLoad>
    {

        public static TDKSetting tDKSetting
        {
            get
            {
                return Resources.Load<TDKSetting>("TDKDefualtSetting");
            }
        }
        public TDKResourcesLoad()
        {
        }

        public static GameObject LoadAPrefebs(string prefebName)
        {
            GameObject temp = Resources.Load<GameObject>(tDKSetting.PrefebsResources + "\\" + prefebName);
            return temp;
        }

    }



}

