namespace Simionic.Core
{
    public class FlapsRange
    {
        public string[] Markings { get; init; } = new string[6] { "UP", null, null, null, null, "F" };
        public int?[] Positions { get; init; } = new int?[6] { 0, null, null, null, null, 100 };
    }
}
