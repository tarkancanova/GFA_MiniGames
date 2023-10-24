using UnityEngine;
using UnityEngine.SceneManagement;

namespace GFA.MiniGames.Data.Games
{
    [CreateAssetMenu(menuName = "Games/Endless Shooter", fileName = "EndlessShooter", order = 0)]
    public class EndlessShooter : MiniGame
    {
        [SerializeField]
        private string _sceneName;
        
        protected override void OnBegin()
        {
            var operation = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            

            operation.completed += asyncOperation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
            };
        }

        protected override void OnTick()
        {
        }

        protected override void OnEnd()
        {
            SceneManager.UnloadSceneAsync(_sceneName);
        }
    }
}
