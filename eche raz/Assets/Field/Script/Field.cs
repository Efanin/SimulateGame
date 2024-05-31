using UnityEngine;

public class Field : MonoBehaviour
{
    public void SetupField(int size) =>
        transform.localScale = new(size, 1f, size);
}
