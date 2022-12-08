namespace Fishbot.Model.Fishing
{
    public struct FishRecord
    {
        public FishRecord(string label)
        {
            Label = label;
            Probability = default;
            Message = default;
        }

        public readonly string Label;
        public int Probability;
        public string Message;
    }
}