using PigeonCoopToolkit.Generic.Editor;
using UnityEditor;

namespace PigeonCoopToolkit.Effects.Trails.Editor
{
    [InitializeOnLoad]
    public class BetterTrailsIntroDialogue : InfoDialogueSpawner
    {
        private static BetterTrailsIntroDialogue _instance;
        static BetterTrailsIntroDialogue() 
        {
            _instance = new BetterTrailsIntroDialogue();
            _instance.SetParams(
                "Better Trails",
                "PCTK/Effects/Trails/banner",
                new Generic.VersionInformation("Better Trails", 1, 5, 0),
                "/PigeonCoopToolkit/__Effects (Trails) Examples/Pigeon Coop Toolkit - Effects (Trails).pdf",
                "16076");
        }

        [MenuItem("Window/Pigeon Coop Toolkit/Better Trails/About")]
        public static void About()
        {
            _instance.LaunchAbout();
        }
    }
}
