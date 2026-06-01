using UnityEngine;

namespace FoolsBrand
{
    public class ColorRegionData
    {

    }

    [System.Serializable]
    public class ColorRegions
    {
        public static string Region0VarName = "_REGION_0";
        public static string Region1VarName = "_REGION_1";
        public static string Region2VarName = "_REGION_2";
        public static string Region3VarName = "_REGION_3";
        public static string Region4VarName = "_REGION_4";
        public static string Region5VarName = "_REGION_5";

        private Color region0 = new Color(0, 0, 0);
        private Color region1 = new Color(60, 0, 0);
        private Color region2 = new Color(120, 0, 0);
        private Color region3 = new Color(180, 0, 0);
        private Color region4 = new Color(240, 0, 0);
        private Color region5 = new Color(300, 0, 0);

        public ColorRegions(Color r0, Color r1, Color r2, Color r3, Color r4, Color r5)
        {
            region0 = r0;
            region1 = r1;
            region2 = r2;
            region3 = r3;
            region4 = r4;
            region5 = r5;
        }

        #region GS
        public Color R0 { get => region0; set => region0 = value; }
        public Color R1 { get => region1; set => region1 = value; }
        public Color R2 { get => region2; set => region2 = value; }
        public Color R3 { get => region3; set => region3 = value; }
        public Color R4 { get => region4; set => region4 = value; }
        public Color R5 { get => region5; set => region5 = value; }
        #endregion

        public Color[] ToArray()
            { return new Color[]{ region0, region1, region2, region3, region4, region5 }; }
    }

    public struct MeshColorRegions
    {
        private MeshRenderer renderer;
        private ColorRegions regions;

        public MeshColorRegions(MeshRenderer r, ColorRegions cr)
        {
            renderer = r;
            regions = cr;
        }

        #region GS
        public MeshRenderer Renderer { get => renderer; set => renderer = value; }
        public ColorRegions Regions { get => regions; set => regions = value; }
        #endregion
    }
}
