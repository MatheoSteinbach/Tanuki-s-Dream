using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenData : MonoBehaviour
{
    public List<DungeonRoom> Rooms { get; set; } = new List<DungeonRoom>();
    public HashSet<Vector2Int> Path { get; set; } = new HashSet<Vector2Int>();
}
