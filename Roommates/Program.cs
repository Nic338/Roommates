using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        string roomInput = Console.ReadLine();
                        int id;
                        if (int.TryParse(roomInput, out id))
                        {
                            Room room = roomRepo.GetById(id);

                            if (room != null)
                            {
                            Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                            }
                            else
                            {
                                Console.WriteLine("Room not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric value");
                        }                    

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                    List< Chore > chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"The '{c.Name}' chore has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a chore"):
                        Console.Write("Chore Id: ");
                        string choreInput = Console.ReadLine();
                        int choreId;
                        if (int.TryParse(choreInput, out choreId))
                        {
                            Chore chore = choreRepo.GetById(choreId);

                            if (chore != null)
                            {
                                Console.WriteLine($"{chore.Id} - {chore.Name}");
                            }
                            else
                            {
                                Console.WriteLine("Chore not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric value");
                        }

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName,
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"The '{choreToAdd.Name}' chore has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"The '{c.Name}' chore has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a roommate"):
                        Console.Write("Roommate Id: ");
                        string roommateInput = Console.ReadLine();
                        int roommateId;
                        if (int.TryParse(roommateInput, out roommateId))
                        {
                            Roommate roommate = roommateRepo.GetById(roommateId);

                            if (roommate != null)
                            {
                                Console.WriteLine($"{roommate.Id} - {roommate.FirstName} Rent Portion: {roommate.RentPortion} They are in the {roommate.Room.Name}");
                            }
                            else
                            {
                                Console.WriteLine("Roommate not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric value");
                        }

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all roommates"):
                        List<Roommate> roommates = roommateRepo.GetAll();
                        foreach (Roommate r in roommates)
                        {
                            Console.WriteLine($"{r.FirstName} {r.LastName} has an Id of {r.Id}, a rent portion of {r.RentPortion}% and they moved in to the {r.Room.Name} on {r.MoveInDate}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a roommate"):
                        Console.Write("Roommate First Name: ");
                        string firstName = Console.ReadLine();

                        Console.Write("Roommate Last Name: ");
                        string lastName = Console.ReadLine();

                        Console.Write("What % of Rent are they paying: ");
                        int parsedRent = int.Parse(Console.ReadLine());

                        Console.Write("What room are they moving in to?: ");
                        int roomId = int.Parse(Console.ReadLine());

                        Roommate roommateToAdd = new Roommate
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            RentPortion = parsedRent,
                            MoveInDate = new DateTime(),
                            Room = roomRepo.GetById(roomId),
                        };

                        roommateRepo.Insert(roommateToAdd);

                        Console.WriteLine($"{roommateToAdd.FirstName} {roommateToAdd.LastName} moved in on {roommateToAdd.MoveInDate} and is paying {roommateToAdd.RentPortion}% of the rent and they are living in {roommateToAdd.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a roommates info"):
                        Console.WriteLine("Which roommate would you like to update?: ");
                        string updatedRoommate = Console.ReadLine();
                        int parsedUpdate;
                        if (int.TryParse(updatedRoommate, out parsedUpdate))
                        {
                            Roommate updatingRoommate = roommateRepo.GetById(parsedUpdate);

                            if (updatingRoommate != null)
                            {
                                Console.WriteLine("Update their information:");

                                Console.Write($"Current First Name: {updatingRoommate.FirstName} | Update to: ");
                                string updateFirstName = Console.ReadLine();

                                Console.Write($"Current Last Name: {updatingRoommate.LastName} | Update to: ");
                                string updateLastName = Console.ReadLine();

                                Console.Write($"Current Rent Portion: {updatingRoommate.RentPortion}% | Update to: ");
                                int updateRentPortion = int.Parse(Console.ReadLine());

                                Console.Write($"Current Move In Date: {updatingRoommate.MoveInDate} | Update to: ");
                                DateTime updateMoveInDate = DateTime.Parse(Console.ReadLine());

                                Console.Write($"Current Room: {updatingRoommate.Room.Name} - ID: {updatingRoommate.Room.Id} | Update to Id: ");
                                int updateRoomId = int.Parse(Console.ReadLine());

                                updatingRoommate.FirstName = updateFirstName;
                                updatingRoommate.LastName = updateLastName;
                                updatingRoommate.RentPortion = updateRentPortion;
                                updatingRoommate.MoveInDate = updateMoveInDate;
                                updatingRoommate.Room = roomRepo.GetById(updateRoomId);

                                roommateRepo.UpdateRoommate(updatingRoommate);

                                Console.WriteLine($"{updatingRoommate.FirstName} {updatingRoommate.LastName} has had their info updated successfully");
                            }
                            else
                            {
                                Console.WriteLine("Roommate Not Found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric value");
                        }
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            break;
                    case ("Delete a roommate"):
                        Console.WriteLine("Which Roommate would you like to delete?: ");
                        string deletedRoommate = Console.ReadLine();
                        int parsedRoommate;
                        if (int.TryParse(deletedRoommate, out parsedRoommate))
                        {
                            Roommate roommate = roommateRepo.GetById(parsedRoommate);

                            if (roommate != null)
                            {
                                roommateRepo.DeleteRoommate(parsedRoommate);
                                Console.WriteLine($"Successfully deleted {roommate.FirstName} {roommate.LastName}");
                            }
                            else
                            {
                                Console.WriteLine("Roommate not found");
                            }
                        }
                        else 
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric value");
                        }

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all chores",
                "Search for a chore",
                "Add a chore",
                "Show all unassigned chores",
                "Show all roommates",
                "Search for a roommate",
                "Add a roommate",
                "Update a roommates info",
                "Delete a roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}