using NewOtherGame.Combat;
using NewOtherGame.Core;
using NewOtherGame.Enemies;
using NewOtherGame.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class NewOtherGamePrototypeBuilder
{
    private const string EnemyLayerName = "Enemy";
    private const string EnemyTagName = "Enemy";

    [MenuItem("Tools/New Other Game/Build Prototype Scene")]
    public static void BuildPrototypeScene()
    {
        EnsureTag(EnemyTagName);
        int enemyLayer = EnsureLayer(EnemyLayerName);

        EnsureMainCamera();
        CreateGameManager();

        Projectile projectilePrefab = EnsureProjectilePrefab();
        EnemyData enemyData = EnsureEnemyDataAsset();

        GameObject player = CreatePlayer(enemyLayer, projectilePrefab);
        CreateEnemy(enemyLayer, enemyData, new Vector2(3f, 0f));
        CreateEnemy(enemyLayer, enemyData, new Vector2(-3f, 2f));

        Selection.activeGameObject = player;
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Prototype scene built. Press Play to verify movement/auto-attack.");
    }

    private static void EnsureMainCamera()
    {
        if (Object.FindObjectOfType<Camera>() != null)
        {
            return;
        }

        var cameraGo = new GameObject("Main Camera");
        var cam = cameraGo.AddComponent<Camera>();
        cameraGo.tag = "MainCamera";
        cam.orthographic = true;
        cam.orthographicSize = 6f;
        cameraGo.transform.position = new Vector3(0f, 0f, -10f);
    }

    private static void CreateGameManager()
    {
        GameObject existing = GameObject.Find("GameManager");
        if (existing != null)
        {
            return;
        }

        var go = new GameObject("GameManager");
        go.AddComponent<GameManager>();
        go.AddComponent<AutoStartRun>();
    }

    private static GameObject CreatePlayer(int enemyLayer, Projectile projectilePrefab)
    {
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayer != null)
        {
            return existingPlayer;
        }

        var player = new GameObject("Player");
        player.tag = "Player";

        var sr = player.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.2f, 0.8f, 1f, 1f);

        var rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        player.AddComponent<CircleCollider2D>();
        player.AddComponent<PlayerStats>();
        player.AddComponent<PlayerController>();

        var attack = player.AddComponent<AutoAttackController>();
        SerializedObject attackSo = new SerializedObject(attack);
        attackSo.FindProperty("projectilePrefab").objectReferenceValue = projectilePrefab;
        attackSo.FindProperty("enemyLayer").intValue = 1 << enemyLayer;
        attackSo.ApplyModifiedPropertiesWithoutUndo();

        player.transform.position = Vector3.zero;
        return player;
    }

    private static void CreateEnemy(int enemyLayer, EnemyData enemyData, Vector2 position)
    {
        var enemy = new GameObject("Enemy");
        enemy.tag = EnemyTagName;
        enemy.layer = enemyLayer;

        var sr = enemy.AddComponent<SpriteRenderer>();
        sr.color = new Color(1f, 0.4f, 0.4f, 1f);

        var rb = enemy.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        enemy.AddComponent<CircleCollider2D>();
        var controller = enemy.AddComponent<EnemyController>();

        SerializedObject enemySo = new SerializedObject(controller);
        enemySo.FindProperty("enemyData").objectReferenceValue = enemyData;
        enemySo.ApplyModifiedPropertiesWithoutUndo();

        enemy.transform.position = position;
    }

    private static Projectile EnsureProjectilePrefab()
    {
        const string prefabPath = "Assets/Prefabs/Projectile.prefab";
        Projectile prefab = AssetDatabase.LoadAssetAtPath<Projectile>(prefabPath);
        if (prefab != null)
        {
            return prefab;
        }

        EnsureFolder("Assets/Prefabs");

        var go = new GameObject("Projectile");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.color = Color.yellow;

        var col = go.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        var projectile = go.AddComponent<Projectile>();
        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        Object.DestroyImmediate(go);

        return AssetDatabase.LoadAssetAtPath<Projectile>(prefabPath);
    }

    private static EnemyData EnsureEnemyDataAsset()
    {
        const string assetPath = "Assets/Data/Enemy_Default.asset";
        EnemyData data = AssetDatabase.LoadAssetAtPath<EnemyData>(assetPath);
        if (data != null)
        {
            return data;
        }

        EnsureFolder("Assets/Data");

        data = ScriptableObject.CreateInstance<EnemyData>();
        data.enemyId = "default";
        data.baseHp = 30f;
        data.moveSpeed = 2.2f;
        data.contactDamage = 8f;
        data.attackCooldown = 1f;
        data.xpDrop = 3;

        AssetDatabase.CreateAsset(data, assetPath);
        AssetDatabase.SaveAssets();
        return data;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        string[] split = path.Split('/');
        string current = split[0];
        for (int i = 1; i < split.Length; i++)
        {
            string next = $"{current}/{split[i]}";
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, split[i]);
            }
            current = next;
        }
    }

    private static void EnsureTag(string tag)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        bool exists = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty entry = tagsProp.GetArrayElementAtIndex(i);
            if (entry.stringValue == tag)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
            tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tag;
            tagManager.ApplyModifiedProperties();
        }
    }

    private static int EnsureLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        for (int i = 8; i < 32; i++)
        {
            SerializedProperty layerProp = layersProp.GetArrayElementAtIndex(i);
            if (layerProp.stringValue == layerName)
            {
                return i;
            }
        }

        for (int i = 8; i < 32; i++)
        {
            SerializedProperty layerProp = layersProp.GetArrayElementAtIndex(i);
            if (string.IsNullOrEmpty(layerProp.stringValue))
            {
                layerProp.stringValue = layerName;
                tagManager.ApplyModifiedProperties();
                return i;
            }
        }

        Debug.LogWarning($"No empty layer slot found. Using Default layer for {layerName}.");
        return 0;
    }
}
