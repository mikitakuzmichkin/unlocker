using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PuzzleUnlocker.Gameplay
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EGameType
    {
        Number,
        Cube,
        Color,
        Figure
    }
}