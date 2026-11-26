using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class AxiomProjectSetup
{
    private const string SceneFolder = "Assets/Scenes";

    static AxiomProjectSetup()
    {
        EditorApplication.delayCall += EnsureProjectScenes;
    }

    [MenuItem("AXIOM/Generate 40 Level Scenes")]
    public static void EnsureProjectScenes()
    {
        if (EditorApplication.isCompiling || EditorApplication.isPlaying)
            return;

        if (!Directory.Exists(SceneFolder))
            Directory.CreateDirectory(SceneFolder);

        List<EditorBuildSettingsScene> buildScenes =
            new List<EditorBuildSettingsScene>();

        for (int level = 1; level <= 40; level++)
        {
            string path = SceneFolder + "/Level_" + level + ".unity";

            if (!File.Exists(path))
                CreateLevelScene(level, path);

            buildScenes.Add(new EditorBuildSettingsScene(path, true));
        }

        EditorBuildSettings.scenes = buildScenes.ToArray();
        PlayerSettings.companyName = "AXIOM Studio";
        PlayerSettings.productName = "AXIOM-ADVENTURE";
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;

        // Use Unity 6.6's generated Gradle templates. The legacy custom template
        // was created for the older 2022.3 project and is intentionally disabled.
        PlayerSettings.Android.useCustomMainGradleTemplate = false;

        AssetDatabase.SaveAssets();
        Debug.Log("AXIOM: 40 level scenes and Android build list are ready.");
    }

    private static void CreateLevelScene(int level, string path)
    {
        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene, NewSceneMode.Single);

        CreateCube("Ground", new Vector3(0f, -0.5f, 12f),
            new Vector3(18f, 1f, 36f), new Color(0.22f, 0.34f, 0.25f));

        for (int i = 0; i < 5; i++)
        {
            float z = 5f + i * 5f;
            float x = (i % 2 == 0 ? -1f : 1f) * (1.5f + (level % 4) * 0.3f);
            CreateCube("ParkourPlatform_" + (i + 1),
                new Vector3(x, 0.5f + (i % 2) * 0.6f, z),
                new Vector3(3f, 1f, 2.5f),
                new Color(0.42f, 0.38f, 0.29f));
        }

        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 1.2f, 0f);
        Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
        CharacterController controller = player.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.45f;
        player.AddComponent<PlayerController>();
        player.AddComponent<PlayerHealth>();

        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.transform.SetParent(player.transform);
        cameraObject.transform.localPosition = new Vector3(0f, 0.65f, 0f);
        cameraObject.transform.localRotation = Quaternion.identity;
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.fieldOfView = 70f;
        cameraObject.AddComponent<AudioListener>();

        GameObject goal = CreateCube("LevelGoal",
            new Vector3(0f, 1.1f, 29f), new Vector3(2f, 2.2f, 2f),
            new Color(0.15f, 0.85f, 0.35f));
        Collider goalCollider = goal.GetComponent<Collider>();
        goalCollider.isTrigger = true;
        goal.AddComponent<LevelGoalTrigger>();

        GameObject manager = new GameObject("LevelManager");
        manager.AddComponent<LevelManager>();

        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        RenderSettings.ambientLight = new Color(0.55f, 0.55f, 0.55f);

        EditorSceneManager.SaveScene(scene, path);
    }

    private static GameObject CreateCube(
        string objectName, Vector3 position, Vector3 scale, Color color)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = objectName;
        cube.transform.position = position;
        cube.transform.localScale = scale;

        Renderer renderer = cube.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = color;
        renderer.sharedMaterial = material;
        return cube;
    }
}
