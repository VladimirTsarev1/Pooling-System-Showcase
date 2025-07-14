using Showcase.MainMenu.Background;
using UnityEngine;

namespace Showcase.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private BackgroundController backgroundController;

        private void Start()
        {
            backgroundController.ActivateBackground();
        }
    }
}