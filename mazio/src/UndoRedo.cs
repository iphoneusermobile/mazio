using System.Collections.Generic;
using System.Diagnostics;

namespace mazio
{
    public class UndoRedo
    {
        private readonly Stack<Command> undoCommands = new Stack<Command>();
        private readonly Stack<Command> redoCommands = new Stack<Command>();
        private readonly ScreenshotEditor mazio;

        public UndoRedo(ScreenshotEditor mazio)
        {
            this.mazio = mazio;
        }

        public string LastUndo { get { return undoCommands.Count > 0 ? undoCommands.Peek().Name : "None"; } }
        public string LastRedo { get { return redoCommands.Count > 0 ? redoCommands.Peek().Name : "None"; } }

        public void undo()
        {
            if (undoCommands.Count <= 0) {
                return;
            }
            Command c = undoCommands.Pop();
            c.unexecute();
            redoCommands.Push(c);
            mazio.notifyUndoRedoState(undoCommands.Count == 0, redoCommands.Count == 0);
            //diag();
        }

        public void redo()
        {
            if (redoCommands.Count <= 0) {
                return;
            }
            Command c = redoCommands.Pop();
            c.execute();
            undoCommands.Push(c);
            mazio.notifyUndoRedoState(undoCommands.Count == 0, redoCommands.Count == 0);
            //diag();
        }

        public void addCommand(Command c)
        {
            undoCommands.Push(c);
            redoCommands.Clear();
            mazio.notifyUndoRedoState(undoCommands.Count == 0, redoCommands.Count == 0);
            //diag();
        }

        private void diag()
        {
            Debug.WriteLine("undo commands:");
            foreach (Command c in undoCommands) Debug.WriteLine("\t" + c);
            Debug.WriteLine("redo commands:");
            foreach (Command c in redoCommands) Debug.WriteLine("\t" + c);
            Debug.WriteLine("");
        }
    }
}
