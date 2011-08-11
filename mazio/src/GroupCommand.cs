using System.Collections.Generic;

namespace mazio
{
    public class GroupCommand : Command
    {
        private readonly LinkedList<Command> commands = new LinkedList<Command>();

        private readonly string name;

        public GroupCommand(string name)
        {
            this.name = name;
        }

        public void add(Command c)
        {
            commands.AddLast(c);
        }

        public virtual void execute()
        {
            LinkedListNode<Command> c = commands.First;
            do
            {
                c.Value.execute();
                c = c.Next;
            }
            while (c != null);
        }

        public virtual void unexecute()
        {
            LinkedListNode<Command> c = commands.Last;
            do
            {
                c.Value.unexecute();
                c = c.Previous;
            }
            while (c != null);
        }

        public override string ToString()
        {
            string str = name;
            foreach (Command c in commands)
                str += "\r\n\t\t" + c.Name;

            return str;
        }

        public string Name { get { return name; } }
    }
}
