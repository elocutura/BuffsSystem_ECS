using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class Stats :  MonoBehaviour{

    public Dictionary<string, float> stats = new Dictionary<string, float>();
    public Dictionary<string, float> statMultipliers = new Dictionary<string, float>();

    public List<Buff> buffsPending = new List<Buff>();
    public List<Buff> buffsActive = new List<Buff>();

    public Text textToTest;

    public GameObject buffUIPrefab;
    public RectTransform buffUIHolder;
    public RectTransform deBuffUIHolder;
}
