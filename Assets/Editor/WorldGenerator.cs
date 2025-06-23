using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq; // Required for LINQ methods like .FirstOrDefault()

[System.Serializable]
public class PrefabMapping
{
    [Tooltip("The digit used in the grid layout text to represent this prefab.")]
    public int id;
    [Tooltip("The prefab GameObject to instantiate.")]
    public GameObject prefab;
    [Tooltip("Rotation in Euler angles to apply to the prefab instance (local rotation).")]
    public Vector3 rotationEuler; // Euler angles for rotation
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
        EditorGUILayout.HelpBox("Define which number (0-9) corresponds to which prefab, its rotation, and size. Drag your prefabs here. An ID must be unique. Prefabs with no renderers or colliders might not be positioned correctly based on bounds.", MessageType.Info);

        // Draw the list of prefab mappings
        EditorGUILayout.PropertyField(serializedMappingsProperty, true);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("2. Grid Layout", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Type your grid layout below. Each digit (0-9) specifies the prefab ID to place at that grid coordinate (column, row). Non-digit characters are ignored. Each character position represents a single grid cell.", MessageType.Info);

        gridLayoutText = EditorGUILayout.TextArea(gridLayoutText, GUILayout.Height(150), GUILayout.ExpandWidth(true));

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("3. Settings", EditorStyles.boldLabel);

        parentObject = (Transform)EditorGUILayout.ObjectField("Parent Object (Optional)", parentObject, typeof(Transform), true);
        EditorGUILayout.HelpBox("Generated objects will be children of this object. If none, a new 'GeneratedWorld' object is created at the scene origin.", MessageType.None);

        // Removed the note about Y spacing for now, assuming Y is handled by prefab vertical origin/pivot
        gridSpacing = EditorGUILayout.Vector3Field("Min Object Spacing (X, Z)", new Vector3(gridSpacing.x, 0f, gridSpacing.z));
        gridSpacing.y = 0f; // Force Y spacing to 0 for this grid model
        EditorGUILayout.HelpBox("This is the minimum empty space between the *edges* of calculated bounds of adjacent objects in the X and Z dimensions.", MessageType.None);

        EditorGUILayout.Space(20);
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Generate Grid", GUILayout.Height(40)))
        {
            GenerateGrid();
        }
        GUI.backgroundColor = Color.white;

        if (parentObject != null)
        {
            GUI.backgroundColor = new Color(1f, 0.6f, 0.6f); // Light red color
            if (GUILayout.Button("Clear Generated Objects", GUILayout.Height(25)))
            {
                // Add a confirmation dialog
                if (EditorUtility.DisplayDialog("Clear Confirmation",
                    "Are you sure you want to delete all child objects of '" + parentObject.name + "'?", "Yes", "No"))
                {
                    ClearGeneratedObjects();
                }
            }
            GUI.backgroundColor = Color.white;
        }

        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }


