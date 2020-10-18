using UnityEngine.SceneManagement;

namespace Assets.Scritps.Presentation
{
    static class SceneModifier 
    {
        internal static void LoadSceneBySceneName(string nextSceneName)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
