﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class EdgeSwipeConfigurationService : IConfigurationService
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\EdgeSwipe";
        private const string STATE_VALUE_NAME = "state";

        private const string EDGE_UI_KEY_NAME = @"HKLM\Software\Policies\Microsoft\Windows\EdgeUI";

        private const string ALLOW_EDGE_SWIPE_VALUE_NAME = "AllowEdgeSwipe";


        private readonly ConfigurationStore _edgeSwipeConfigurationService;
        
        public EdgeSwipeConfigurationService(
            [FromKeyedServices("EdgeSwipe")] ConfigurationStore edgeSwipeConfigurationService)
        {
            _edgeSwipeConfigurationService = edgeSwipeConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(EDGE_UI_KEY_NAME, ALLOW_EDGE_SWIPE_VALUE_NAME, 00000000, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _edgeSwipeConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(EDGE_UI_KEY_NAME, ALLOW_EDGE_SWIPE_VALUE_NAME);
            RegistryHelper.DeleteKey(ATLAS_STORE_KEY_NAME);

            _edgeSwipeConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
