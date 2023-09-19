using Bonsai.Harp;
using Neurophotometrics.Definitions;
using Neurophotometrics.Design.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neurophotometrics.Design.Editors
{
    public partial class SettingsControl : UserControl
    {
        public FP3002Settings Settings;
        private KeyFilterType CurrentKeyFilter = KeyFilterType.None;

        public SettingsControl()
        {
            InitializeComponent();
            Settings = new FP3002Settings();

            Settings_PropGrid.SelectedObject = Settings;
            Settings_PropGrid.ExpandAllGridItems();

            Settings.LEDPowers_ValueChanged += RegSettingChanged;
            Settings.PulseTrain_ValueChanged += RegSettingChanged;
            Settings_PropGrid.SelectedGridItemChanged += Settings_PropGrid_SelectedGridItemChanged;
            AddKeyFiltering();
        }

        internal void ReadRegions(List<PhotometryRegion> regions)
        {
            Settings.Regions = regions;
        }

        private static readonly FrameFlags[] DefaultTrigSeq = new FrameFlags[] { FrameFlags.L470 | FrameFlags.L560 | FrameFlags.L415 };
        private const ushort DefaultTriggerPeriod = 25000;
        private const ushort DefaultTriggerPeriodUpdateOutputs = 24500;
        private const ushort DefaultTriggerTime = 24000;

        public IEnumerable<HarpMessage> GenCaliSettings()
        {
            //yield return HarpCommand.WriteByte(Registers.Start, (byte)(AcquisitionModes.StopPhotometry | AcquisitionModes.StopExternalCamera));

            yield return HarpCommand.WriteUInt16(Registers.DacL415, 0);
            yield return HarpCommand.WriteUInt16(Registers.DacL470, 0);
            yield return HarpCommand.WriteUInt16(Registers.DacL560, 0);
            yield return HarpCommand.WriteByte(Registers.Out0Conf, (byte)DigitalOutputConfiguration.Software);
            yield return HarpCommand.WriteByte(Registers.Out1Conf, (byte)DigitalOutputConfiguration.Software);
            yield return HarpCommand.WriteByte(Registers.In0Conf, (byte)DigitalInputConfiguration.None);
            yield return HarpCommand.WriteByte(Registers.In1Conf, (byte)DigitalInputConfiguration.None);
            yield return HarpCommand.WriteByte(Registers.TriggerState, (byte[])TypeDescriptor.GetProperties(Settings).Find(nameof(Settings.TriggerSequence), false).Converter.ConvertTo(null, null, DefaultTrigSeq, typeof(byte[])));
            yield return HarpCommand.WriteByte(Registers.TriggerState, TriggerSequenceConverter);
            yield return HarpCommand.WriteByte(Registers.TriggerStateLength, (byte)DefaultTrigSeq.Length);
            yield return HarpCommand.WriteUInt16(Registers.TriggerPeriod, DefaultTriggerPeriod);
            yield return HarpCommand.WriteUInt16(Registers.TriggerTime, DefaultTriggerTime);
            yield return HarpCommand.WriteUInt16(Registers.TriggerTimeUpdateOutputs, DefaultTriggerPeriodUpdateOutputs);
            //yield return HarpCommand.WriteUInt16(Registers.TriggerLaserOn, Settings.TriggerLaserOn);
            //yield return HarpCommand.WriteUInt16(Registers.TriggerLaserOff, Settings.TriggerLaserOff);

            yield return HarpCommand.WriteByte(Registers.Start, (byte)AcquisitionModes.StartPhotometry);
        }

        

        public event RegSettingChangedHandler SettingChanged;

        public RegSettingChangedArgs GetSettings()
        {
            return new RegSettingChangedArgs(nameof(Settings), Settings);
        }

        internal void RefreshProps()
        {
            Settings_PropGrid.Refresh();
        }



        /// <summary>
        /// Reads the <see cref="HarpMessage"/> coming from the <see cref="device"/>
        /// and updates the properties within <see cref="SystemSettings"/> accordingly. 
        /// </summary>
        /// <param name="message"><see cref="HarpMessage"/> from the <see cref="device"/> to read.</param>
        internal void ReadRegister(HarpMessage message)
        {
            try
            {
                switch (message.Address)
                {
                    case Registers.WhoAmI:
                        Settings.WhoAmI = message.GetPayloadUInt16();
                        break;
                    case Registers.HardwareVersionHigh:
                        Settings.HardwareVersionHigh = message.GetPayloadByte();
                        break;
                    case Registers.HardwareVersionLow:
                        Settings.HardwareVersionLow = message.GetPayloadByte();
                        break;
                    case Registers.FirmwareVersionHigh:
                        Settings.FirmwareVersionHigh = message.GetPayloadByte();
                        break;
                    case Registers.FirmwareVersionLow:
                        Settings.FirmwareVersionLow = message.GetPayloadByte();
                        break;
                    case Registers.Config:
                        Settings.Config = message.GetPayloadUInt16();
                        break;
                    case Registers.DacL415:
                        Settings.LEDPowers.L415 = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.LEDPowers.L415), Settings.LEDPowers.L415));
                        break;
                    case Registers.DacL470:
                        Settings.LEDPowers.L470 = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.LEDPowers.L470), Settings.LEDPowers.L470));
                        break;
                    case Registers.DacL560:
                        Settings.LEDPowers.L560 = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.LEDPowers.L560), Settings.LEDPowers.L560));
                        break;
                    case Registers.DacLaser:
                        Settings.PulseTrain.LaserAmplitude = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.PulseTrain.LaserAmplitude), Settings.PulseTrain.LaserAmplitude));
                        break;
                    case Registers.ScreenBrightness:
                        Settings.ScreenBrightness = message.GetPayloadByte();
                        break;
                    case Registers.StimWavelength:
                        Settings.PulseTrain.LaserWavelength = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.PulseTrain.LaserWavelength), Settings.PulseTrain.LaserWavelength));
                        break;
                    case Registers.StimPeriod:
                        Settings.PulseTrain.StimPeriod = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.PulseTrain.StimPeriod), Settings.PulseTrain.StimPeriod));
                        break;
                    case Registers.StimOn:
                        Settings.PulseTrain.StimOn = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.PulseTrain.StimOn), Settings.PulseTrain.StimOn));
                        break;
                    case Registers.StimReps:
                        Settings.PulseTrain.StimReps = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.PulseTrain.StimReps), Settings.PulseTrain.StimReps));
                        break;
                    case Registers.Out1Conf:
                        Settings.DigitalOutput1 = (DigitalOutputConfiguration)message.GetPayloadByte();
                        break;
                    case Registers.Out0Conf:
                        Settings.DigitalOutput0 = (DigitalOutputConfiguration)message.GetPayloadByte();
                        break;
                    case Registers.In1Conf:
                        Settings.DigitalInput1 = (DigitalInputConfiguration)message.GetPayloadByte();
                        break;
                    case Registers.In0Conf:
                        Settings.DigitalInput0 = (DigitalInputConfiguration)message.GetPayloadByte();
                        break;
                    case Registers.TriggerState:
                        Settings.TriggerSequence = message.GetPayloadArray<byte>();
                        var frameFlags = (FrameFlags[])TypeDescriptor.GetProperties(Settings).Find(nameof(Settings.TriggerSequence), false).Converter.ConvertTo(null, null, Settings.TriggerSequence, typeof(FrameFlags[]));
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.TriggerSequence), frameFlags));
                        break;
                    case Registers.TriggerStateLength:
                        Settings.TriggerSequenceLength = message.GetPayloadByte();
                        break;
                    case Registers.TriggerPeriod:
                        Settings.TriggerPeriod = message.GetPayloadUInt16();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs(nameof(Settings.TriggerPeriod), Settings.TriggerPeriod));
                        break;
                    case Registers.TriggerTime:
                        Settings.TriggerTime = message.GetPayloadUInt16();
                        break;
                    case Registers.TriggerTimeUpdateOutputs:
                        Settings.TriggerTimeUpdateOutputs = message.GetPayloadUInt16();
                        break;
                    case Registers.SerialNumber:
                        Settings.SerialNumber = message.GetPayloadUInt64();
                        break;
                    case Registers.StimKeySwitchState:
                        Settings.StimKeySwitchState = message.GetPayloadByte();
                        SettingChanged.Invoke(this, new RegSettingChangedArgs("StimKeySwitchState", Settings.StimKeySwitchState));
                        break;
                    //case Registers.TriggerLaserOn:
                    //    Settings.TriggerLaserOn = message.GetPayloadUInt16();
                    //    break;
                    //case Registers.TriggerLaserOff:
                    //    Settings.TriggerLaserOff = message.GetPayloadUInt16();
                    //    break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot Read Register: {0}\nMessage = {1}", message.Address, ex.Message);
            }
        }

        internal IEnumerable<HarpMessage> WritePropToReg(RegSettingChangedArgs arg)
        {
            switch (arg.Name)
            {
                case nameof(Settings.LEDPowers.L415):
                    yield return HarpCommand.WriteUInt16(Registers.DacL415, (ushort)arg.Value);
                    break;
                case nameof(Settings.LEDPowers.L470):
                    yield return HarpCommand.WriteUInt16(Registers.DacL470, (ushort)arg.Value);
                    break;
                case nameof(Settings.LEDPowers.L560):
                    yield return HarpCommand.WriteUInt16(Registers.DacL560, (ushort)arg.Value);
                    break;
                case nameof(Settings.PulseTrain.LaserAmplitude):
                    yield return HarpCommand.WriteUInt16(Registers.DacLaser, (ushort)arg.Value);
                    break;
                case nameof(Settings.PulseTrain.StimPeriod):
                    yield return HarpCommand.WriteUInt16(Registers.StimPeriod, (ushort)arg.Value);
                    break;
                case "TabPage":
                    yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.Stop);
                    yield return HarpCommand.WriteUInt16(Registers.DacL415, 0);
                    yield return HarpCommand.WriteUInt16(Registers.DacL470, 0);
                    yield return HarpCommand.WriteUInt16(Registers.DacL560, 0);
                    break;
                case "StartCalLaser":
                    var pulseTrain = (PulseTrain)arg.Value;
                    yield return HarpCommand.WriteUInt16(Registers.StimWavelength, pulseTrain.LaserWavelength);
                    yield return HarpCommand.WriteUInt16(Registers.DacLaser, pulseTrain.LaserAmplitude);
                    yield return HarpCommand.WriteUInt16(Registers.StimPeriod, pulseTrain.StimPeriod);
                    yield return HarpCommand.WriteUInt16(Registers.StimOn, pulseTrain.StimOn);
                    yield return HarpCommand.WriteUInt16(Registers.StimReps, pulseTrain.StimReps);
                    yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.StartContinuous);
                    break;
                case "StopCalLaser":
                    yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.Stop);
                    yield return HarpCommand.WriteUInt16(Registers.StimWavelength, Settings.PulseTrain.LaserWavelength);
                    yield return HarpCommand.WriteUInt16(Registers.DacLaser, Settings.PulseTrain.LaserAmplitude);
                    yield return HarpCommand.WriteUInt16(Registers.StimPeriod, Settings.PulseTrain.StimPeriod);
                    yield return HarpCommand.WriteUInt16(Registers.StimOn, Settings.PulseTrain.StimOn);
                    yield return HarpCommand.WriteUInt16(Registers.StimReps, Settings.PulseTrain.StimReps);
                    break;
                case nameof(Settings):
                    yield return HarpCommand.WriteByte(Registers.StimStart, (byte)StimulationCommand.Stop);
                    yield return HarpCommand.WriteByte(Registers.Start, (byte)(AcquisitionModes.StopPhotometry | AcquisitionModes.StopExternalCamera));

                    yield return HarpCommand.WriteUInt16(Registers.Config, Settings.Config);
                    yield return HarpCommand.WriteByte(Registers.ScreenBrightness, Settings.ScreenBrightness);
                    yield return HarpCommand.WriteByte(Registers.TriggerState, Settings.TriggerSequence);
                    yield return HarpCommand.WriteByte(Registers.TriggerStateLength, Settings.TriggerSequenceLength);
                    yield return HarpCommand.WriteUInt16(Registers.TriggerPeriod, Settings.TriggerPeriod);
                    yield return HarpCommand.WriteUInt16(Registers.TriggerTimeUpdateOutputs, Settings.TriggerTimeUpdateOutputs);
                    //yield return HarpCommand.WriteUInt16(Registers.TriggerLaserOn, Settings.TriggerLaserOn);
                    //yield return HarpCommand.WriteUInt16(Registers.TriggerLaserOff, Settings.TriggerLaserOff);
                    yield return HarpCommand.WriteUInt16(Registers.DacL415, Settings.LEDPowers.L415);
                    yield return HarpCommand.WriteUInt16(Registers.DacL470, Settings.LEDPowers.L470);
                    yield return HarpCommand.WriteUInt16(Registers.DacL560, Settings.LEDPowers.L560);
                    yield return HarpCommand.WriteByte(Registers.Out0Conf, (byte)Settings.DigitalOutput0);
                    yield return HarpCommand.WriteByte(Registers.Out1Conf, (byte)Settings.DigitalOutput1);
                    yield return HarpCommand.WriteByte(Registers.In0Conf, (byte)Settings.DigitalInput0);
                    yield return HarpCommand.WriteByte(Registers.In1Conf, (byte)Settings.DigitalInput1);


                    yield return HarpCommand.WriteUInt16(Registers.StimWavelength, Settings.PulseTrain.LaserWavelength);
                    yield return HarpCommand.WriteUInt16(Registers.DacLaser, Settings.PulseTrain.LaserAmplitude);
                    yield return HarpCommand.WriteUInt16(Registers.StimPeriod, Settings.PulseTrain.StimPeriod);
                    yield return HarpCommand.WriteUInt16(Registers.StimOn, Settings.PulseTrain.StimOn);
                    yield return HarpCommand.WriteUInt16(Registers.StimReps, Settings.PulseTrain.StimReps);

                    break;
                default: yield break;
            }
        }

        internal void UpdateProp(RegSettingChangedArgs arg)
        {
            
            switch (arg.Name)
            {
                case nameof(Settings.TriggerSequence):
                    var trigSeq = TypeDescriptor.GetProperties(Settings).Find(nameof(Settings.TriggerSequence), false).Converter.ConvertTo(null, null, (FrameFlags[])arg.Value, typeof(byte[]));
                    Settings.TriggerSequence = (byte[])trigSeq;
                    goto default;
                case nameof(Settings.LEDPowers.L415):
                    Settings.LEDPowers.L415 = (ushort)arg.Value;
                    goto default;
                case nameof(Settings.LEDPowers.L470):
                    Settings.LEDPowers.L470 = (ushort)arg.Value;
                    goto default;
                case nameof(Settings.LEDPowers.L560):
                    Settings.LEDPowers.L560 = (ushort)arg.Value;
                    goto default;
                case nameof(Settings.PulseTrain.LaserWavelength):
                    Settings.PulseTrain.LaserWavelength = (ushort)arg.Value;
                    goto default;
                case nameof(Settings.PulseTrain.LaserAmplitude):
                    Settings.PulseTrain.LaserAmplitude = (ushort)arg.Value;
                    goto default;
                case nameof(Settings.PulseTrain.StimPeriod):
                    Settings.PulseTrain.StimPeriod = (ushort)arg.Value;
                    goto default;
                case nameof(Settings.PulseTrain.StimOn):
                    Settings.PulseTrain.StimOn = (ushort)arg.Value;
                    goto default;
                case nameof(Settings.PulseTrain.StimReps):
                    Settings.PulseTrain.StimReps = (ushort)arg.Value;
                    goto default;
                case nameof(Settings.Regions):
                    Settings.Regions = (List<PhotometryRegion>)arg.Value;
                    break;
                default:
                    Settings_PropGrid.Refresh();
                    break;

            }
        }
        #region Initialize Key Filtering
        /// <summary>
        /// Handles the <see cref="Control.KeyPress"/> event thrown by the children of the <see cref="Settings_PropGrid"/>
        /// with <see cref="GridItem_KeyFiltering(object, KeyPressEventArgs)"/>.
        /// </summary>
        private void AddKeyFiltering()
        {
            // Get the control Hierarchy of the active control on the property grid
            var activeControl = Settings_PropGrid.ActiveControl;
            var activeControlHierarchy = GetControlHierarchy(activeControl);

            // For each control in the hierarchy:
            foreach (Control control in activeControl.Controls)
            {
                // Add KeyFiltering the KeyPress events.
                control.KeyPress += GridItem_KeyFiltering;
            }
        }

        /// <summary>
        /// Recursively, finds the hierarchy of controls belonging to a root <see cref="Control"/>.
        /// Creates a <see cref="Queue{Control}"/> starting with the root <see cref="Control"/>.
        /// While the queue is not empty, remove the next control of the queue, yield returning it.
        /// Then add of that control's children to the queue.
        /// </summary>
        /// <param name="root"><see cref="Control"/> that is the Active Control of the <see cref="Settings_PropGrid"/>.</param>
        /// <returns></returns>
        private IEnumerable<Control> GetControlHierarchy(Control root)
        {
            // Create a control queue
            var queue = new Queue<Control>();

            // Add the root control to the queue
            queue.Enqueue(root);

            // Recursively find the next child controls while there are controls in the queue.
            do
            {
                // Get the next control in the queue.
                var control = queue.Dequeue();

                // Yield return the next control.
                yield return control;

                // Add each child of the next control to the queue
                foreach (var child in control.Controls.OfType<Control>())
                    queue.Enqueue(child);

            } while (queue.Count > 0);
        }

        /// <summary>
        /// Gets the <see cref="KeyPressEventArgs.KeyChar"/> and references the 
        /// <see cref="CurrentKeyFilter"/> which is updated when handling the
        /// SelectedGridItemChanged event with <see cref="Settings_PropGrid_SelectedGridItemChanged"/>.
        /// Uses a particular <see cref="KeyFilteringHelpers"/> method based on the 
        /// <see cref="CurrentKeyFilter"/>.
        /// </summary>
        /// <param name="sender">
        /// <see cref="Control"/> in the <see cref="Settings_PropGrid"/> that is being edited by the user.
        /// </param>
        /// <param name="e">
        /// <see cref="KeyPressEventArgs"/> used to get the <see cref="KeyPressEventArgs.KeyChar"/>
        /// and to handle the event if an invalid character was entered.
        /// </param>
        private void GridItem_KeyFiltering(object sender, KeyPressEventArgs e)
        {
            // Get the char of the key pressed
            var keyChar = e.KeyChar;
            bool invalidCharEntered;

            // Filter based on the specified type of key filter, tracking if an invalid char was entered.
            switch (CurrentKeyFilter)
            {
                case KeyFilterType.IntegerNumeric:
                    invalidCharEntered = !KeyFilteringHelpers.IsIntegerNumeric(keyChar);
                    break;
                case KeyFilterType.DecimalNumeric:
                    invalidCharEntered = !KeyFilteringHelpers.IsDecimalNumeric(keyChar);
                    break;
                default:
                    invalidCharEntered = false;
                    break;
            }

            // If and invalid char was entered:
            if (invalidCharEntered == true)
            {
                // Stop the character from being entered into the Control.
                e.Handled = true;
            }
        }
        #endregion

        #region Event Handling

        /// <summary>
        /// Updates the <see cref="CurrentKeyFilter"/> when the selected grid item 
        /// changed event is thrown by the <see cref="Settings_PropGrid"/>. 
        /// Gets <see cref="PropertyDescriptor"/> of the <see cref="SelectedGridItemChangedEventArgs.NewSelection"/>.
        /// Gets the <see cref="KeyFilterType"/> from the <see cref="KeyFilterAttribute"/>, updating the
        /// <see cref="CurrentKeyFilter"/>.
        /// </summary>
        /// <param name="sender"><see cref="Settings_PropGrid"/> displaying the <see cref="SystemSettings"/>.</param>
        /// <param name="e"><see cref="SelectedGridItemChangedEventArgs"/> containing the new selection.</param>
        private void Settings_PropGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            // Find the property descriptor of the new selection
            var selectedGridItem = e.NewSelection;
            var propDesc = selectedGridItem.PropertyDescriptor;
            if (propDesc != null)
            {
                // Find the KeyFilterAttributes in the property description
                var keyFilterAttrs = propDesc.Attributes.OfType<KeyFilterAttribute>();

                // If there are not KeyFilterAttributes
                if (keyFilterAttrs.Count() == 0)
                    CurrentKeyFilter = KeyFilterType.None;  // Set CurrentKeyFilter to None
                // If there is one KeyFilterAttribute
                else if (keyFilterAttrs.Count() == 1)
                    CurrentKeyFilter = keyFilterAttrs.First().KeyFilterType;    // Set CurrentKeyFilter to the KeyFilterType
            }
        }

        #endregion

        

        private void Settings_PropGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (SettingChanged != null)
                SettingChanged.Invoke(this, new RegSettingChangedArgs(e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value));
        }

        private void RegSettingChanged(object sender, RegSettingChangedArgs args)
        {
            if (SettingChanged != null)
                SettingChanged.Invoke(this, args);
        }
    }
}
