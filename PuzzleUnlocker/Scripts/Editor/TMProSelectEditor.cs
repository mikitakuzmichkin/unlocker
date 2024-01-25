using TMPro.EditorUtilities;

namespace PuzzleUnlocker.Editor
{
    public class TMProSelectEditor : TMP_SDFShaderGUI
    {
        private static bool s_Custom = true;
        protected override void DoGUI()
        {
            base.DoGUI();
            s_Custom = BeginPanel("Custom", s_Custom);
            if (s_Custom)
            {
                DoColor("_SelectColor", "Select Color");
                DoFloat("_Speed", "Speed");
            }

            EndPanel();
        }
    }
}