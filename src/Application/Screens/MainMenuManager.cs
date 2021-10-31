using Appalachia.Core.Behaviours;
using Appalachia.Core.Extensions;
using Appalachia.Globals.Application.Screens.Fading;
using UnityEngine;

namespace Appalachia.Globals.Application.Screens
{
    public class MainMenuManager : SingletonAppalachiaBehaviour<MainMenuManager>
    {
        private CanvasFadeManager _menuCanvasFader;

        protected override void OnAwake()
        {
            if (_menuCanvasFader == null)
            {
                _menuCanvasFader = FindObjectOfType<CanvasFadeManager>();
            }
        }

        public void NewGame()
        {
            
        }

        public void LoadGame()
        {
            
        }

        public void Settings()
        {
            
        }

        public void Quit()
        {
            
        }
    }
}
