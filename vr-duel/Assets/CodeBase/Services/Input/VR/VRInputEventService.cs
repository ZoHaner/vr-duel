using System;

namespace CodeBase.Services.Input.VR
{
    public class VRInputEventService : IInputEventService
    {
        bool IInputEventService.InputChanged { get; set; }

        public Action AttackButtonPressed { get; set; }
       
        public bool IsAttackButtonPressed()
        {
            throw new NotImplementedException();
        }
    }
}