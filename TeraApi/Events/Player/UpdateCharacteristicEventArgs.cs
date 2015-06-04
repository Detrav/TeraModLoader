using System;
using Detrav.TeraApi.Core;
using Detrav.TeraApi.Enums;

namespace Detrav.TeraApi.Events.Player
{
    public class UpdateCharacteristicEventArgs : EventArgs
    {
        public TeraPlayer player {get;private set;}
        public PlayerCharacteristic characteristic { get; private set; }
        public uint value {get; private set;}
        public uint max {get; private set;}
        public UpdateCharacteristicEventArgs(TeraPlayer player, PlayerCharacteristic characteristic, uint value,uint max)
        {
            this.player = player;
            this.characteristic = characteristic;
            this.value = value;
            this.max = max;
        }
    }
    public delegate void OnUpdateCharacteristic(object sender, UpdateCharacteristicEventArgs e);
}