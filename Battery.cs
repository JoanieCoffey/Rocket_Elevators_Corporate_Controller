using System;
using System.Collections.Generic;
using System.Text;

namespace RocketElevatorsCorporateController
{
    class Battery
    {
        public int amount_batteries;
        public Battery(int amount_batteries)
        {
            for (var i = 0; i < amount_batteries; i++)
            {
                new Column(10, 2);
            }
        }
    }
}
