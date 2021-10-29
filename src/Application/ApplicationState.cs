using System;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;

namespace Appalachia.Globals.Application
{
    [Serializable]
    public class ApplicationState
    {
        public ApplicationState()
        {
            currentArea = ApplicationStateArea.None;
            nextArea = ApplicationStateArea.SplashScreen;
            game = new ApplicationSubstate(ApplicationStateArea.Game);
            inGameMenu = new ApplicationSubstate(ApplicationStateArea.InGameMenu);
            loadingScreen = new ApplicationSubstate(ApplicationStateArea.LoadingScreen);
            mainMenu = new ApplicationSubstate(ApplicationStateArea.MainMenu);
            pauseScreen = new ApplicationSubstate(ApplicationStateArea.PauseScreen);
            splashScreen = new ApplicationSubstate(ApplicationStateArea.SplashScreen);
        }

        [HorizontalGroup("A"), SmartLabel, ReadOnly]
        public ApplicationStateArea currentArea;

        [HorizontalGroup("A"), SmartLabel, ReadOnly]
        public ApplicationStateArea nextArea;

        [PropertySpace]
        [InlineProperty, HideLabel]
        public ApplicationSubstate game;

        [InlineProperty, HideLabel]
        public ApplicationSubstate inGameMenu;

        [InlineProperty, HideLabel]
        public ApplicationSubstate loadingScreen;

        [InlineProperty, HideLabel]
        public ApplicationSubstate mainMenu;

        [InlineProperty, HideLabel]
        public ApplicationSubstate pauseScreen;

        [InlineProperty, HideLabel]
        public ApplicationSubstate splashScreen;

        public ApplicationSubstate current
        {
            get
            {
                switch (currentArea)
                {
                    case ApplicationStateArea.None:
                        return default;
                    case ApplicationStateArea.SplashScreen:
                        return splashScreen;
                    case ApplicationStateArea.MainMenu:
                        return mainMenu;
                    case ApplicationStateArea.LoadingScreen:
                        return loadingScreen;
                    case ApplicationStateArea.InGameMenu:
                        return inGameMenu;
                    case ApplicationStateArea.PauseScreen:
                        return pauseScreen;
                    case ApplicationStateArea.Game:
                        return game;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public ApplicationSubstate next
        {
            get
            {
                switch (nextArea)
                {
                    case ApplicationStateArea.None:
                        return default;
                    case ApplicationStateArea.SplashScreen:
                        return splashScreen;
                    case ApplicationStateArea.MainMenu:
                        return mainMenu;
                    case ApplicationStateArea.LoadingScreen:
                        return loadingScreen;
                    case ApplicationStateArea.InGameMenu:
                        return inGameMenu;
                    case ApplicationStateArea.PauseScreen:
                        return pauseScreen;
                    case ApplicationStateArea.Game:
                        return game;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
