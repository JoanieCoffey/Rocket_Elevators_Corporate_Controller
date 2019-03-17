using System;
using System.Linq;
using System.Collections.Generic;

namespace RocketElevatorsCorporateController
{
    class Column
    {
        // Propriétés 
        public int floorNumber;
        public int elevatorNumber;
        public List<CallButton> callButtons = new List<CallButton>();
        public List<Elevator> elevators = new List<Elevator>();

        // Méthodes
        public Column(int floorNumber, int elevatorNumber)
        {
            this.floorNumber = floorNumber;
            this.elevatorNumber = elevatorNumber;

            // On crée tout les boutons d'appels "UP" et "DOWN" en fonction du nombre d'étages
            this.callButtonsCreation();
            // On crée tout les elevators qui ont été demandé en fonction de "elevatorNumber"
            this.elevatorCreate();
        }

        // On crée le nombre d'elevators demandé, qu'on ajoute au tableau "elevators"
        public void elevatorCreate()
        {
            for (int i = 0; i < this.elevatorNumber; i++)
            {
                // On crée les ascenceurs qu'on place au premier étage
                this.elevators.Add(new Elevator(1, this.floorNumber));
            }
        }

        // En fonction du nombre d'étages, on crée des boutons d'appel, qu'on ajoute au tableau "callButtons"
        public void callButtonsCreation()
        {
            for (int i = 0; i < this.floorNumber; i++)
            {
                // Si i est le dernier étage, alors on ne met pas de bouton UP
                if (i != this.floorNumber - 1)
                {
                    // On créer un bouton d'appel "UP" qu'on rajoute au tableaux de boutons d'appels
                    this.callButtons.Add(new CallButton(i + 1, "up"));
                }
                // Si i est le rez-de-chaussée (0) on ne met pas de bouton DOWN
                if (i != 0)
                {
                    // On créer un bouton d'appel "DOWN" qu'on rajoute au tableaux de boutons d'appels
                    this.callButtons.Add(new CallButton(i + 1, "down"));
                }
            }
        }

        // Trouver l'ascenceur le plus proche de l'étage appelé
        public Elevator nearestElevator(int calledfloor)
        {
            Elevator nearestElevator = null;
            while (nearestElevator == null)
            {
                List<int> differences = new List<int>();

                // Pour chaque elevateur
                for (int i = 0; i < this.elevators.Count; i++)
                {
                    // Calcul de la différence entre l'étage de l'élévateur et l'étage appelé
                    differences.Add(Math.Abs(this.elevators[i].floor - calledfloor));
                }

                // Pour chaque difference calculée
                for (int j = 0; j < differences.Count; j++)
                {
                    // Si la valeur la différence la plus petite est la différence entre l'étage de l'elevateur actuel et l'étage appelé
                    if (
                        differences.Min() ==
                        Math.Abs(this.elevators[j].floor - calledfloor)
                    )
                    {
                        // Alors on choisit cette elevateur
                        nearestElevator = this.elevators[j];
                    }
                }
            }
            return nearestElevator;
        }

        public Elevator findElevator(int calledfloor, string direction)
        {
            // Tant que aucune elevator n'a été trouvé, recommencer
            Elevator elevatorChoosed = null;
            while (elevatorChoosed == null)
            {
                // Pour chaque ascenceur
                for (int i = 0; i < this.elevators.Count; i++)
                {
                    // Si l'étage de l'ascenceur est le même que celui où on l'appel et qu'il est en "available"
                    if (
                        this.elevators[i].floor == calledfloor &&
                        this.elevators[i].status == "available"
                    )
                    {
                        // La direction de cette ascenceur est celle du bouton appuyé
                        this.elevators[i].direction = direction;
                        // Cette ascenceur va répondre à la requête
                        elevatorChoosed = this.elevators[i];
                    }
                    // Sinon si l'élevateur est en status "available"
                    else if (this.elevators[i].status == "available")
                    {
                        // On recherche l'élevateur le plus proche
                        Elevator nearestElevator = this.nearestElevator(calledfloor);
                        // On passe l'elevateur le plus proche en status "unavailable"
                        nearestElevator.status = "unavailable";
                        // Si l'étage de l'elevateur le plus proche est plus grand que celui appelé
                        if (nearestElevator.floor > calledfloor)
                        {
                            // Sa déstination devient down
                            nearestElevator.direction = "down";
                        }
                        // Si l'étage de l'elevateur le plus proche est plus petit que celui appelé
                        else if (nearestElevator.floor < calledfloor)
                        {
                            // Sa déstination devient up
                            nearestElevator.direction = "up";
                        }
                        // Sinon on prend la direction demandé
                        else
                        {
                            nearestElevator.direction = direction;
                        }

                        elevatorChoosed = nearestElevator;
                    }
                    // Si l'elevateur est en status "unavailable" et que la direction est la même que celle demandé
                    else if (
                        this.elevators[i].status == "unavailable" &&
                        this.elevators[i].direction == direction
                    )
                    {
                        // Si la direction est up et que son étage est plus petit que l'étage appelé
                        if (
                        this.elevators[i].direction == "up" &&
                        this.elevators[i].floor < calledfloor
                        )
                        {
                            // On choisit cet
                            elevatorChoosed = this.elevators[i];
                        }
                        // Sinon si sa direction est down et que l'étage de l'elevateur est plus grand que l'étage appelé
                        else if (
                        this.elevators[i].direction == "down" &&
                        this.elevators[i].floor > calledfloor
                        )
                        {
                            // On choisit cet ascenceur
                            elevatorChoosed = this.elevators[i];
                        }
                    }
                }
            }
            return elevatorChoosed;
        }

        // Si on se trouve au dixième étage et qu'on appuie sur le bouton "DOWN"
        // On demande qu'un elevateur viennent où on est (exemple 10ème etage), UP ou DOWN est juste pour optimiser
        public Elevator RequestElevator(int calledfloor, string direction)
        {
            // On affiche que l'ascenceur a été demandé à l'étage en question
            Console.WriteLine(
            "Elevator requested at the floor " +
                calledfloor +
                " with " +
                direction +
                " option."
            );
            // On recherche l'elevateur qui doit répondre à la demande
            Elevator elevator = this.findElevator(calledfloor, direction);
            Console.WriteLine("Elevator selected at floor " + elevator.floor);

            // On rajoute l'étage appelé à la liste des étages en attente et on trie les étages en attente pour optimiser le trajet
            elevator.floorAwaiting(calledfloor);
            // L'elevator se déplace
            elevator.elevatorMoves(calledfloor);

            return elevator;
        }
    }
}