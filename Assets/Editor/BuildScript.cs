using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class BuildScript
{
    private static void DoBuild(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
    {
        EditorUserBuildSettings.selectedBuildTargetGroup = buildTargetGroup;
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

        string buildDir = "";

        if (buildTargetGroup == BuildTargetGroup.WebGL)
        {
            buildDir = "./builds";
        }
        else if (buildTargetGroup == BuildTargetGroup.Standalone)
        {
            buildDir = "./builds/game.exe";
        }

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenePaths,
            locationPathName = buildDir,
            target = buildTarget,
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
        DoBuild(BuildTargetGroup.WebGL, BuildTarget.WebGL);
    }

    public static void BuildWebGL()
    {
        // Build EmbeddedLinux ARM64 Unity player
        DoBuild(BuildTargetGroup.WebGL, BuildTarget.WebGL);
    }

    public static void BuildWindows()
    {
        // Build EmbeddedLinux ARM64 Unity player
        DoBuild(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
    }
}
