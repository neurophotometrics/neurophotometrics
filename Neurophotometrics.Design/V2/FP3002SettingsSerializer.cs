using Neurophotometrics.Design.V2.Converters;
using Neurophotometrics.V2.Definitions;
using System;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace Neurophotometrics.Design.V2
{
    public sealed class FP3002SettingsSerializer : IDisposable
    {
        private readonly FP3002Settings Settings;
        private XmlWriter Writer;
        private XmlReader Reader;
        private readonly string FileName;

        public FP3002SettingsSerializer(string fileName, FP3002Settings settings)
        {
            Settings = settings;
            Writer = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true });
        }
        public FP3002SettingsSerializer(string fileName)
        {
            Settings = new FP3002Settings();
            FileName = fileName;
            Reader = XmlReader.Create(FileName);
        }

        internal FP3002Settings GetSettings()
        {
            return Settings;
        }

        internal void TryWriteSettings()
        {
            try
            {
                WriteSettings();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
            finally
            {
                Dispose();
            }
        }

        internal void TryReadSettings()
        {
            try
            {
                ReadSettings();
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError(ex);
            }
            finally
            {
                Dispose();
            }
        }

        private void WriteSettings()
        {
            if (Writer == null)
                return;

            Writer.WriteStartDocument();
            Writer.WriteStartElement("Settings");
            WriteDeviceInformation();
            WriteConfigSettings();
            WriteEmissionSettings();
            WriteExcitationSettings();
            WriteOptoStimulationSettings();
            WriteDigitalIOsSettings();
            Writer.WriteEndElement();
            Writer.WriteEndDocument();
        }

        private void ReadSettings()
        {
            if (Reader == null)
                return;

            var isOldXML = GetIsOldXML();

            Reader.Dispose();
            Reader = XmlReader.Create(FileName);

            if (isOldXML)
                ReadOldXML();
            else
                ReadNewXML();
        }

        private bool GetIsOldXML()
        {
            Reader.ReadToFollowing("DeviceInformation");

            if (Reader.EOF)
                return true;
            else
                return false;
        }

        private void ReadNewXML()
        {
            ReadConfigSettings();
            ReadEmissionSettings();
            ReadExcitationSettings();
            ReadOptoStimulationSettings();
            ReadDigitalIOsSettings();
        }

        private void ReadOldXML()
        {
            Reader.ReadToFollowing(nameof(Settings.ClockSynchronizer));
            var clockSynchronizerAsString = Reader.ReadElementContentAsString();
            Settings.ClockSynchronizer = Enum.GetValues(typeof(ClockSynchronizerConfiguration)).Cast<ClockSynchronizerConfiguration>()
                .First(clockSynchronizer => clockSynchronizer.ToString() == clockSynchronizerAsString);

            Reader.ReadToFollowing(nameof(Settings.Output1Routing));
            var output1RoutingAsString = Reader.ReadElementContentAsString();
            Settings.Output1Routing = Enum.GetValues(typeof(DigitalOutputRouting)).Cast<DigitalOutputRouting>()
                .First(digitalOutputRoute => digitalOutputRoute.ToString() == output1RoutingAsString);

            Reader.ReadToFollowing(nameof(Settings.ScreenBrightness));
            Settings.ScreenBrightness = byte.Parse(Reader.ReadElementContentAsString());

            Reader.ReadToFollowing("FrameRate");
            Settings.TriggerPeriod = TriggerPeriodConverter.ConvertFreqStringToPeriodMicros(Reader.ReadElementContentAsString());

            Reader.ReadToFollowing("TriggerState");
            var triggerSequence = new byte[32];
            if (Reader.ReadToDescendant("Trigger"))
            {
                var index = 0;
                do
                {
                    triggerSequence[index++] = TriggerSequenceConverter.ConvertOldStringToByte(Reader.ReadElementContentAsString());
                    Settings.TriggerSequenceLength++;
                }
                while (Reader.ReadToNextSibling("Trigger"));
            }
            Settings.TriggerSequence = triggerSequence;

            Reader.ReadToFollowing("L415");
            Settings.LedPowers.L415 = LedPowerConverter.ConvertStringToRegisterValue(Reader.ReadElementContentAsString());
            Reader.ReadToFollowing("L470");
            Settings.LedPowers.L470 = LedPowerConverter.ConvertStringToRegisterValue(Reader.ReadElementContentAsString());
            Reader.ReadToFollowing("L560");
            Settings.LedPowers.L560 = LedPowerConverter.ConvertStringToRegisterValue(Reader.ReadElementContentAsString());

            Reader.ReadToFollowing("LaserAmplitude");
            Settings.PulseTrain.LaserAmplitude = LaserAmplitudeConverter.ConvertStringToRegisterValue(Reader.ReadElementContentAsString());
            Reader.ReadToFollowing("PulseFrequency");
            Settings.PulseTrain.StimPeriod = StimPeriodConverter.ConvertFreqStringToPeriodMillis(Reader.ReadElementContentAsString());
            Reader.ReadToFollowing("PulseWidth");
            Settings.PulseTrain.StimOn = StimOnConverter.ConvertPulseWidthStringToRegisterValue(Reader.ReadElementContentAsString());
            Reader.ReadToFollowing("PulseCount");
            Settings.PulseTrain.StimReps = ushort.Parse(Reader.ReadElementContentAsString());

            Reader.ReadToFollowing(nameof(Settings.DigitalOutput0));
            var digitalOutput0AsString = Reader.ReadElementContentAsString();
            Settings.DigitalOutput0 = Enum.GetValues(typeof(DigitalOutputConfiguration)).Cast<DigitalOutputConfiguration>()
                .First(digitalOutputConfig => digitalOutputConfig.ToString() == digitalOutput0AsString);

            var digitalInputConfigs = Enum.GetValues(typeof(DigitalInputConfiguration)).Cast<DigitalInputConfiguration>();
            Reader.ReadToFollowing(nameof(Settings.DigitalInput0));
            var digitalInput0AsString = Reader.ReadElementContentAsString();
            if (digitalInputConfigs.Any(digitalInputConfig => digitalInputConfig.ToString() == digitalInput0AsString))
                Settings.DigitalInput0 = digitalInputConfigs.First(digitalInputConfig => digitalInputConfig.ToString() == digitalInput0AsString);
            else
                Settings.DigitalInput0 = DigitalInputConfiguration.None;

            Reader.ReadToFollowing(nameof(Settings.DigitalInput1));
            var digitalInput1AsString = Reader.ReadElementContentAsString();
            if (digitalInputConfigs.Any(digitalInputConfig => digitalInputConfig.ToString() == digitalInput1AsString))
                Settings.DigitalInput1 = digitalInputConfigs.First(digitalInputConfig => digitalInputConfig.ToString() == digitalInput1AsString);
            else
                Settings.DigitalInput1 = DigitalInputConfiguration.None;

            Reader.ReadToFollowing("Regions");
            if (Reader.ReadToFollowing("RotatedRect"))
            {
                do
                {
                    Reader.ReadToFollowing("X");
                    var centerX = Reader.ReadElementContentAsFloat();
                    Reader.ReadToFollowing("Y");
                    var centerY = Reader.ReadElementContentAsFloat();
                    Reader.ReadToFollowing("Width");
                    var width = Reader.ReadElementContentAsFloat();
                    Reader.ReadToFollowing("Height");
                    var height = Reader.ReadElementContentAsFloat();

                    var xPos = (int)(centerX - width / 2.0f);
                    var yPos = (int)(centerY - height / 2.0f);

                    var photometryRegion = new PhotometryRegion()
                    {
                        Rectangle = new Rectangle(xPos, yPos, (int)width, (int)height),
                        Index = Settings.Regions.Count
                    };
                    Settings.Regions.Add(photometryRegion);
                }
                while (Reader.ReadToFollowing("RotatedRect"));
            }
        }

        private void WriteDeviceInformation()
        {
            Writer.WriteStartElement("DeviceInformation");
            WriteSetting(nameof(Settings.Id), Settings.Id);
            WriteSetting("HardwareVersion", Settings.HardwareVersionHigh.ToString() + "." + Settings.HardwareVersionLow.ToString());
            WriteSetting("FirmwareVersion", Settings.FirmwareVersionHigh.ToString() + "." + Settings.FirmwareVersionLow.ToString());
            WriteSetting(nameof(Settings.SerialNumber), Settings.SerialNumber.ToString());
            Writer.WriteEndElement();
        }

        private void WriteConfigSettings()
        {
            Writer.WriteStartElement("Configuration");
            WriteSetting(nameof(Settings.ClockSynchronizer), Settings.ClockSynchronizer.ToString());
            WriteSetting(nameof(Settings.ScreenBrightness), Settings.ScreenBrightness.ToString());
            Writer.WriteEndElement();
        }

        private void ReadConfigSettings()
        {
            Reader.ReadToFollowing(nameof(Settings.ClockSynchronizer));
            var clockSynchronizerAsString = Reader.ReadElementContentAsString();
            Settings.ClockSynchronizer = Enum.GetValues(typeof(ClockSynchronizerConfiguration)).Cast<ClockSynchronizerConfiguration>()
                .First(clockSynchronizer => clockSynchronizer.ToString() == clockSynchronizerAsString);
            Reader.ReadToFollowing(nameof(Settings.ScreenBrightness));
            Settings.ScreenBrightness = byte.Parse(Reader.ReadElementContentAsString());
        }

        private void WriteEmissionSettings()
        {
            Writer.WriteStartElement("Emission");
            WriteRegions();
            Writer.WriteEndElement();
        }

        private void ReadEmissionSettings()
        {
            Reader.ReadToFollowing("Emission");
            ReadRegions();
        }

        private void WriteExcitationSettings()
        {
            Writer.WriteStartElement("Excitation");
            WriteTriggerPeriod();
            WriteSequence();
            WriteLEDPowers();
            Writer.WriteEndElement();
        }

        private void ReadExcitationSettings()
        {
            Reader.ReadToFollowing("Excitation");
            ReadTriggerPeriod();
            ReadSequence();
            ReadLEDPowers();
        }

        private void WriteOptoStimulationSettings()
        {
            Writer.WriteStartElement("OptoStimulation");
            WriteSetting(nameof(Settings.PulseTrain.LaserWavelength), Settings.PulseTrain.LaserWavelength.ToString());
            WriteLaserAmplitude();
            WriteStimPeriod();
            WriteStimOn();
            WriteSetting(nameof(Settings.PulseTrain.StimReps), Settings.PulseTrain.StimReps.ToString());
            Writer.WriteEndElement();
        }

        private void ReadOptoStimulationSettings()
        {
            Settings.PulseTrain = new PulseTrain();
            Reader.ReadToFollowing("OptoStimulation");
            Reader.ReadToFollowing(nameof(Settings.PulseTrain.LaserWavelength));
            Settings.PulseTrain.LaserWavelength = ushort.Parse(Reader.ReadElementContentAsString());
            ReadLaserAmplitude();
            ReadStimPeriod();
            ReadStimOn();
            Reader.ReadToFollowing(nameof(Settings.PulseTrain.StimReps));
            Settings.PulseTrain.StimReps = ushort.Parse(Reader.ReadElementContentAsString());
        }

        private void WriteDigitalIOsSettings()
        {
            Writer.WriteStartElement("DigitalIOs");
            WriteSetting(nameof(Settings.DigitalOutput0), Settings.DigitalOutput0.ToString());
            WriteSetting(nameof(Settings.Output1Routing), Settings.Output1Routing.ToString());
            WriteSetting(nameof(Settings.DigitalInput0), Settings.DigitalInput0.ToString());
            WriteSetting(nameof(Settings.DigitalInput1), Settings.DigitalInput1.ToString());
            Writer.WriteEndElement();
        }

        private void ReadDigitalIOsSettings()
        {
            Reader.ReadToFollowing("DigitalIOs");

            Reader.ReadToFollowing(nameof(Settings.DigitalOutput0));
            var digitalOutput0AsString = Reader.ReadElementContentAsString();
            Settings.DigitalOutput0 = Enum.GetValues(typeof(DigitalOutputConfiguration)).Cast<DigitalOutputConfiguration>()
                .First(digitalOutputConfig => digitalOutputConfig.ToString() == digitalOutput0AsString);

            Reader.ReadToFollowing(nameof(Settings.Output1Routing));
            var output1RoutingAsString = Reader.ReadElementContentAsString();
            Settings.Output1Routing = Enum.GetValues(typeof(DigitalOutputRouting)).Cast<DigitalOutputRouting>()
                .First(digitalOutputRoute => digitalOutputRoute.ToString() == output1RoutingAsString);

            Reader.ReadToFollowing(nameof(Settings.DigitalInput0));
            var digitalInput0AsString = Reader.ReadElementContentAsString();
            Settings.DigitalInput0 = Enum.GetValues(typeof(DigitalInputConfiguration)).Cast<DigitalInputConfiguration>()
                .First(digitalInputConfig => digitalInputConfig.ToString() == digitalInput0AsString);

            Reader.ReadToFollowing(nameof(Settings.DigitalInput1));
            var digitalInput1AsString = Reader.ReadElementContentAsString();
            Settings.DigitalInput1 = Enum.GetValues(typeof(DigitalInputConfiguration)).Cast<DigitalInputConfiguration>()
                .First(digitalInputConfig => digitalInputConfig.ToString() == digitalInput1AsString);
        }

        private void WriteRegions()
        {
            if (Settings.Regions == null) return;
            foreach (var region in Settings.Regions)
            {
                Writer.WriteStartElement("Region");
                Writer.WriteAttributeString("X", region.Rectangle.X.ToString());
                Writer.WriteAttributeString("Y", region.Rectangle.Y.ToString());
                Writer.WriteAttributeString("Width", region.Rectangle.Width.ToString());
                Writer.WriteAttributeString("Height", region.Rectangle.Height.ToString());
                Writer.WriteString(region.Name);
                Writer.WriteEndElement();
            }
        }

        private void ReadRegions()
        {
            if (Reader.ReadToDescendant("Region"))
            {
                do
                    ReadRegion();
                while (Reader.ReadToNextSibling("Region"));
            }
        }

        private void ReadRegion()
        {
            var photometryRegion = new PhotometryRegion()
            {
                Rectangle = new Rectangle(int.Parse(Reader[0]), int.Parse(Reader[1]), int.Parse(Reader[2]), int.Parse(Reader[3]))
            };
            photometryRegion.Name = Reader.ReadElementContentAsString();
            photometryRegion.Index = Settings.Regions.Count;
            Settings.Regions.Add(photometryRegion);
        }

        private void WriteTriggerPeriod()
        {
            Writer.WriteStartElement("TriggerPeriod");
            var FPS = TriggerPeriodConverter.ConvertPeriodMicrosToFreqString(Settings.TriggerPeriod, 3);
            Writer.WriteAttributeString("Reg", Settings.TriggerPeriod.ToString());
            Writer.WriteString(FPS);
            Writer.WriteEndElement();
        }

        private void ReadTriggerPeriod()
        {
            Reader.ReadToFollowing("TriggerPeriod");
            Settings.TriggerPeriod = TriggerPeriodConverter.ConvertStringToPeriodMicros(Reader[0]);
        }

        private void WriteSequence()
        {
            Writer.WriteStartElement("Sequence");
            var frameFlags = TriggerSequenceConverter.ConvertByteArrToFrameFlagsArr(Settings.TriggerSequence, Settings.TriggerSequenceLength);
            for (var i = 0; i < frameFlags.Length; i++)
            {
                Writer.WriteStartElement("LED");
                Writer.WriteAttributeString("Index", i.ToString());
                Writer.WriteAttributeString("Flag", ((ushort)frameFlags[i]).ToString());
                Writer.WriteString(frameFlags[i].ToString());
                Writer.WriteEndElement();
            }
            Writer.WriteEndElement();
        }

        private void ReadSequence()
        {
            Reader.ReadToFollowing("Sequence");
            var triggerSequence = new byte[32];
            if (Reader.ReadToDescendant("LED"))
            {
                triggerSequence[int.Parse(Reader[0])] = byte.Parse(Reader[1]);
                Settings.TriggerSequenceLength = 1;
                while (Reader.ReadToNextSibling("LED"))
                {
                    triggerSequence[int.Parse(Reader[0])] = byte.Parse(Reader[1]);
                    Settings.TriggerSequenceLength++;
                }
            }
            Settings.TriggerSequence = triggerSequence;
        }

        private void WriteLEDPowers()
        {
            Writer.WriteStartElement("Power");
            var L415Power = LedPowerConverter.ConvertRegisterValueToPercentString(Settings.LedPowers.L415, 3);
            Writer.WriteStartElement("L415");
            Writer.WriteAttributeString("Reg", Settings.LedPowers.L415.ToString());
            Writer.WriteString(L415Power);
            Writer.WriteEndElement();

            Writer.WriteStartElement("L470");
            var L470Power = LedPowerConverter.ConvertRegisterValueToPercentString(Settings.LedPowers.L470, 3);
            Writer.WriteAttributeString("Reg", Settings.LedPowers.L470.ToString());
            Writer.WriteString(L470Power);
            Writer.WriteEndElement();

            Writer.WriteStartElement("L560");
            var L560Power = LedPowerConverter.ConvertRegisterValueToPercentString(Settings.LedPowers.L560, 3);
            Writer.WriteAttributeString("Reg", Settings.LedPowers.L560.ToString());
            Writer.WriteString(L560Power);
            Writer.WriteEndElement();

            Writer.WriteEndElement();
        }

        private void ReadLEDPowers()
        {
            Settings.LedPowers = new LedPowers();
            Reader.ReadToFollowing("L415");
            Settings.LedPowers.L415 = LedPowerConverter.ConvertStringToRegisterValue(Reader[0]);
            Reader.ReadToFollowing("L470");
            Settings.LedPowers.L470 = LedPowerConverter.ConvertStringToRegisterValue(Reader[0]);
            Reader.ReadToFollowing("L560");
            Settings.LedPowers.L560 = LedPowerConverter.ConvertStringToRegisterValue(Reader[0]);
        }

        private void WriteLaserAmplitude()
        {
            Writer.WriteStartElement(nameof(Settings.PulseTrain.LaserAmplitude));
            var LaserAmp = LaserAmplitudeConverter.ConvertRegisterValueToPercentString(Settings.PulseTrain.LaserAmplitude, 3);
            Writer.WriteAttributeString("Reg", Settings.PulseTrain.LaserAmplitude.ToString());
            Writer.WriteString(LaserAmp);
            Writer.WriteEndElement();
        }

        private void ReadLaserAmplitude()
        {
            Reader.ReadToFollowing(nameof(Settings.PulseTrain.LaserAmplitude));
            Settings.PulseTrain.LaserAmplitude = LaserAmplitudeConverter.ConvertStringToRegisterValue(Reader[0]);
        }

        private void WriteStimPeriod()
        {
            Writer.WriteStartElement(nameof(Settings.PulseTrain.StimPeriod));
            var StimPeriod = StimPeriodConverter.ConvertPeriodMillisToFreqString(Settings.PulseTrain.StimPeriod, 3);
            Writer.WriteAttributeString("Reg", Settings.PulseTrain.StimPeriod.ToString());
            Writer.WriteString(StimPeriod);
            Writer.WriteEndElement();
        }

        private void ReadStimPeriod()
        {
            Reader.ReadToFollowing(nameof(Settings.PulseTrain.StimPeriod));
            Settings.PulseTrain.StimPeriod = StimPeriodConverter.ConvertStringToPeriodMillis(Reader[0]);
        }

        private void WriteStimOn()
        {
            Writer.WriteStartElement(nameof(Settings.PulseTrain.StimOn));
            var StimOn = StimOnConverter.ConvertRegisterValueToPulseWidthString(Settings.PulseTrain.StimOn);
            Writer.WriteAttributeString("Reg", Settings.PulseTrain.StimOn.ToString());
            Writer.WriteString(StimOn);
            Writer.WriteEndElement();
        }

        private void ReadStimOn()
        {
            Reader.ReadToFollowing(nameof(Settings.PulseTrain.StimOn));
            Settings.PulseTrain.StimOn = StimOnConverter.ConvertPulseWidthStringToRegisterValue(Reader[0]);
        }

        private void WriteSetting(string settingName, string settingValue)
        {
            Writer.WriteStartElement(settingName);
            Writer.WriteString(settingValue);
            Writer.WriteEndElement();
        }

        public void Dispose()
        {
            if (Writer != null)
            {
                Writer.Dispose();
                Writer = null;
            }
            if (Reader != null)
            {
                Reader.Dispose();
                Reader = null;
            }
        }
    }
}