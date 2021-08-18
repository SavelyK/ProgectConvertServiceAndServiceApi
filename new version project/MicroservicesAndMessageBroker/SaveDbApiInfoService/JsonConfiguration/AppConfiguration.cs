using Calabonga.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaveDbApiInfoService
{
    public class AppConfiguration :Configuration<AppSettings>
    {
        public AppConfiguration(IConfigurationSerializer serializer) : base(serializer)
        {

        }
        public override string FileName => "appSettings.Json";

    }
}
