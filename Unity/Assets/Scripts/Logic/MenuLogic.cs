namespace DrivingGameV2 {
    public static class MenuLogic {
        public static bool IsMenuOpened = false;

        public static void Init() {
            SceneObjects.Menu.SetActive(false);
            SceneObjects.OpenMenuButton.onClick.AddListener(() => {
                SceneObjects.Menu.SetActive(true);
                IsMenuOpened = true;
            });
            SceneObjects.CloseMenuButton.onClick.AddListener(() => {
                SceneObjects.Menu.SetActive(false);
                IsMenuOpened = false;
            });
        }
    }
}
