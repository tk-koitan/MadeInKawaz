using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DebugMenu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMesh;
    public static MenuNode rootNode = new MenuNode("root",0);    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string str = string.Empty;
        rootNode.OutputNames(ref str);
        textMesh.text = str;
    }


    public static void AddDebugCommand(string path, Action act)
    {
        string[] names = path.Split('/');
        Dictionary<string, MenuNode> cd = rootNode.commandDict;
        for (int i = 0; i < names.Length - 1; i++)
        {
            if (!cd.ContainsKey(names[i]))
            {
                cd.Add(names[i], new MenuNode(names[i], i));
            }
            cd = cd[names[i]].commandDict;
        }
        cd.Add(names[names.Length - 1], new MenuNode(names[names.Length - 1], names.Length - 1, act));
    }

    public class MenuNode
    {
        public string name;
        public Dictionary<string, MenuNode> commandDict = new Dictionary<string, MenuNode>();
        public int level;
        Action onClick;

        public MenuNode(string n, int l, Action act = null)
        {
            name = n;
            level = l;
            onClick = act;
        }

        public void OutputNames(ref string str)
        {
            string blank = new string(' ', level * 4);
            str += blank + name + "\n";
            foreach(MenuNode node in commandDict.Values)
            {
                node.OutputNames(ref str);
            }
        }
    }
}
