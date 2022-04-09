namespace CodeBase.Services.Editor
{
#if UNITY_EDITOR

    public class EditorCloseApplicationService : ICloseApplicationService
    {
        public void Execute()
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

#endif
}