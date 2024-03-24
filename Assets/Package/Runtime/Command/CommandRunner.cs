using System.Collections.Generic;
using System.Linq;
using static ADM.Assert;

namespace ADM
{
    // TODO - Add support for command arguments
    public class CommandRunner
    {
        private Dictionary<string, ICommand> m_commands = new();

        public void Add(string name, ICommand command)
        {
            ASSERT_FALSE(m_commands.ContainsKey(name), 
                $"Command for {name} already exists");
            ASSERT_FALSE(m_commands.Values.Any(i => i.GetType().Equals(command.GetType())),
                $"{command.GetType().Name} already registered");

            m_commands.Add(name, command);
        }

        public void Execute(string name)
        {
            ASSERT_TRUE(m_commands.ContainsKey(name),
                $"Unrecognized command {name}");

            m_commands[name].Execute();
        }
    }
}
