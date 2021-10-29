using System;
using Appalachia.Core.Scriptables;
using Sirenix.OdinInspector;

namespace Appalachia.Globals.Application
{
    [Serializable]
    [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Boxed)]
    [HideLabel]
    [LabelWidth(0)]
    public class
        SceneBootloadDataCollection : MetadataLookupBase<SceneBootloadDataCollection, SceneBootloadData>
    {
        [PropertyOrder(60)] public SceneBootloadData game;
        [PropertyOrder(70)] public SceneBootloadData inGameMenu;
        [PropertyOrder(20)] public SceneBootloadData loadingScreen;
        [PropertyOrder(10)] public SceneBootloadData mainMenu;
        [PropertyOrder(50)] public SceneBootloadData pauseScreen;
        [PropertyOrder(00)] public SceneBootloadData splashScreen;

        public SceneBootloadData GetByArea(ApplicationStateArea area)
        {
            switch (area)
            {
                case ApplicationStateArea.SplashScreen:
                    return splashScreen;
                case ApplicationStateArea.MainMenu:
                    return mainMenu;
                case ApplicationStateArea.LoadingScreen:
                    return loadingScreen;
                case ApplicationStateArea.Game:
                    return game;
                case ApplicationStateArea.InGameMenu:
                    return inGameMenu;
                case ApplicationStateArea.PauseScreen:
                    return pauseScreen;
                default:
                    throw new ArgumentOutOfRangeException(nameof(area), area, null);
            }
        }

        protected override void RegisterNecessaryInstances()
        {
            if (splashScreen == null)
            {
                splashScreen = SceneBootloadData.LoadOrCreateNew(nameof(splashScreen));
            }

            if (mainMenu == null)
            {
                mainMenu = SceneBootloadData.LoadOrCreateNew(nameof(mainMenu));
            }

            if (loadingScreen == null)
            {
                loadingScreen = SceneBootloadData.LoadOrCreateNew(nameof(loadingScreen));
            }

            if (game == null)
            {
                game = SceneBootloadData.LoadOrCreateNew(nameof(game));
            }

            if (inGameMenu == null)
            {
                inGameMenu = SceneBootloadData.LoadOrCreateNew(nameof(inGameMenu));
            }

            if (pauseScreen == null)
            {
                pauseScreen = SceneBootloadData.LoadOrCreateNew(nameof(pauseScreen));
            }
        }
    }
}
