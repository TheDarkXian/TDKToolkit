using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TDKToolkit.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDKToolkit
{
    public class TDKApplication : SingletonMono<TDKApplication>
    {
        private void OnValidate()
        {
#if UNITY_EDITOR
            this.gameObject.name = typeof(TDKApplication).Name;
            if (tdkSetting == null)
            {
                tdkSetting = Resources.Load<TDKSetting>("TDKDefualtSetting");
                if (tdkSetting == null)
                {
                    tdkSetting = ScriptableObject.CreateInstance<TDKSetting>();

                    // 确保 Resources 文件夹存在
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    // 保存新的 TDKSetting 实例到 Resources 文件夹
                    AssetDatabase.CreateAsset(tdkSetting, "Assets/Resources/TDKSetting.asset");
                    AssetDatabase.SaveAssets();

                    Debug.Log("新的 TDKSetting 资源已创建并保存到 Resources 文件夹。");
                }

            }
#endif


        }
        public TDKSetting tdkSetting;

        private void Awake()
        {
            if (Instance != null && Instance == this)
            {
                this.gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);

            }
            this.transform.parent = TDKHierarchy.MangerNode;
        }
        #region 加载场景
        [SerializeField]
        LoadSenceSetting loadSenceSetting;

        public bool enableApplicationProgress
        {
            set { loadSenceSetting.enableApplicationProgress = value; }
            get => loadSenceSetting.enableApplicationProgress;
        }
        public float applicationProgress
        {
            set => loadSenceSetting.applicationProgress = value;
            get => loadSenceSetting.applicationProgress;
        }
        public void ReBorn()
        {

            string thisSence = SceneManager.GetActiveScene().name;
            LoadSenceAscy(thisSence, loadSenceSetting.strTips, loadSenceSetting.loadImg);

        }
        [Button]
        public void LoadSence(string target)
        {

            SceneManager.LoadScene(target);


        }
        public void LoadSenceAscy(string targetSence, string tips, Sprite loadImg)
        {
            if (loadImg != null)
            {
                loadSenceSetting.loadImg = loadImg;

            }
            loadSenceSetting.targetName = targetSence;
            LoadSenceAscy(tips);
        }
        public void LoadSenceAscy(LoadSenctSettingData loadSenctSettingData)
        {
            LoadSenceAscy(loadSenctSettingData.targetName, loadSenctSettingData.tips, loadSenctSettingData.loadsprite);
        }
        public void LoadSenceAscy(string tips = "")
        {
            loadSenceSetting.strTips = tips;
            StartCoroutine(loadSenceSetting.IEloadSenceAscy());
        }
        [Button("跳转")]
        void JumpSence(string strips)
        {
            LoadSenceAscy(strips);
        }
        [System.Serializable]
        public class LoadSenceSetting
        {
            bool isonloding;
            [ValueDropdown("buildSenceName")]
            public string targetName;
            List<string> buildSenceName { get { return TDKToolkitPag.GetBuildSceneList(); } }
            public bool enableApplicationProgress;
            [ShowIfGroup("enableApplicationProgress")]
            public float applicationProgress;
            public string strTips;
            public Sprite loadImg;
            internal IEnumerator IEloadSenceAscy()
            {
                if (isonloding == true) { yield break; }
                isonloding = true;
                AsyncOperation op = SceneManager.LoadSceneAsync(targetName);
                float loadprocess = 0.0f;
                op.allowSceneActivation = false;

                UILoadScene uILoadScene = null;
                try
                {
                    uILoadScene = UIManger.GetUiPage<UILoadScene>();

                }
                catch
                {
                }

                if (strTips == "")
                {
                    strTips = "正在前往 " + targetName + "... ";
                }
                if (loadImg != null)
                {
                    uILoadScene?.SetLoadImg(loadImg);

                }

                uILoadScene?.ShowPage();
                uILoadScene?.SetTips(strTips);
                uILoadScene?.SetProcessValue(loadprocess);


                if (uILoadScene)
                {

                    yield return new WaitForSeconds(uILoadScene.loadEntryTime);
                }

                while (true)
                {
                    if (loadprocess >= 1.0f)
                    {
                        loadprocess = 1.0f;
                        if ((float)op.progress >= 0.9f)
                        {
                            uILoadScene?.SetLoadReadyMode();
                            break;
                        }
                        uILoadScene?.SetProcessValue(loadprocess);

                    }
                    else
                    {
                        loadprocess += Random.Range(0.01f, 0.05f);
                        uILoadScene?.SetProcessValue(loadprocess);

                    }
                    yield return new WaitForSeconds(0.05f);
                }
                if (enableApplicationProgress)
                {
                    while (true)
                    {
                        loadprocess = applicationProgress;
                        uILoadScene.SetProcessValue(loadprocess);

                        if (loadprocess >= 0.9f)
                        {
                            break;
                        }
                        yield return null;
                    }
                }
                yield return null;
                while (true)
                {

                    if (Input.anyKeyDown)
                    {

                        if (uILoadScene)
                        {

                            yield return new WaitForSeconds(uILoadScene.SetLoadReadyEnd());

                        }

                        break;
                    }
                    yield return null;

                }


                applicationProgress = 0.0f;
                isonloding = false;
                strTips = "";
                //      uILoadScene?.HidePage();
                Debug.Log("over");
                op.allowSceneActivation = true;

            }


        }
        [System.Serializable]
        public class LoadSenctSettingData
        {
            public string tips;
            [ValueDropdown("namelist")]
            public string targetName;
            public string[] namelist
            {

                get
                {
                    return
                        TDKToolkitPag.GetBuildSceneList().ToArray();
                }

            }

            public Sprite loadsprite;

        }

        #endregion

    }


}

