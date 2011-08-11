namespace mazio
{
    public interface Command
    {
        void execute();
        void unexecute();
        string Name { get; } 
    }
}