    private void GenerateGrid()
    {
        if (prefabMappings == null || prefabMappings.Count == 0)
        {
            Debug.LogError("World Generator: No prefab mappings defined. Please add at least one ID and prefab.");
            return;
        }

        // Map IDs to PrefabMapping objects for quick lookup
        Dictionary<int, PrefabMapping> prefabMapDict = new Dictionary<int, PrefabMapping>();
        foreach (var mapping in prefabMappings)
        {
            if (mapping.prefab != null)
            {
                if (!prefabMapDict.ContainsKey(mapping.id))
                {
                    prefabMapDict.Add(mapping.id, mapping);
                }
                else
                {
                    Debug.LogWarning($"World Generator: Duplicate ID '{mapping.id}' found in mappings. Using the first entry encountered.");
                }
            }
            else
            {
                // This is handled later by checking prefabMapDict, but a heads-up is good
                Debug.LogWarning($"World Generator: Prefab mapping for ID '{mapping.id}' has no prefab assigned. It will be skipped.");
            }
        }

        if (prefabMapDict.Count == 0)
        {
            // Check if any mappings resulted in a valid entry in the dictionary
            bool hasValidMapping = false;
            foreach (var mapping in prefabMappings)
            {
                if (mapping.prefab != null && !prefabMapDict.ContainsKey(mapping.id)) // Check original list for valid entries not added due to duplicates
                {
                    // This case shouldn't happen if the above logic is correct, but adds robustness
                    hasValidMapping = true; break;
                }
            }

            if (!hasValidMapping)
            {
                Debug.LogError("World Generator: None of the defined prefab mappings have a prefab assigned or all had duplicate IDs. Nothing to generate.");
                return;
            }
        }


        // Create parent object if needed
        if (parentObject == null)
        {
            GameObject newParent = new GameObject("GeneratedWorld");
            parentObject = newParent.transform;
            Debug.LogWarning("World Generator: No parent object assigned. Created a new GameObject named 'GeneratedWorld'.");
        }

        // Clear previous generated objects *only if they are children of the designated parent*
        // Add a check to make sure we don't accidentally clear the scene if parentObject was something critical.
        // Maybe only clear objects *we* generated? This requires tracking... Simpler for now is just clearing children of *our* parent.
        ClearGeneratedObjects();

        // Parse grid layout text
        string[] lines = gridLayoutText.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        int maxRows = lines.Length;
        int maxCols = 0;
        foreach (string line in lines)
        {
            maxCols = Mathf.Max(maxCols, line.Length);
        }

        if (maxRows == 0 || maxCols == 0)
        {
            Debug.LogWarning("World Generator: Grid layout is empty or contains no valid rows/columns. Nothing to generate.");
            return;
        }


        // --- Step 1: Pre-calculate required cell sizes based on max prefab bounds in each column/row ---
        // These arrays store the maximum *physical size* (X for width, Z for depth) needed for items
        // placed in each specific column (maxCellWidths) and each specific row (maxCellHeights).
        float[] maxCellWidths = new float[maxCols];
        float[] maxCellHeights = new float[maxRows]; // Corresponds to Z in world space

        // Also pre-calculate the offset from the prefab's pivot to its bounds center after rotation for each prefab ID
        Dictionary<int, Vector3> pivotToCenterWorldOffsets = new Dictionary<int, Vector3>();


        EditorUtility.DisplayProgressBar("World Generator", "Calculating prefab bounds and layout dimensions...", 0.2f);

        // Iterate through the grid layout to find max sizes per column/row and store pivot offsets per ID
        for (int z = 0; z < maxRows; z++)
        {
            string line = lines[z];
            for (int x = 0; x < line.Length; x++)
            {
                if (x >= maxCols) continue; // Handle lines shorter than maxCols

                char character = line[x];
                if (!int.TryParse(character.ToString(), out int id))
                {
                    continue; // Skip non-numeric characters
                }

                if (prefabMapDict.TryGetValue(id, out PrefabMapping mapping))
                {
                    // Calculate bounds and pivot offset ONCE per prefab ID, but update maxCellSizes per position
                    if (!pivotToCenterWorldOffsets.ContainsKey(id) || !pivotToCenterWorldOffsets[id].y.Equals(float.NaN)) // Check if offset is already calculated or if previous attempt failed
                    {
                        // Instantiate temporarily to get bounds and pivot offset with correct rotation
                        GameObject tempInstance = null;
                        Bounds tempBounds = new Bounds();
                        bool boundsCalculated = false;

                        try
                        {
                            tempInstance = (GameObject)PrefabUtility.InstantiatePrefab(mapping.prefab);
                            tempInstance.transform.rotation = Quaternion.Euler(mapping.rotationEuler); // Apply rotation to get correct bounds

                            tempBounds = GetBounds(tempInstance);

                            if (tempBounds.size != Vector3.zero)
                            {
                                boundsCalculated = true;
                                // Calculate offset from pivot (tempInstance.transform.position is the pivot's world position)
                                // to the bounds center in world space. This offset is constant regardless of final world position.
                                pivotToCenterWorldOffsets[id] = tempBounds.center - tempInstance.transform.position;
                            }
                            else
                            {
                                Debug.LogWarning($"World Generator: Prefab '{mapping.prefab.name}' (ID: {id}) has no renderers or colliders to determine bounds. Cannot determine size or accurate pivot offset. Will be treated as zero size and placed by pivot. This may cause overlap.");
                                // Store a zero offset. Using NaN as a flag could be better, but 0 works.
                                pivotToCenterWorldOffsets[id] = Vector3.zero; // Signal that we tried but got no bounds
                            }
                        }
                        finally
                        {
                            if (tempInstance != null) DestroyImmediate(tempInstance); // Clean up temporary instance
                        }

                    }

                    // Update max size for THIS specific column (x) and THIS specific row (z) based on the bounds of the object placed HERE.
                    // This is crucial. The space for column X is determined by the widest object *in that column*.
                    if (boundsCalculated)
                    {
                        // Note: Bounds are in world space relative to the temporary rotated object's origin.
                        // size.x and size.z correspond to the rotated object's width and depth.
                        maxCellWidths[x] = Mathf.Max(maxCellWidths[x], tempBounds.size.x);
                        maxCellHeights[z] = Mathf.Max(maxCellHeights[z], tempBounds.size.z);
                    }
                    // If bounds couldn't be calculated, maxCellWidths/Heights for this cell remain 0, meaning it contributes no size requirement.

                }
            }
        }

        // --- Step 2: Calculate cumulative offsets for cell boundary positions ---
        // These define the cumulative space occupied by cells up to a certain index, including spacing.
        // Index 0 is the start. Index i+1 is the end of cell i and the start of cell i+1.
        // The total width of column X's allocated space is maxCellWidths[X] + gridSpacing.x
        // The total height of row Z's allocated space is maxCellHeights[Z] + gridSpacing.z

        float[] cumulativeXOffsets = new float[maxCols + 1]; // +1 for the end point after the last column
        float[] cumulativeZOffsets = new float[maxRows + 1]; // +1 for the end point after the last row

        cumulativeXOffsets[0] = 0;
        for (int x = 0; x < maxCols; x++)
        {
            // The end of cell X is the start of cell X + the size needed for cell X (max object width + spacing)
            cumulativeXOffsets[x + 1] = cumulativeXOffsets[x] + maxCellWidths[x] + gridSpacing.x;
        }

        cumulativeZOffsets[0] = 0;
        // Z in layout text corresponds to rows, and often represents depth in world space.
        // We'll make Z positive go "down" in the layout text/rows, corresponding to negative Z in world space.
        for (int z = 0; z < maxRows; z++)
        {
            // The end of cell Z is the start of cell Z + the size needed for cell Z (max object depth + spacing)
            cumulativeZOffsets[z + 1] = cumulativeZOffsets[z] + maxCellHeights[z] + gridSpacing.z;
        }

        // Note: The cumulative offsets now represent the positions of the boundaries *between* cells.
        // cumulativeXOffsets[x] is the world X position of the left edge of column x's allocated space (relative to grid origin).
        // cumulativeXOffsets[x+1] is the world X position of the right edge of column x's allocated space.
        // cumulativeZOffsets[z] is the world Z position of the top edge of row z's allocated space (relative to grid origin).
        // cumulativeZOffsets[z+1] is the world Z position of the bottom edge of row z's allocated space.


        // --- Step 3: Instantiate and position prefabs ---
        EditorUtility.DisplayProgressBar("World Generator", "Generating grid...", 0.7f);

        // The world origin for the grid is the parent object's position.
        Vector3 gridWorldOrigin = parentObject.position;

        // Calculate total grid dimensions (from the start of the first cell to the end of the last cell's spacing)
        float totalGridWidth = cumulativeXOffsets[maxCols];
        float totalGridDepth = cumulativeZOffsets[maxRows];

        // Calculate the offset to center the grid if desired. For now, placing the top-left corner of the grid at parent origin.
        // To center the grid at the parent, you'd add -totalGridWidth/2 to X and +totalGridDepth/2 to Z (or -totalGridDepth/2 depending on Z direction)
        // Vector3 centeringOffset = new Vector3(-totalGridWidth / 2f, 0, totalGridDepth / 2f); // Example centering offset


        for (int z = 0; z < maxRows; z++)
        {
            string line = lines[z];
            for (int x = 0; x < line.Length; x++)
            {
                if (x >= maxCols) continue; // Handle lines shorter than maxCols

                char character = line[x];
                if (!int.TryParse(character.ToString(), out int id))
                {
                    continue; // Skip non-numeric characters
                }

                // Check if a valid mapping exists for this ID
                if (prefabMapDict.TryGetValue(id, out PrefabMapping mapping) && mapping.prefab != null)
                {
                    // Calculate the center position of the current cell's *allocated space* (x, z) in parent's local space
                    // This is the midpoint between the start and end cumulative offsets for this cell.
                    float cellCenterX_local = (cumulativeXOffsets[x] + cumulativeXOffsets[x + 1]) / 2f;
                    float cellCenterZ_local = (cumulativeZOffsets[z] + cumulativeZOffsets[z + 1]) / 2f; // Z increases as row index increases

                    // The target world position for the *center of the bounds* of the object.
                    // The Z axis in the grid layout usually maps to the negative Z world axis to go "forward" or "down" from a top-down view.
                    // Adjust the Z coordinate to reflect this and apply parent transform.
                    Vector3 targetBoundsCenterWorld = parentObject.TransformPoint(new Vector3(cellCenterX_local, 0, -cellCenterZ_local));
                    // Assuming Y=0 is the base plane for placement for all objects.

                    // Retrieve the pre-calculated offset from pivot to bounds center for this prefab ID
                    Vector3 pivotToCenterWorldOffset = pivotToCenterWorldOffsets.ContainsKey(id) ? pivotToCenterWorldOffsets[id] : Vector3.zero;


                    // Calculate the required world position for the object's *pivot*
                    // Target Pivot World Position = (Target Bounds Center World Position) - (World Offset from Pivot to Bounds Center)
                    // The pivotToCenterWorldOffset was calculated when the temp instance had its rotation applied.
                    // When we subtract it here, it correctly offsets the target bounds center back to where the pivot should be in world space
                    // relative to the parent's rotation.
                    Vector3 targetPivotWorldPosition = targetBoundsCenterWorld - pivotToCenterWorldOffset;


                    // Instantiate the prefab as a child of the parent object
                    GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(mapping.prefab, parentObject);

                    // Set the object's world position first
                    instance.transform.position = targetPivotWorldPosition;

                    // Set local rotation
                    instance.transform.localRotation = Quaternion.Euler(mapping.rotationEuler);


                    // instance.name = $"{mapping.prefab.name}_{x}_{z}"; // Optional: Name instances
                }
            }
        }

        EditorUtility.ClearProgressBar();
        Debug.Log($"World Generator: Grid generation complete! Generated {parentObject.childCount} objects.");
    }

