using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace RocketElevatorsCorporateController
{
    class Elevator
    {
        public int floor;
        public int floorNumber;
        public string status;
        public string direction;
        public string door;
        public List<int> floorList = new List<int>(); // Liste de nombres
        public List<int> unavailableList = new List<int>(); // Liste de nombres
        public List<FloorRequestButton> floorRequestButtons = new List<FloorRequestButton>(); // Liste d'objet FloorRequestButton

        public Elevator(int floor, int floorNumber)
        {
            this.status = "available"; // Un elevateur est en available lors de sa création
            this.floor = floor;
            this.direction = "none"; // Un elevateur n'a pas de direction lors de sa création
            this.floorNumber = floorNumber;
            this.door = "closed";

            // Si j'ai 10 étages, je crée 10 boutons dans chaque ascenceur pour pouvoir voyager entre les différents étages
            for (int i = 0; i < this.floorNumber; i++)
            {
                // +1 car on veut que les boutons aillent de 1 à 10
                this.floorRequestButtons.Add(new FloorRequestButton(i + 1));
            }
        }

        public void elevatorUp()
        {
            while (
            this.floor < this.unavailableList.DefaultIfEmpty().Min() &&
            this.floor < this.floorList.DefaultIfEmpty().Min()
            )
            {

                Console.WriteLine("Elevator going up, at floor:  " + this.floor++);
                Thread.Sleep(1000);
            }
        }

        public void elevatorDown()
        {
            while (
            this.floor > this.floorList.DefaultIfEmpty().Max() &&
            this.floor > this.unavailableList.DefaultIfEmpty().Max()
            )
            {
                Console.WriteLine("Elevator going down, at floor:  " + this.floor--);
                Thread.Sleep(1000);
            }
        }

        public void floorAwaiting(int calledfloor)
        {
            if (this.status == "unavailable")
            {
                // On rajoute l'étage demandé à la liste "de unavailable" zuq171
                this.unavailableList.Add(calledfloor);
                // Trie la liste des étages en attente
                this.unavailableList.Sort(sortFloorLists);
            }
            // S'il est en available, on ne fait rien
            else if (this.status == "available") { }
        }

        public int sortFloorLists(int a, int b)
        {
            // Trie les nombres d'un tableau
            // https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Objets_globaux/Array/sort
            // Si le premier nombre - le deuxième donne un resultat en-dessous de 0 : On ne bouge rien
            // Si égale à 0 - On bouge rien
            // Si + grand que 0 - On échange les deux nombres de place
            return a - b;
        }

        public void elevatorMoves(int requestedfloor)
        {
            // Tant qu'il reste des étages en attente, on continue
            while (this.floorList.Count > 0)
            {
                // Si la detination est down
                if (this.direction == "down")
                {
                    // On descend l'elevateur
                    this.elevatorDown();
                    // Supprime le dernier étage en attente de la liste
                    this.floorList.RemoveAt(this.floorList.Count - 1);
                }
                // Si la direction est up
                else if (this.direction == "up")
                {
                    // On descend l'elevateur
                    this.elevatorUp();
                    // Supprime le premier étage en attente de la liste
                    this.floorList.RemoveAt(0);
                }
                // Procedure d'arrivage de l'elevateur
                this.elevatorArrival(requestedfloor);
            }
            // Tant qu'il reste des unavailable, même principe qu'avant
            while (this.unavailableList.Count > 0)
            {
                if (this.direction == "down")
                {
                    this.elevatorDown();
                    this.unavailableList.RemoveAt(this.unavailableList.Count - 1);
                }
                else if (this.direction == "up")
                {
                    this.elevatorUp();
                    this.unavailableList.RemoveAt(0);
                }
                this.elevatorArrival(requestedfloor);
            }

            // Une fois la procédure d'arrivage est faîtes, on repasse en status + réinitialise la direction
            this.status = "available";
            this.direction = "none";
        }

        public void ElevatorStop()
        {
            // L'elevateur se stop
            this.status = "stopped";
        }

        public void OpenDoor()
        {
            // Portes s'ouvrent
            this.door = "opened";
        }

        public void CloseDoor()
        {
            // Tant que les protes sont obstruées, on ouvre la porte
            while (this.door == "obstructed")
            {
                this.OpenDoor();
            }
            // Lorsqu'elles ne sont plus obstruées, on tente de fermer
            this.door = "closed";
        }

        // Quand il arrive à sa desination
        public void elevatorArrival(int requestedfloor)
        {
            // L'elevator se stop
            this.ElevatorStop();
            Console.WriteLine("Elevator stopped at floor " + requestedfloor);
            // Les portes s'ouvrent
            this.OpenDoor();
            Console.WriteLine("Doors are " + this.door);
            // Les portes se ferment
            this.CloseDoor();
            Console.WriteLine("Doors are " + this.door);
            Console.WriteLine("Elevator ready");
        }

        public void RequestFloor(int requestedfloor)
        {
            // Le status passe en unavailable
            this.status = "unavailable";
            // Si l'étage demandé est plus grand que l'étage actuel de l'élévateur
            if (requestedfloor > this.floor)
            {
                this.direction = "up";
            }
            // Si l'inverse
            else if (requestedfloor < this.floor)
            {
                this.direction = "down";
            }

            // Si up et que l'étage demandé est plus grand que l'actuelle, on ajoute l'étage à ceux en attente
            if (this.direction == "up" && requestedfloor > this.floor)
            {
                this.floorList.Add(requestedfloor);
            }
            // Si down  et que l'étage demandé est plus petit que l'actuelle, on ajoute l'étage à ceux en attente
            else if (this.direction == "down" && requestedfloor < this.floor)
            {
                this.floorList.Add(requestedfloor);
            }
            // L'elevateur se déplace
            this.elevatorMoves(requestedfloor);
        }

    }
}
