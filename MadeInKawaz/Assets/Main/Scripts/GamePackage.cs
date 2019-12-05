using UnityEngine;

[CreateAssetMenu]
public class GamePackage : ScriptableObject
{
    public string sceneName;
    public string titleName;
    public string authorName;
    public string developerName;
    public string statement;
    [Multiline(3)]
    public string explanation;
    public GameType gameType;
    public Texture2D iconImage;
    public Texture2D screenshotImage;    
}

public enum GameType
{
    Short,
    Long,
    Boss
}
