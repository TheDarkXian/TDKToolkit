using System.Net.NetworkInformation;
using UnityEngine;
namespace TDKToolkit
{

    public class TDKHierarchy : Singleton<TDKHierarchy>
    {

        public static Transform UiNode
        {
            get
            {
                if (uinode == null)
                {
                    uinode = TDKToolkitPag.FindTransform("[UI]");
                }
                return uinode;
            }
        }
        static Transform uinode;

        public static Transform SenceNode
        {
            get
            {
                if (sencenode == null)
                {
                    sencenode = TDKToolkitPag.FindTransform("[SenceObject]");
                }
                return sencenode;
            }
        }
        static Transform sencenode;

        public static Transform GameNode
        {
            get
            {
                if (gamenode == null)
                {
                    gamenode = TDKToolkitPag.FindTransform("[GameObject]");
                    if (gamenode != null)
                    {
                        gamenode.transform.position = Vector3.zero;
                        gamenode.transform.rotation = Quaternion.identity;

                    }
                }

                return gamenode;
            }
        }
        static Transform gamenode;

        public static Transform MangerNode
        {
            get
            {
                if (mangernode == null)
                {
                    mangernode = TDKToolkitPag.FindTransform("[Manger]");
                }
                return mangernode;
            }
        }
        static Transform mangernode;

        public static Transform EffectNode
        {
            get
            {
                if (effectnode == null)
                {
                    effectnode = TDKToolkitPag.FindTransform("[Effect]");
                }
                return effectnode;

            }
        }
        static Transform effectnode;

        public static Transform EnvNode
        {
            get
            {
                if (envnode == null)
                {
                    envnode = TDKToolkitPag.FindTransform("[Env]");
                }
                return envnode;
            }
        }
        static Transform envnode;


    }


}
