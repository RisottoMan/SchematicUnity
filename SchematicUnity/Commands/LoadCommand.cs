namespace SchematicUnity.Commands
{
    using System;
    using System.Linq;
    using System.IO;
    using CommandSystem;
    using Qurre;
    using API;

    [CommandHandler(typeof(SchematicParent))]
    internal class LoadCommand : ICommand, IUsageProvider
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

            var schemeName = arguments.FirstElement();
            var pathToScheme = Path.Combine(PathToSchemesFolder, schemeName);
            if (!schemeName.EndsWith(".json")) pathToScheme = string.Concat(pathToScheme, ".json");

            try
            {
                if (!File.Exists(pathToScheme))
                {
                    response = $"Schematic file named \"{arguments.ElementAt(0)}\" not found!";
                    return false;
                }

                var commandSender = sender as CommandSender;
                var schemeID = SchematicManager.LoadSchematic(pathToScheme).ID;

                Log.Info($"Player \"{commandSender.Nickname}\" ({commandSender.SenderId}) has successfully loaded schematic \"{pathToScheme}\" (ID - {schemeID})");
                response = $"Schematic \"{schemeName}\" was loaded successfully! (ID - {schemeID})\nFor unload use \"schematic unload {schemeID}\"";
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"An unexpected exception occurred while loading schematic \"{pathToScheme}\":\n{ex}");
                response = $"An unexpected exception occurred while loading schematic \"{pathToScheme}\"!";
                return false;
            }
        }

        public string Command => "load";
        public string[] Aliases => new string[] { "l" };
        public string Description => "Load schematic file";
        public string[] Usage => new string[]
        {
            "%name%"
        };
    }
}