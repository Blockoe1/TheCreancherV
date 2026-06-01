using UnityEngine;

public static class InvertColorToggle
{
    public static void EnableInvert()
    {
        Shader.EnableKeyword("_INVERTENABLED");
    }

    public static void DisableInvert()
    {
        Shader.DisableKeyword("_INVERTENABLED");
    }
}
