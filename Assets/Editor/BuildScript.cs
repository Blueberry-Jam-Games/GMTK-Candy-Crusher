using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class BuildScript
{
    private static void BuildWebGL()
    {
        EditorUserBuildSettings.selectedBuildTargetGroup = BuildTargetGroup.WebGL;
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string[] scenePaths = new string[scenes.Length];
        for(int i = 0; i < scenePaths.Length; i++)
        {
            Debug.Log("Adding scene " + scenes[i].path);
            scenePaths[i] = scenes[i].path;
        }

        // string[] scenePaths = new string[]
        // {
        //     "Assets/Scenes/SampleScene.unity"
        // };

        Debug.Log("Started we have logs");

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenePaths,
            locationPathName = "C:/Programs/WebGLBuild",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        Debug.Log("Options created");

        BuildReport report = BuildPipeline.BuildPlayer(options);

        Debug.Log("End of build");

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build successful - Build written to {options.locationPathName}");
        }
        else if (report.summary.result == BuildResult.Failed)
        {
            Debug.LogError($"Build failed");
            EditorApplication.Exit(1);
        }
    }

    // This function will be called from the build process
    public static void Build()
    {
        // Build EmbeddedLinux ARM64 Unity player
        BuildWebGL();
    }
}
