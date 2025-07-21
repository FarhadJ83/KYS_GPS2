using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq; // Required for LINQ methods like .Max()

[System.Serializable]
public class PrefabMapping
{
    [Tooltip("The character used in the grid layout text to represent this prefab. If more than one character is entered, only the first will be used.")]
    public string id;
    [Tooltip("The list of prefabs to instantiate. World 1 uses the first prefab. World 2 uses the second, or the first if a second isn't assigned.")]
    public List<GameObject> prefabs = new List<GameObject>(); // Changed to a list
    [Tooltip("Rotation in Euler angles to apply to the prefab instance (local rotation). This is applied to all prefabs in this mapping.")]
    public Vector3 rotationEuler;
}

public class WorldGenerator : EditorWindow
{
    private string gridLayoutText = "00000\n01110\n01210\n01110\n00000";
    [Tooltip("This is the minimum empty space to leave BETWEEN the calculated bounds of adjacent objects.")]
    private Vector3 gridSpacing = new Vector3(1.2f, 0f, 1.2f); // Represents minimum gap between object bounds
    private Transform parentObject;
    private Vector2 scrollPosition;

    public List<PrefabMapping> prefabMappings = new List<PrefabMapping>();

    private SerializedObject serializedObject;
    private SerializedProperty serializedMappingsProperty;

    [MenuItem("Tools/World Generator")]
    public static void ShowWindow()
    {
        GetWindow<WorldGenerator>("Prefab Grid");
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        serializedMappingsProperty = serializedObject.FindProperty("prefabMappings");
    }

