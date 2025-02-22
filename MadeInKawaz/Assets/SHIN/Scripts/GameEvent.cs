using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager; (removed because of build error By y_y: 2025_02_22
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private static float currentAudioTime = 0;
    public List<GameObject> prefabs = new List<GameObject>();
    private List<SpriteMove> sprites= new List<SpriteMove>();
    public int Level = 5;
    public AudioSource audioSource;
    public AudioClip clearAudio;

    public ParticleSystem senseiParticle;

    public float senseiNoKigen = 0;
    void Start()
    {
        audioSource.time = currentAudioTime;
        for(int i = 0; i < Level; i++)  
        {
            sprites.Add(Instantiate(prefabs[Random.Range(0, prefabs.Count)], new Vector3(0, 0, 0), Quaternion.identity).GetComponent<SpriteMove>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        bool allStop = true;
        int count = 0;
        foreach(SpriteMove  sprite in sprites)
        {
            if(!sprite.stopDancing)
            {
                allStop = false;
            }
            else
            {
                count++;
            }
        }
        senseiNoKigen = ((float)count)/((float)Level);
        if(allStop)
        {
            if(!GameManager.ClearFlag){
                currentAudioTime = audioSource.time;
                senseiParticle.Play();
                audioSource.Stop();
                audioSource.clip = clearAudio;
                audioSource.Play();
            }
            GameManager.Clear();
        }
    }
}
