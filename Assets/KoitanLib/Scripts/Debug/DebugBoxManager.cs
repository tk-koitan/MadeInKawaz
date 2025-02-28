using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

public class DebugBoxManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _textBoxPrefab;
    [SerializeField]
    private Transform _debugCanvas;
    private static GameObject textBoxPrefab;
    private static Transform debugCanvas;
    private static Camera mainCamera;

    private void Awake()
    {
        textBoxPrefab = _textBoxPrefab;
        debugCanvas = _debugCanvas;
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public static DebugBox Display(MonoBehaviour mono)
    {
        mainCamera = Camera.main;
        Vector3 pos = mainCamera.WorldToScreenPoint(mono.transform.position);
        GameObject boxObj = Instantiate(textBoxPrefab, pos, Quaternion.identity, debugCanvas);
        return boxObj.GetComponent<DebugBox>().SetBox(mono.ToString, mono.transform);
    }
}
