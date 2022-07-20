namespace SchematicUnity.Commands
{
    using System;
    using System.Linq;
    using System.IO;
    using CommandSystem;
    using Qurre;
    using API;

    [CommandHandler(typeof(SchematicParent))]
    internal class UnloadCommand : ICommand, IUsageProvider
    {
        internal static readonly string PathToSchemesFolder = Path.Combine(PluginManager.PluginsDirectory, "Schemes");

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.FacilityManagement))
            {
                response = "Insufficient permissions to run this command!";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "Specify the name of the schematic to load.";
                return false;
            }

            if (!uint.TryParse(arguments.FirstElement(), out var schemeId))
            {
                response = $"\"{arguments.FirstElement()}\" is not a ULong Integer!";
                return false;
            }

            if (!SchematicManager.LoadedSchemes.TryGetValue(schemeId, out var scheme) || scheme == null)
            {
                response = $"Unable to find uploaded schematic with id \"{schemeId}\"";
                return false;
            }

            try
            {
                scheme.Unload();
                var commandSender = sender as CommandSender;

                Log.Info($"Player \"{commandSender.Nickname}\" ({commandSender.SenderId}) has successfully unloaded schematic \"{schemeId}\"");
                response = $"Schematic with id \"{schemeId}\" was unloaded successfully!";
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"An unexpected exception occurred while unloading schematic with id \"{schemeId}\":\n{ex}");
                response = $"An unexpected exception occurred while unloading schematic with id \"{schemeId}\"!";
                return false;
            }
        }

        public string Command => "unload";
        public string[] Aliases => new string[] { "u", "ul", "unl" };
        public string Description => "Unload schematic file";
        public string[] Usage => new string[]
        {
            "%id%"
        };
    }
}