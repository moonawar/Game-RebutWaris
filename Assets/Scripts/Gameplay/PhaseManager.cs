using UnityEngine;

[System.Serializable]
public class Phase {
    public float limit;
    public float increase;
}

public class PhaseManager : MonoBehaviour {
    [SerializeField] private Phase[] phases = new Phase[3];
}