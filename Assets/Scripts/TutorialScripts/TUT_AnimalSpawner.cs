using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_AnimalSpawner : MonoBehaviour
{
    [Header("AnimalsToSpawn")]
    private GameObject animalToSpawn;
    private GameObject schoolAnimal;


    [Header("SpawnPosVars")]
    private Transform xSpawnPos;
    private float yCorralMin, yCorralMax;

    private GameManager gameManager;

    public TUT_AnimalSpawner(GameObject _schoolAnimal, float _yCorralMin, float _yCorralMax, Transform _xSpawnPos, GameManager _gameManager)
    {
        schoolAnimal = _schoolAnimal;
        yCorralMin = _yCorralMin;
        yCorralMax = _yCorralMax;
        xSpawnPos = _xSpawnPos;
        gameManager = _gameManager;
    }


    public void SpawnSchoolAnimal()
    {
        animalToSpawn = schoolAnimal;
        Vector2 spawnPos = new Vector2(xSpawnPos.position.x + Random.Range(0, 1f), Random.Range(yCorralMin, yCorralMax));

        GameObject instance = GameObject.Instantiate(animalToSpawn, spawnPos, animalToSpawn.transform.rotation);
        gameManager.AddAnimalToList(instance.transform.GetChild(instance.transform.childCount - 1).gameObject);
    }
}
