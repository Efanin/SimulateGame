using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Animal : MonoBehaviour
{
    [SerializeField] GameObject food;

    private MoveAnimal moveAnimal;
    private int sizeField;
    private int speed;
    private int maxTimeSpawn = 5;
    private List<Transform> listFood = new();
    private bool isRequiredParameters = false;

    public void SetParameters(int sizeField,int speed, List<Transform> listFood)
    {
        this.sizeField = sizeField;
        this.speed = speed;
        this.listFood = listFood;
        isRequiredParameters = true;
    }
    public Transform SpawnFood(int n = 0)
    {
        if (n > 100)
        {
            Debug.LogError("It is impossible to create an object, there is no place on the field");
            return null; 
        }
        if (isRequiredParameters == false)
            Debug.LogError("Required parameters are not specified: field size, animal speed, listFood");
        Vector3 position = newPosition();
        foreach (var item in listFood)
        {
            if (position == item.transform.position)
                return SpawnFood(n + 1);
        }
        return SpawnFoodWithPosition(position);
    }
    public Transform SpawnFoodWithPosition(Vector3 position)
    {
        GameObject newfood = Instantiate(food, position, Quaternion.identity);
        setColor(gameObject, newfood);
        moveAnimal = newMoveAnimal(newfood.transform, listFood, speed, sizeField);
        return newfood.transform;
    }
    private Vector3 newPosition()
    {
        int timeDistance = maxTimeSpawn * speed;
        return new(
            Random.Range(
                point((int)transform.position.x, timeDistance, sizeField),
                point((int)transform.position.x, -timeDistance, -sizeField)),
            0,
            Random.Range(
                point((int)transform.position.z, timeDistance, sizeField),
                point((int)transform.position.z, -timeDistance, -sizeField)));
    }
    private int point(int postion, int timeDistance, int sizeField)
    {
        return Mathf.Abs(postion + timeDistance) > Mathf.Abs(sizeField / 2) ?
            sizeField / 2 :
            postion + timeDistance;
    }

    private void setColor(GameObject Animal, GameObject Food)
    {
        Color color = new(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        Animal.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", color);
        Food.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", color);
    }

    private MoveAnimal newMoveAnimal(Transform food, List<Transform> listFood, int speed,int sizeField)
    {
        MoveAnimal moveAnimal = GetComponent<MoveAnimal>();
        moveAnimal.listFood = listFood;
        moveAnimal.Food = food;
        moveAnimal.Speed = speed;
        moveAnimal.SizeField = sizeField;
        return moveAnimal;
    }
}
