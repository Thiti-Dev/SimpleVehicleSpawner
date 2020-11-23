using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace VehicleSpawner
{
    public class VehicleSpawner : BaseScript
    {
        private uint spawnCount = 0;
        public VehicleSpawner()
        {
            //Constructor
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            //check if this is the exact resource
            if (GetCurrentResourceName() != resourceName) return;

            //Using lambda
            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                string model = "adder";
                if(args.Count > 0)
                {
                    model = args[0].ToString();
                }
                var hash = (uint)GetHashKey(model);
                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {
                    SendClientMessage($"The car model:{model} doesn't seem to be existed in the fivem world");
                }

                //Create a vehicle
                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                //Force player into the vehicle
                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);
                this.spawnCount++;
                SendClientMessage($"Vehicle model: {model} has been spawned, Total Spawned Car -> {this.spawnCount}");
            }),false); // not restrict -> (the last bool)
        }

        private void SendClientMessage(string msg)
        {
            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 0, 0, 255 },
                args = new[] { msg }
            });
        }
    }
}
