using UnityEngine;

public class AnimalSpawner 
{
    /*Cómo voy a solucionar el tema de pasarle la data a esto?
       Entrada:
            int[] animalsAmountPerRow
            GameObject[] animalsPrefabs
            Transform yCorralMin, yCorralMax, xSpawnPos*/
            

    [Header("AnimalsToSpawn")]
    private GameObject animalToSpawn;
    private GameObject[] animalsPrefabs;
    private GameObject fishPrefab;


    [Header("SpawnPosVars")]
    private Transform xSpawnPos;
    private float yCorralMin, yCorralMax;

    private GameManager gameManager;

    public AnimalSpawner( GameObject[] _animalsPrefabs, GameObject _fish,float _yCorralMin, float _yCorralMax, Transform _xSpawnPos, GameManager _gameManager)
    {
        animalsPrefabs = _animalsPrefabs;
        fishPrefab = _fish;
        yCorralMin = _yCorralMin;
        yCorralMax = _yCorralMax;
        xSpawnPos = _xSpawnPos;
        gameManager = _gameManager;
    }


   
    public void SpawnAnimalRound(int animalsAmount)
    {
        for (int i = 0; i < animalsAmount; i++)
            SpawnSoloAnimal();
    }

    public void SpawnSoloAnimal()
    {
        animalToSpawn = animalsPrefabs[Random.Range(0, animalsPrefabs.Length)];
        Vector2 spawnPos = new Vector2(xSpawnPos.position.x + Random.Range(0, 1f), Random.Range(yCorralMin, yCorralMax));

        GameObject instance = GameObject.Instantiate(animalToSpawn, spawnPos, animalToSpawn.transform.rotation);
        gameManager.AddAnimalToList(instance.transform.GetChild(instance.transform.childCount - 1).gameObject);
    }

    public void SpawnFish()
    {
        animalToSpawn = fishPrefab;
        Vector2 spawnPos = new Vector2(xSpawnPos.position.x + Random.Range(0, 1f), Random.Range(yCorralMin, yCorralMax));

        GameObject instance = GameObject.Instantiate(animalToSpawn, spawnPos, animalToSpawn.transform.rotation);
        gameManager.AddAnimalToList(instance.transform.GetChild(instance.transform.childCount - 1).gameObject);
    }
  


}
