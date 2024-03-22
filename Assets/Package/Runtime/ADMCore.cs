﻿namespace ADM
{
    public static class ADMCore
    {
        public static void StartSystems()
        {
            ServiceProvider.CollectServiceDefinitions();
            LoadModules();
        }

        private static void LoadModules()
        {
            var modules = ServiceProvider.GetAll<ICoreModule>();
            foreach (var module in modules)
                module.Load();
        }
    }
}
