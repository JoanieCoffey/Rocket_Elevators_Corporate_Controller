using System;

namespace RocketElevatorsCorporateController
{
    class Program
    {
        static void Main(string[] args)
        {
            // Début d'utilisation du programme:

            // Création de tout, colonne, ascenceurs, bouton d'appels DOWN et UP, bouton de requete placé dans l'ascenceurs
            Column column = new Column(10, 2);

            Elevator currentElevator;
            // Appuyer sur le bouton d'appel à un certain étage
            // Gestion de l'appel d'un elevateur qui sera selectionné par le controlleur jusqu'à son arrivée
            currentElevator = column.RequestElevator(5, "up");
            // Une fois dans l'ascenceur appuyé sur le bouton de l'étage où on veut aller
            currentElevator.RequestFloor(9);

            currentElevator = column.RequestElevator(4, "down");
            currentElevator.RequestFloor(1);

            Console.ReadLine();
        }
    }
}
