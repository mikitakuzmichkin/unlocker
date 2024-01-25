using System.Collections.Generic;
using Dainty.DI;
using Newtonsoft.Json;
using PuzzleUnlocker.Data;
using PuzzleUnlocker.Gameplay;
using PuzzleUnlocker.Gameplay.Level;
using UnityEngine;

namespace PuzzleUnlocker.Tutorials
{
    public class TutorialDataJsonProvider : MonoBehaviour, ITutorialDataProvider
    {
        [SerializeField] private TextAsset _json;

        private TutorialData[] _tutorialsData;

        private void Awake()
        {
            _tutorialsData = JsonConvert.DeserializeObject<TutorialData[]>(_json.text,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        public int GetLength()
        {
            return _tutorialsData.Length;
        }

        public int GetLevelsBefore(int curLevel, bool isTutorial)
        {
            var count = 0;
            foreach (var tutorialData in _tutorialsData)
            {
                var tutorialSteps = tutorialData.TutorialSteps;
                if (tutorialSteps == null)
                {
                    continue;
                }

                for (var i = 0; i < tutorialSteps.Count; i++)
                {
                    var step = tutorialSteps[i];
                    if (step.LevelData != null && step.ShowBeforeLevel != null && step.ShowBeforeLevel <= curLevel)
                    {
                        count++;
                    }
                }
            }

            if (isTutorial)
            {
                return count - 1;
            }

            return count;
        }

        public bool TryGetTutorialsData(int level, out List<TutorialStep> steps, out EGameType type)
        {
            var dataProvider = ProjectContext.GetInstance<IPuDataProvider>();

            steps = new List<TutorialStep>();
            foreach (var tutorialData in _tutorialsData)
            {
                var tutorialSteps = tutorialData.TutorialSteps;
                if (tutorialSteps == null)
                {
                    continue;
                }

                var lastStep = dataProvider.GetTutorialLevelId(tutorialData.Stage);

                for (var i = 0; i < tutorialSteps.Count; i++)
                {
                    var step = tutorialSteps[i];
                    if (step.ShowBeforeLevel == level)
                    {
                        if (lastStep >= i)
                        {
                            continue;
                        }

                        type = tutorialData.Stage;
                        do
                        {
                            steps.Add(step);

                            if (i + 1 == tutorialSteps.Count)
                            {
                                return true;
                            }

                            i++;
                            step = tutorialSteps[i];
                        } while (step.ShowBeforeLevel == null);

                        return true;
                    }
                }
            }

            type = EGameType.Number;
            return false;
        }
    }
}