using System.Collections.Generic;
using UnityEngine;

public class MoveAnimal : MonoBehaviour
{
    [HideInInspector] public Transform Food { get; set; }
    [HideInInspector] public List<Transform> listFood { get; set; }
    [HideInInspector] public int Speed { get; set; }
    [HideInInspector] public int SizeField { get; set; }

    private System.Func<Vector3> target;
    private Vector3 newTargetAround;
    private void Start()
    {
        target = targetFood;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target(),
            Time.deltaTime * Speed);
        if(newTargetAround == transform.position)
            target = targetFood;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Food>() == Food.GetComponent<Food>())
        {
            for (int i = 0; i < listFood.Count; i++)
                if (listFood[i] == Food)
                    listFood.RemoveAt(i);
            Destroy(other.gameObject.GetComponentInParent<Food>().gameObject);
            listFood.Add(GetComponent<Animal>().SpawnFood());
            return;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Food>() != null ||
            collision.gameObject.GetComponentInParent<Animal>() != null)
        {
            newTargetAround = transform.position +
                (new Vector3(
                Random.Range(-0.5f, 0.5f),
                0,
                Random.Range(-0.5f, 0.5f)) *
                distanceAround() / 2) / 2;
            target = targetAround;
        }
    }
    private float distanceAround()
    {
        return Mathf.Min(
        SizeField/2 - Mathf.Abs(transform.position.x),
        SizeField/2 - Mathf.Abs(transform.position.y));
    }
    private Vector3 targetFood()
    {
        return Food.position;
    }
    private Vector3 targetAround()
    {
        return newTargetAround;
    }

}
