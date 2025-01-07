using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVR.Locations;
using VVR.Vehicles; 

namespace VVR.VVR_logic
{
    internal class AiDriver
    {
        public void Drive(int carIndex, ref int deltaPosX, ref float deltaSpeed, Track track, List<Vehicle> vehicles)
        {
            // needed changes: 
            // car finding 
            // wall finding but considering wheels
            // using  Denis functions 
            if (vehicles[carIndex].positionY < 0) return;
            int humanIndex = vehicles.FindIndex(v => v.isHuman == true);
            int PosY = (int)Math.Round(vehicles[carIndex].positionY);
            int predictedPosition = PosY + 2*(int)Math.Round((vehicles[carIndex].speed / vehicles[humanIndex].speed));
            if (predictedPosition > track.trackPieces.Count)
            {
                predictedPosition -= (track.trackPieces.Count + 1);
            }
            predictedPosition = (track.trackPieces.Count + track.trackPieces.Count - predictedPosition) % track.trackPieces.Count;
            if (predictedPosition < 0 || predictedPosition >= track.trackPieces.Count) return;
            if (track.trackPieces[predictedPosition].leftBorder >= vehicles[carIndex].positionX)
                deltaPosX++;
            else if (track.trackPieces[predictedPosition].rightBorder <= vehicles[carIndex].positionX)
                deltaPosX--;

            //Console.SetCursorPosition(0, 22);
            //Console.Write($"{vehicles[0].positionY}     ");

            //Console.SetCursorPosition(0, 23);
            //Console.Write($"{vehicles[1].positionY}     ");
            //Console.SetCursorPosition(0, 24);
            //Console.Write($"{vehicles[2].positionY}     ");
            //Console.SetCursorPosition(0, 25);
            //Console.Write($"{vehicles[3].positionY}     ");
            
            //Console.SetCursorPosition(0, 26);
            //Console.Write($"{vehicles[4].positionY} ");
            //Console.SetCursorPosition(10, 26);
            //if (carIndex == 4)
            //    Console.Write($"{predictedPosition}     ");
            //Thread.Sleep(100);
        }
    }
}
