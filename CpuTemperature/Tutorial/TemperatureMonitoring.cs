﻿using SignalF.Controller;
using SignalF.Controller.Calculator;
using SignalF.Controller.Signals;
using SignalF.Controller.Signals.SignalProcessor;
using SignalF.Datamodel.Calculation;
using SignalF.Datamodel.Configuration;

namespace Tutorial
{
    public class TemperatureMonitoring : SignalProcessor<ICalculatorConfiguration>, ICalculator
    {
        private const double WarnLevel = 70.0;
        private const double CriticalLevel = 100.0;
        private const int NumberOfOutputSignals = 4;

        private const int OkId = 0;
        private const int WarningId = 1;
        private const int AlarmId = 2;
        private const int FanId = 3;

        private Signal _temperature;
        private readonly Signal[] _outputSignals = new Signal[4];

        public TemperatureMonitoring(ISignalHub signalHub, ILogger<SignalProcessor<ICalculatorConfiguration>> logger) 
            : base(signalHub, logger)
        {
        }

        protected override void OnConfigure(ICalculatorConfiguration configuration)
        {
            base.OnConfigure(configuration);

            foreach (var (signalName, index) in SignalNameToIndexMapping)
            {
                switch (signalName)
                {
                    case "Temperature":
                    {
                        _temperature = new Signal(index, double.NaN, null);
                        break;
                    }
                    case "OK":
                    {
                        _outputSignals[OkId] = new Signal(index, double.NaN, null);
                        break;
                    }
                    case "Warning":
                    {
                        _outputSignals[WarningId] = new Signal(index, double.NaN, null);
                        break;
                    }
                    case "Alarm":
                    {
                        _outputSignals[AlarmId] = new Signal(index, double.NaN, null);
                        break;
                    }
                    case "Fan":
                    {
                        _outputSignals[FanId] = new Signal(index, double.NaN, null);
                        break;
                    }
                    default:
                    {
                        throw new ControllerException($"Unsupported signal {signalName}");
                    }
                }
            }
        }

        public override void Execute(ETaskType taskType)
        {
            switch (taskType)
            {
                case ETaskType.Init:
                {
                    break;
                }
                case ETaskType.Calculate:
                {
                    var timestamp = SignalHub.GetTimestamp();
                        _outputSignals[OkId] = _outputSignals[OkId] with{Value = _temperature.Value < WarnLevel ? 1.0 : 0.0 } 

                    break;
                }
                case ETaskType.Read:
                {
                    _temperature = SignalHub.GetSignal(_temperature.SignalIndex);
                    break;
                }
                case ETaskType.Write:
                {
                    for (var i = 0; i < NumberOfOutputSignals; i++)
                    {
                        WriteSignal(_outputSignals[i]);
                    }

                    break;
                }
                case ETaskType.Exit:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(taskType), taskType, "Task type is not supported.");
            }

        }

        private void InitializeDevice()
        {
            throw new NotImplementedException();
        }
    }
}
