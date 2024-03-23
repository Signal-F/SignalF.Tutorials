﻿using System.Runtime.Versioning;
using SignalF.Configuration;
using SignalF.Controller.Configuration;
using SignalF.Datamodel.Configuration;

namespace Tutorial.Configuration;

public class SystemConfiguration : ISystemConfiguration
{
    private readonly Func<ISignalFConfiguration> _configurationFactory;

    public SystemConfiguration(Func<ISignalFConfiguration> configurationFactory)
    {
        _configurationFactory = configurationFactory;
    }

    [SupportedOSPlatform("linux")]
    public void Configure(IControllerConfiguration configuration)
    {
        var signalFConfiguration = _configurationFactory();

        signalFConfiguration
            .AddDevices()
            //.AddDataOutputs()
            //.AddDataOutputSenders()
            .AddTasks()
            .AddTaskMappings();
        
        signalFConfiguration.Build(configuration);
    }
}