using System;

namespace TriangleTT {
    public static class TimeSpanExtensions {
        public static string ToLapTime(this TimeSpan self) {
            return self.ToString(@"mm\:ss\.fff");
        }
    }
}
