using System.Formats.Tar;
using System.IO;

namespace Cubie;

public class PlayerStats
{
    private float _speed;
    private float _accurancy;
    private float _memory;
    public static readonly string filePath = "playerStats.txt";

    public PlayerStats(float speed, float accurancy, float memory)
    {
        _speed = speed;
        _accurancy = accurancy;
        _memory = memory;   
    }
    public void SaveStats()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(Speed);
            writer.WriteLine(Accurancy);
            writer.WriteLine(Memory);
        }
    }
    public void LoadStats()
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            Speed = float.Parse(reader.ReadLine());
            Accurancy = float.Parse(reader.ReadLine());
            Memory = float.Parse(reader.ReadLine());
        }
    }
    public void UpdateStats()
    {

    }
    //Properties
    private float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    private float Accurancy
    {
        get { return _accurancy; }
        set { _accurancy = value; }
    }
    private float Memory
    {
        get { return _memory; }
        set { _memory = value; }
    }

}