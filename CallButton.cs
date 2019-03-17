using System;

namespace RocketElevatorsCorporateController
{
    class CallButton
    {
        public int floor;
        public string direction;
        public CallButton(int floor, string direction)
        {
            this.floor = floor;
            this.direction = direction;
        }
    }
}
