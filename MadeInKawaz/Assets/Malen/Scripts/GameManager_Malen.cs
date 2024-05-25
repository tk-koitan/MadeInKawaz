using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Malen : MonoBehaviour
{
    [SerializeField] GameObject[] people;
    [SerializeField] Sprite correctSprite;
    [SerializeField] ParticleSystem particle;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip correctSE;
    [SerializeField] AudioClip incorrectSE;
    private bool flag;
    private GameObject correctPerson;

    private void Start()
    {
        flag = false;
        correctPerson = people[Random.Range(0, people.Length)];
        correctPerson.GetComponent<Image>().sprite = correctSprite;
    }

    public void OnClick()
    {
        if (flag == false)
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            foreach (GameObject person in people)
            {
                if(person != correctPerson){
                    person.GetComponent<Button>().interactable = false;
                }
            }
            if(selectedObject == correctPerson){
                particle.transform.position = correctPerson.transform.position;
                particle.Play();
                audioSource.PlayOneShot(correctSE);
                GameManager.Clear();
            }else{
                audioSource.PlayOneShot(incorrectSE);
            }
        }
        flag = true;
    }
}
