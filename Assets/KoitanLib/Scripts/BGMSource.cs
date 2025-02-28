using UnityEngine;

//ScriptableObjectを継承したクラス
[CreateAssetMenu]
public class BGMSource : ScriptableObject
{

    //設定したいデータの変数
    public bool isLoop;
    public AudioClip BGM;
    public float startTime,endTime;    

    /*簡略化のために全てpublicにしてますが、Scriptableobjectは基本的に変更しないデータを扱うので、
    以下のようにprivateな変数にSerializeFieldを付けて、getterとsetterを別途用意する方が安全です。
    setterは後述する「プログラムから作成」の時に使います。

    [SerializeField]
    private bool _isBoss = false;
    public  bool  IsBoss {
      get { return _isBoss; }
      #if UNITY_EDITOR
      set { _isBoss = value; }
      #endif
    }

    */

}