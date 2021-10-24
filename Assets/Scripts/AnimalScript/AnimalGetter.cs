using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyExtensionMethods;

public class AnimalGetter
{
    //Puta este script se cae a pedazos aaaaaaaaaaa.
    //MUY BONITO PERO HAY QUE HACERLO DE NUEVO.



    [SerializeField] private GameObject door;
    [SerializeField] private GameObject[] animalsArray;
    [SerializeField] private List<GameObject> animalsList;
    [SerializeField] private Transform rightCorralLimit;
    
    public List<GameObject> AnimalsList { get => animalsList; }
    public GameObject returnAnim;


    public AnimalGetter(Transform _rightCorralLimit)
    {
        rightCorralLimit = _rightCorralLimit;
        animalsList = new List<GameObject>();
    }

   

    public void AddAnimalToList(GameObject animal)
    {
        animalsList.Add(animal);
        Debug.Log("Animals in list: " + animalsList.Count);
    }

    public void RemoveAnimalFromList(GameObject animal)
    {
        
        Debug.Log("Removing: " + animal.name);
        if (animalsList.Contains(animal))
            animalsList.Remove(animal);
    }

    public void ReduceAnimalList()
    {

        //PARA OPTIMIZAR HAY QUE USAR List.RemoveAt() EL CUAL ELIMINA Y TRAE TODOS LOS ELEMENTOS UNA POSICIÓN MÁS ATRÁS.

        for (int i = 0; i < animalsList.Count; i++)
        {
            if (animalsList[i] == null || animalsList[i].activeSelf == false)
                animalsList.RemoveAt(i);
            else
            {
                AnimalState anim = animalsList[i].GetComponent<AnimalStateMachine>().CurrentState;

                if (anim != null)
                {
                    if (anim.Dead == true)
                    {
                        //anim = null;
                        //gameManager.AnimalsKilled++;
                        Debug.LogWarning("Remooving animal at: " + i);
                        animalsList.RemoveAt(i);
                    }
                }
            }
        }
    }



    public GameObject GetFirstAnimal()
    {
        returnAnim = null;

        if (animalsList.Count != 0)
        {
            foreach (GameObject animal in animalsList)
            {

                if (returnAnim == null && animal.transform.position.x < rightCorralLimit.position.x)
                {
                    returnAnim = animal;
                }

                if (returnAnim != null)
                    if ((animal.transform.position.x <= returnAnim.transform.position.x) && animal.transform.position.x < rightCorralLimit.position.x)
                    {
                            returnAnim = animal;
                    }
            }
        }
        return returnAnim;
    }
}
