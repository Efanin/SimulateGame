using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem 
{
    private static string nameFile = "/save.data";
    public static void SaveData(int sizeField, int speedAnimal, List<Transform> positionAnimal, List<Transform> positionFood)
    {
        string path = Application.persistentDataPath + nameFile;
        FileStream stream = new(path, FileMode.Create);
        new BinaryFormatter().Serialize(stream, new DataFile(sizeField, speedAnimal, positionAnimal, positionFood));
        stream.Close();
    }
    public static DataFile LoadData()
    {
        string path = Application.persistentDataPath + nameFile;
        if (!File.Exists(path))
        {
            Debug.LogError("File not found");
            return null;
        }
        FileStream stream = new(path, FileMode.Open);
        var data = new BinaryFormatter().Deserialize(stream) as DataFile;
        stream.Close();
        return data;
    }
}
[System.Serializable]
public class DataFile
{
    public int sizeField;
    public int speedAnimal;
    public float[,] positionAnimal;
    public float[,] positionFood;
    public DataFile(int sizeField, int speedAnimal,  List<Transform> positionAnimal, List<Transform> positionFood)
    {
        
        this.sizeField = sizeField;
        this.speedAnimal = speedAnimal;
        addPosition(out this.positionAnimal, positionAnimal);
        addPosition(out this.positionFood, positionFood);
    }
    private void addPosition(out float[,] dataPosition, List<Transform> position)
    {
        dataPosition = new float[position.Count, 3];
        for (int i = 0; i < position.Count; i++)
        {
            dataPosition[i, 0] = position[i].position.x;
            dataPosition[i, 1] = position[i].position.y;
            dataPosition[i, 2] = position[i].position.z;
        }
    }
}