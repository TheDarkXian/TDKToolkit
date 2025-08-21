using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


namespace TDKToolkit
{
    public class EAManger : SingletonMono<EAManger>, IEPoolBase
    {
        protected PoolBase pool;
        public AudioSource backGroundAudio;
        AudioClip nowBGM { get => backGroundAudio.clip; set => backGroundAudio.clip = value; }
        [Button]
        protected void OnValidate()
        {
            pool = transform.TDKGetOrAddComponent<PoolBase>();
            pool.poolPrefebsSetting.loadType = PrefebsSetting.PrefebsLoadType.FromScript;
            pool.poolPrefebsSetting.prefebPath = "AudioEffectBase";
            pool.poolPrefebsSetting.LoadPrefeb();
            Transform temp = TDKToolkitPag.FindOrGetAObj("BackGroundAudio", transform).transform;
            backGroundAudio = temp.TDKGetOrAddComponent<AudioSource>();
            pool.poolParentActive = TDKToolkitPag.FindOrGetAObj("EffectAudioPoolActive", transform).transform;


        }
        public static void SetBGM(AudioClip audioClip)
        {
            Instance.nowBGM = audioClip;
            Instance.backGroundAudio.Stop();
        }
        public static void PlayBGM()
        {
            Instance.
                backGroundAudio.Play();
        }
        public static void StopBGM()
        {
            Instance.backGroundAudio.Stop();
        }
        public static void SetBGMVloume(float vloume)
        {
            Instance.
                backGroundAudio.volume = vloume;
        }
        private void Awake()
        {
            OnValidate();
            pool.IniPool();

        }

        public static void PlayVioce(AudioClip audioClip, Vector3 pos, float is3D = 1)
        {
            GameObject audioobj = Instance.pool.InifromPool().gameObject;
            PoolableObject poolableObject = audioobj.GetComponent<PoolableObject>();
            AudioSource audioSource = audioobj.GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.Play();
            audioSource.spatialBlend = is3D;
            Sanlock sanlock = new Sanlock(audioClip.length);
            sanlock.invokeOnEnd += poolableObject.ReturnPool;
            sanlock.StartSandlock();
        }
        public static void PlayVioce(AudioClip audioClip, Transform pos, float is3D = 1)
        {
            PlayVioce(audioClip, pos.position, is3D);
        }

        public virtual void ClearPool()
        {
            pool.ClearPool();
        }

        public virtual IEPoolBeahavior InifromPool()
        {
            return pool.InifromPool();
        }
        public virtual void RenturnPool(IEPoolBeahavior target)
        {
            pool.ReturnPool(target);

        }

    }

}


