﻿using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class LockScreenConfigurationService : IConfigurationService
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\LockScreen";
        private const string STATE_VALUE_NAME = "state";

        private const string PERSONALIZATION_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization";

        private const string NO_LOCK_SCREEN_VALUE_NAME = "NoLockScreen";

        private readonly ConfigurationStore _lockScreenConfigurationStore;

        public LockScreenConfigurationService(
            [FromKeyedServices("LockScreen")] ConfigurationStore lockScreenConfigurationStore)
        {
            _lockScreenConfigurationStore = lockScreenConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(PERSONALIZATION_KEY_NAME, NO_LOCK_SCREEN_VALUE_NAME, 00000001, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.DeleteKey(ATLAS_STORE_KEY_NAME);

            _lockScreenConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(PERSONALIZATION_KEY_NAME, NO_LOCK_SCREEN_VALUE_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _lockScreenConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
