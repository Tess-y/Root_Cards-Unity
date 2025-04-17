using UnityEngine;

namespace RootCore {
    public static class Colour
    {
        public static Color New(double r, double g, double b, double a = 1) {
            return new Color((float)r, (float)g, (float)b, (float)a);
        }
    }
}
