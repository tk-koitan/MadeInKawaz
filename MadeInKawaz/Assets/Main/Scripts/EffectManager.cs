using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [SerializeField]
    GameObject[] effects;

    [SerializeField]
    ParticleSystem eff;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        StopEffect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static GameObject EmitEffect(EffectCode code)
    {
        return Instantiate(Instance.effects[(int)code]);
    }

    public static GameObject EmitEffect(EffectCode code, Vector3 pos)
    {
        return Instantiate(Instance.effects[(int)code], pos, Quaternion.identity);
    }

    public static void PlayEffect()
    {
        Instance.eff.Play();
    }

    public static void StopEffect()
    {
        Instance.eff.Stop();        
    }
}

public enum EffectCode
{
    Kamihubuki = 0
}
