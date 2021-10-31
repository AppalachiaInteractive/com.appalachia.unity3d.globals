using Appalachia.Core.Behaviours;
using Appalachia.Globals.Application.Screens.Fading;

namespace Appalachia.Globals.Application.Areas
{
    public class AreaManager<T> : SingletonAppalachiaBehaviour<T>, IAreaManager
        where T : AreaManager<T>
    {
        protected CanvasFadeManager _menuCanvasFader;

        protected override void OnAwake()
        {
            if (_menuCanvasFader == null)
            {
                _menuCanvasFader = FindObjectOfType<CanvasFadeManager>();
            }
        }
    }
}
