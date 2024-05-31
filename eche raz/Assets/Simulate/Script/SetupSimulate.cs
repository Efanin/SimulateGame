using System.Collections.Generic;
using UnityEngine;

public class SetupSimulate : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject animal;
    [SerializeField, Range(2, 1000)] private int sizeField;
    [SerializeField, Range(1, 100)] private int speedAnimal;
    [SerializeField, Min(1)] private int countAnimal;
    [SerializeField, Range(0f, 100f)] private float sumulateSpeed = 1f;

    private List<Transform> listAnimal = new();
    private List<Transform> listFood = new();

    [SerializeField] private bool buttonSave = false;
    [SerializeField] private bool load = false;

    private void OnValidate()
    {
        countAnimal = countAnimal > (sizeField * sizeField) / 2 ? 
            (sizeField * sizeField) / 2:
            countAnimal;
    }
    private void Start()
    {
        field.GetComponent<Field>().SetupField(load ? SaveSystem.LoadData().sizeField : sizeField);
        for (int i = 0; i < (load ? SaveSystem.LoadData().positionAnimal.GetLength(0) : countAnimal); i++) 
            spawnAnimal(i);
    }

    private void Update()
    {
        Time.timeScale = sumulateSpeed;
        if (buttonSave)
        {
            SaveSystem.SaveData(sizeField, speedAnimal, listAnimal, listFood);
            buttonSave = false;
        }
    }
    private void spawnAnimal(int index)
    {
        Vector3 position;
        if (load == false)
        {
            position = new(
                Random.Range(-sizeField / 2, sizeField / 2),
                0,
                Random.Range(-sizeField / 2, sizeField / 2));
        }
        else
        {
            position = new(
            SaveSystem.LoadData().positionAnimal[index, 0],
            SaveSystem.LoadData().positionAnimal[index, 1],
            SaveSystem.LoadData().positionAnimal[index, 2]);
            sizeField = SaveSystem.LoadData().sizeField;
            speedAnimal = SaveSystem.LoadData().speedAnimal;
        }
        var newAnimal = Instantiate(animal, position, Quaternion.identity);
        newAnimal.GetComponent<Animal>().SetParameters(sizeField, speedAnimal, listFood);
        var newFood = load ?
            newAnimal.GetComponent<Animal>().SpawnFoodWithPosition(new Vector3(
                SaveSystem.LoadData().positionFood[index, 0],
                SaveSystem.LoadData().positionFood[index, 1],
                SaveSystem.LoadData().positionFood[index, 2])):
                newAnimal.GetComponent<Animal>().SpawnFood();
        listAnimal.Add(newAnimal.transform);
        listFood.Add(newFood.transform);
    } 
 
    
}
