using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamipakku : MonoBehaviour
{
    [SerializeField]
    [Tooltip("x軸の回転角度")]
    private float rotateX = 0;
    
    [SerializeField]
    [Tooltip("y軸の回転角度")]
    private float rotateY = 0;

    [SerializeField]
    [Tooltip("z軸の回転角度")]
    private float rotateZ = 50;
    private static bool buttonFlag;

    public static bool getButtonFlag()
    {
        return buttonFlag;
    }

    public void Start()
    {
        Debug.Log("1" + buttonFlag);
        buttonFlag = false;
    }
    public void Update()
    {
        if(buttonFlag == true)
        {
            
        }
        else if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("3" + buttonFlag);
            buttonFlag = true;
        }
        else
        {        
            // X,Y,Z軸に対してそれぞれ、指定した角度ずつ回転させている。
            // deltaTimeをかけることで、フレームごとではなく、1秒ごとに回転するようにしている。
            gameObject.transform.Rotate(new Vector3(rotateX, rotateY, rotateZ) * Time.deltaTime);
        }
    }

    
}
