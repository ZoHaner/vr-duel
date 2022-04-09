using UnityEngine;

namespace CodeBase.Services
{
    public class StandaloneCloseApplicationService : ICloseApplicationService
    {
        public void Execute()
        {
            Application.Quit();
        }
    }
}