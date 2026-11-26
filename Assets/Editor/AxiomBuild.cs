using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class AxiomBuild
{
    public static void PerformAndroidBuild()
    {
        AxiomProjectSetup.EnsureProjectScenes();

        EditorBuildSettingsScene[] enabledScenes =
            EditorBuildSettings.scenes.Where(scene => scene.enabled).ToArray();

        if (enabledScenes.Length != 40)
            throw new Exception("AXIOM build expects exactly 40 enabled level scenes.");

        foreach (EditorBuildSettingsScene scene in enabledScenes)
        {
            if (!File.Exists(scene.path))
                throw new FileNotFoundException("Missing scene: " + scene.path);
        }

        string outputDirectory = "build";
        Directory.CreateDirectory(outputDirectory);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = enabledScenes.Select(scene => scene.path).ToArray(),
            locationPathName = Path.Combine(outputDirectory, "AXIOM-ADVENTURE.apk"),
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        PlayerSettings.productName = "AXIOM-ADVENTURE";
        PlayerSettings.companyName = "AXIOM Studio";
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new Exception(
                "AXIOM Android build failed: " +
                report.summary.result + ", errors: " +
                report.summary.totalErrors);
        }

        Debug.Log(
            "AXIOM APK created: " + options.locationPathName +
            " (" + report.summary.totalSize + " bytes)");
    }
}
