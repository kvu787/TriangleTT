namespace DrivingGameV2 {
    public static class UnityNullExtensions {
        // This is a Unity version of the "??=" operator because "is null" is not equivalent to "== null" for some Unity stuff and the "??=" operator uses "is null".
        public static void AssignIfNull<T>(ref T field, T value) where T : UnityEngine.Object {
            if (field == null) // Uses Unity's overloaded null check
            {
                field = value;
            }
        }
    }
}
