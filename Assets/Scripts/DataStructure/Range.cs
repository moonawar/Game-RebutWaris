[System.Serializable]
public class Range 
{
    public float min;
    public float max;

    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float RandomValue()
    {
        return UnityEngine.Random.Range(min, max);
    }
}

public class RangeInt
{
    public int min;
    public int max;

    public RangeInt(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int RandomValue()
    {
        return UnityEngine.Random.Range(min, max);
    }
}