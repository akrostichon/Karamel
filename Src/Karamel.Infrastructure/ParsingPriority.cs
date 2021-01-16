namespace Karamel.Infrastructure
{

    /// <summary>
    /// parsing priority - which criterion gets the highest priority
    /// </summary>
    public enum ParsingPriority
    {
        TagBeforeFileName,
        FileNameBeforeTag
    }
}