    // Helper to get the total bounds of a GameObject including all its children's renderers/colliders
    // Returns Bounds with zero size if no renderers or colliders are found.
    private Bounds GetBounds(GameObject obj)
    {
        // Start with bounds at the object's position, size 0
        Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);
        bool gotBounds = false;

        // Get bounds from all renderers first
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            foreach (Renderer r in renderers)
            {
                if (r.enabled) // Only consider enabled renderers
                {
                    if (!gotBounds)
                    {
                        bounds = r.bounds; // Start with the first renderer's bounds
                        gotBounds = true;
                    }
                    else
                    {
                        bounds.Encapsulate(r.bounds); // Encapsulate subsequent renderer bounds
                    }
                }
            }
        }

        // If no renderers provided bounds, try getting bounds from enabled colliders
        if (!gotBounds)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>();
            if (colliders.Length > 0)
            {
                foreach (Collider c in colliders)
                {
                    if (c.enabled) // Only consider enabled colliders
                    {
                        if (!gotBounds)
                        {
                            bounds = c.bounds; // Start with the first collider's bounds
                            gotBounds = true;
                        }
                        else
                        {
                            bounds.Encapsulate(c.bounds); // Encapsulate subsequent collider bounds
                        }
                    }
                }
            }
        }

        // Return the calculated bounds. If gotBounds is false, size will be Vector3.zero.
        return bounds;
    }


    private void ClearGeneratedObjects()
    {
        if (parentObject == null)
        {
            // If parent was null and we created "GeneratedWorld", clear its children.
            // If parent was assigned but is now null (e.g., deleted), this check prevents error.
            GameObject defaultParent = GameObject.Find("GeneratedWorld");
            if (defaultParent != null)
            {
                parentObject = defaultParent.transform; // Temporarily set parentObject to clear
                Debug.LogWarning("World Generator: Parent object was null, attempting to clear children of 'GeneratedWorld'.");
            }
            else
            {
                Debug.LogWarning("World Generator: No designated parent object and no 'GeneratedWorld' object found. Nothing to clear.");
                return;
            }
        }

        // Store children to array before destroying, as DestroyImmediate happens immediately in Editor
        GameObject[] children = new GameObject[parentObject.childCount];
        for (int i = 0; i < parentObject.childCount; i++)
        {
            children[i] = parentObject.GetChild(i).gameObject;
        }

        // Destroy all children
        foreach (GameObject child in children)
        {
            if (child != null) // Check if object wasn't somehow destroyed already
            {
                DestroyImmediate(child);
            }
        }

        Debug.Log($"World Generator: Cleared {children.Length} generated objects under '{parentObject.name}'.");

        // If we temporarily set parentObject, reset it back to null
        if (parentObject.name == "GeneratedWorld" && children.Length > 0) // Only reset if children were cleared from the default object
        {
            // parentObject = null; // Decided against this, keep the reference if it exists
        }
    }
}