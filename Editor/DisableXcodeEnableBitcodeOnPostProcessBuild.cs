using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Kogane.Internal
{
    /// <summary>
    /// iOS ビルド完了時に Xcode プロジェクトの「Enable Bitcode」を「No」にするエディタ拡張
    /// https://support.unity.com/hc/en-us/articles/207942813-How-can-I-disable-Bitcode-support-
    /// </summary>
    internal static class DisableXcodeEnableBitcodeOnPostProcessBuild
    {
        //================================================================================
        // 関数(static)
        //================================================================================
        /// <summary>
        /// ビルド完了時に呼び出されます
        /// </summary>
        [PostProcessBuild]
        private static void OnPostProcessBuild
        (
            BuildTarget buildTarget,
            string      pathToBuiltProject
        )
        {
            if ( buildTarget != BuildTarget.iOS ) return;

            var projectPath = PBXProject.GetPBXProjectPath( pathToBuiltProject );
            var project     = new PBXProject();

            const string name  = "ENABLE_BITCODE";
            const string value = "NO";

            project.ReadFromFile( projectPath );
            project.SetBuildProperty( project.ProjectGuid(), name, value );
            project.SetBuildProperty( project.GetUnityMainTargetGuid(), name, value );
            project.SetBuildProperty( project.TargetGuidByName( PBXProject.GetUnityTestTargetName() ), name, value );
            project.SetBuildProperty( project.GetUnityFrameworkTargetGuid(), name, value );
            project.WriteToFile( projectPath );
        }
    }
}