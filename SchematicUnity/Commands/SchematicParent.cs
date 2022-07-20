namespace SchematicUnity.Commands
{
    using System;
    using CommandSystem;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SchematicParent : ParentCommand, IUsageProvider
    {
        public SchematicParent() =>
            LoadGeneratedCommands();

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new LoadCommand());
            RegisterCommand(new UnloadCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format("Please specify a valid subcommand!");
            return false;
        }

        public override string Command => "scheme";
        public override string[] Aliases => new string[] { "schematic" };
        public override string Description => "Schematic Management";
        public string[] Usage => new string[]
        {
            "load %name%",
            "unload %id%"
        };
    }
}