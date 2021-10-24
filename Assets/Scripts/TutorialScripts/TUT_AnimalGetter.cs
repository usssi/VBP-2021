using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_AnimalGetter
{
    
    private Transform rightCorralLimit;
    public GameObject schoolAnimal;


    public TUT_AnimalGetter( GameObject _animal ,Transform _rightCorralLimit)
    {
        rightCorralLimit = _rightCorralLimit;
        schoolAnimal = _animal;
    }





    public GameObject GetSchoolAnimal()
    {
        if(schoolAnimal.transform.position.x > rightCorralLimit.position.x)
        {
            return schoolAnimal;
        }

        return null;
    }
}
