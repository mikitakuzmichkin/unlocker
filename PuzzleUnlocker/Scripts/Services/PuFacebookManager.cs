using System;
using System.Threading.Tasks;
using Facebook.Unity;
using UnityEngine;

namespace PuzzleUnlocker.Services
{
    public static class PuFacebookManager
    {
        public static void Init(Action<bool> onComplete = null)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                onComplete?.Invoke(true);
                return;
            }

            FB.Init(() =>
            {
                var result = InitCallback();
                onComplete?.Invoke(result);
            });
        }

        public static Task<bool> InitAsync()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                return Task.FromResult(true);
            }

            var completionSource = new TaskCompletionSource<bool>();
            FB.Init(() =>
            {
                var result = InitCallback();
                completionSource.SetResult(result);
            });
            return completionSource.Task;
        }

        private static bool InitCallback()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                return true;
            }

            Debug.LogWarning("Failed to Initialize the Facebook SDK");
            return false;
        }
    }
}