    private void OnGUI()
    {
        serializedObject.Update();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.LabelField("1. Prefab Definitions", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Define which character (ID) corresponds to which prefabs. World 1 uses the first prefab in the list. World 2 uses the second (or first if no second exists). All prefabs in a mapping share the same rotation.", MessageType.Info);
        EditorGUILayout.PropertyField(serializedMappingsProperty, true);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("2. Grid Layout", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Type your grid layout. Each character is a prefab ID. Unrecognized characters are ignored.", MessageType.Info);
        gridLayoutText = EditorGUILayout.TextArea(gridLayoutText, GUILayout.Height(150), GUILayout.ExpandWidth(true));

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("3. Settings", EditorStyles.boldLabel);
        parentObject = (Transform)EditorGUILayout.ObjectField("Parent Object (Optional)", parentObject, typeof(Transform), true);
        EditorGUILayout.HelpBox("Generated 'World_1' and 'World_2' folders will be children of this object. If none, a new 'GeneratedWorld' object is created.", MessageType.None);

        gridSpacing = EditorGUILayout.Vector3Field("Min Object Spacing (X, Z)", new Vector3(gridSpacing.x, 0f, gridSpacing.z));
        gridSpacing.y = 0f;
        EditorGUILayout.HelpBox("The minimum empty space between the edges of adjacent objects.", MessageType.None);

        EditorGUILayout.Space(20);
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Generate Worlds", GUILayout.Height(40)))
        {
            GenerateGrid();
        }
        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("Clear Generated Objects", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("Clear Confirmation",
                "This will delete all children of the assigned Parent Object (or of 'GeneratedWorld' if none is assigned). Are you sure?", "Yes, Clear", "No"))
            {
                ClearGeneratedObjects();
            }
        }

        EditorGUILayout.EndScrollView();
        serializedObject.ApplyModifiedProperties();
    }

    private void GenerateGrid()
    {
        if (prefabMappings == null || prefabMappings.Count == 0)
        {
            Debug.LogError("World Generator: No prefab mappings defined.");
            return;
        }

        // --- SETUP ---
        var prefabMapDict = new Dictionary<string, PrefabMapping>();
        foreach (var mapping in prefabMappings)
        {
            if (mapping.prefabs != null && mapping.prefabs.Count > 0 && !string.IsNullOrEmpty(mapping.id))
            {
                string key = mapping.id.Substring(0, 1);
                if (!prefabMapDict.ContainsKey(key)) prefabMapDict.Add(key, mapping);
                else Debug.LogWarning($"World Generator: Duplicate ID '{key}'. Using first entry found.");
            }
        }
        if (prefabMapDict.Count == 0)
        {
            Debug.LogError("World Generator: No valid mappings (missing ID or prefabs).");
            return;
        }

        if (parentObject == null)
        {
            GameObject newParent = GameObject.Find("GeneratedWorld") ?? new GameObject("GeneratedWorld");
            parentObject = newParent.transform;
        }

        ClearGeneratedObjects();

        Transform world1Parent = new GameObject("World_1").transform;
        world1Parent.SetParent(parentObject, false);
        Transform world2Parent = new GameObject("World_2").transform;
        world2Parent.SetParent(parentObject, false);

        string[] lines = gridLayoutText.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        int maxRows = lines.Length;
        int maxCols = lines.Length > 0 ? lines.Max(l => l.Length) : 0;
        if (maxRows == 0 || maxCols == 0)
        {
            Debug.LogWarning("World Generator: Grid layout is empty.");
            return;
        }

        // --- PRE-CALCULATION & CACHING ---
        EditorUtility.DisplayProgressBar("World Generator", "Calculating prefab bounds...", 0.1f);
        var prefabBoundsCache = new Dictionary<GameObject, Bounds>();
        var pivotToCenterOffsetsCache = new Dictionary<GameObject, Vector3>();

        foreach (var mapping in prefabMapDict.Values)
        {
            foreach (var prefab in mapping.prefabs)
            {
                if (prefab != null && !prefabBoundsCache.ContainsKey(prefab))
                {
                    GameObject tempInstance = null;
                    try
                    {
                        tempInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        tempInstance.transform.rotation = Quaternion.Euler(mapping.rotationEuler);
                        Bounds bounds = GetBounds(tempInstance);
                        prefabBoundsCache[prefab] = bounds;
                        pivotToCenterOffsetsCache[prefab] = (bounds.size != Vector3.zero) ? bounds.center - tempInstance.transform.position : Vector3.zero;
                    }
                    finally { if (tempInstance != null) DestroyImmediate(tempInstance); }
                }
            }
        }

        // --- LAYOUT DIMENSION CALCULATION ---
        EditorUtility.DisplayProgressBar("World Generator", "Calculating layout dimensions...", 0.3f);
        float[] maxCellWidths = new float[maxCols];
        float[] maxCellHeights = new float[maxRows];

        for (int z = 0; z < maxRows; z++)
        {
            string line = lines[z];
            for (int x = 0; x < line.Length; x++)
            {
                if (prefabMapDict.TryGetValue(line[x].ToString(), out PrefabMapping mapping))
                {
                    foreach (var prefab in mapping.prefabs)
                    {
                        if (prefab != null && prefabBoundsCache.TryGetValue(prefab, out Bounds bounds))
                        {
                            maxCellWidths[x] = Mathf.Max(maxCellWidths[x], bounds.size.x);
                            maxCellHeights[z] = Mathf.Max(maxCellHeights[z], bounds.size.z);
                        }
                    }
                }
            }
        }

        // --- CUMULATIVE OFFSET CALCULATION ---
        float[] cumulativeXOffsets = new float[maxCols + 1];
        float[] cumulativeZOffsets = new float[maxRows + 1];
        for (int x = 0; x < maxCols; x++) cumulativeXOffsets[x + 1] = cumulativeXOffsets[x] + maxCellWidths[x] + gridSpacing.x;
        for (int z = 0; z < maxRows; z++) cumulativeZOffsets[z + 1] = cumulativeZOffsets[z] + maxCellHeights[z] + gridSpacing.z;

        // --- INSTANTIATION ---
        EditorUtility.DisplayProgressBar("World Generator", "Generating worlds...", 0.6f);
        for (int z = 0; z < maxRows; z++)
        {
            string line = lines[z];
            for (int x = 0; x < line.Length; x++)
            {
                if (prefabMapDict.TryGetValue(line[x].ToString(), out PrefabMapping mapping) && mapping.prefabs.Count > 0 && mapping.prefabs[0] != null)
                {
                    float cellCenterX = (cumulativeXOffsets[x] + cumulativeXOffsets[x + 1] - gridSpacing.x) / 2f;
                    float cellCenterZ = (cumulativeZOffsets[z] + cumulativeZOffsets[z + 1] - gridSpacing.z) / 2f;
                    Vector3 targetBoundsCenterWorld = parentObject.TransformPoint(new Vector3(cellCenterX, 0, -cellCenterZ));

                    // Instantiate for World 1
                    GameObject prefab1 = mapping.prefabs[0];
                    InstantiateObjectInCell(prefab1, mapping.rotationEuler, world1Parent, targetBoundsCenterWorld, pivotToCenterOffsetsCache);

                    // Instantiate for World 2 (fallback to prefab 1 if needed)
                    GameObject prefab2 = (mapping.prefabs.Count > 1 && mapping.prefabs[1] != null) ? mapping.prefabs[1] : prefab1;
                    InstantiateObjectInCell(prefab2, mapping.rotationEuler, world2Parent, targetBoundsCenterWorld, pivotToCenterOffsetsCache);
                }
            }
        }

        EditorUtility.ClearProgressBar();
        Debug.Log($"World Generator: Complete! Created 'World_1' and 'World_2' under '{parentObject.name}'.");
    }

    private void InstantiateObjectInCell(GameObject prefab, Vector3 eulerRotation, Transform parent, Vector3 targetBoundsCenterWorld, Dictionary<GameObject, Vector3> pivotOffsetsCache)
    {
        if (prefab == null || !pivotOffsetsCache.TryGetValue(prefab, out Vector3 pivotToCenterOffset)) return;

        Vector3 targetPivotWorldPosition = targetBoundsCenterWorld - pivotToCenterOffset;
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
        instance.transform.position = targetPivotWorldPosition;
        instance.transform.localRotation = Quaternion.Euler(eulerRotation);
    }

    private Bounds GetBounds(GameObject obj)
    {
        Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);
        bool hasBounds = false;

        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            if (!hasBounds) { bounds = r.bounds; hasBounds = true; }
            else { bounds.Encapsulate(r.bounds); }
        }

        if (!hasBounds)
        {
            foreach (Collider c in obj.GetComponentsInChildren<Collider>())
            {
                if (!hasBounds) { bounds = c.bounds; hasBounds = true; }
                else { bounds.Encapsulate(c.bounds); }
            }
        }
        return bounds;
    }

    private void ClearGeneratedObjects()
    {
        Transform parentToClear = parentObject;
        if (parentToClear == null)
        {
            GameObject defaultParent = GameObject.Find("GeneratedWorld");
            if (defaultParent != null) parentToClear = defaultParent.transform;
            else return;
        }

        GameObject[] children = new GameObject[parentToClear.childCount];
        for (int i = 0; i < parentToClear.childCount; i++)
        {
            children[i] = parentToClear.GetChild(i).gameObject;
        }

        foreach (GameObject child in children)
        {
            if (child != null) DestroyImmediate(child);
        }

        if (children.Length > 0)
            Debug.Log($"World Generator: Cleared {children.Length} child objects from '{parentToClear.name}'.");
    }
}