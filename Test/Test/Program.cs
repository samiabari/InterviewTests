using System;
using System.Collections.Generic;
using System.Linq;

namespace WPH.DotNetInterview
{

    public class Appointment
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationInMinutes { get; set; }
        public string Location { get; set; }

        public Appointment(string name, DateTime startTime, int durationInMinutes, string location)
        {
            Name = name;
            StartTime = startTime;
            DurationInMinutes = durationInMinutes;
            Location = location;
        }
    }

    public class Calendar
    {
        private List<Appointment> appointments;

        public Calendar()
        {
            appointments = new List<Appointment>();
        }

        public bool ScheduleAppointment(Appointment newAppointment)
        {
            if (IsOverlapping(newAppointment))
            {
                return false;
            }
            appointments.Add(newAppointment);
            return true;
        }

        public void DeleteAppointment(string name)
        {
            List<Appointment> result = GetAppointments().ToList();
            foreach (var appointment in result)
            {
                if (appointment.Name == name)
                {
                    appointments.Remove(appointment);
                 
                }
            }
           
        }

        private bool IsOverlapping(Appointment newAppointment)
        {

            List<Appointment> result = GetAppointments().ToList();
            foreach (var appointment in result)
            {
                if (appointment.Name == newAppointment.Name)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Appointment> GetAppointments()
        {
            List<Appointment> appoints = appointments.ToList() ;
            return appointments;
        }

        public List<Appointment> GetAppointmentsByLocation(string location)
        {
            List<Appointment> appoints = appointments.Where(x=>x.Location==location).ToList();
            return appointments;
        }
    }

    public class Program
    {

        public static void Main(string[] args)
        {
            RunTests();
        }

        static void RunTests()
        {
            TestScheduleAppointment_NoOverlap_ReturnsTrue();
            TestScheduleAppointment_WithOverlap_ReturnsFalse();
            TestDeleteAppointment_ExistingAppointment_DeletesSuccessfully();
            TestGetAppointments_ReturnsSortedAppointments();
            TestGetAppointmentsByLocation_ReturnsAppointmentsForLocation();
        }

        static void TestScheduleAppointment_NoOverlap_ReturnsTrue()
        {
            var calendar = new Calendar();
            var appointment = new Appointment("Meeting", DateTime.Now, 60, "Office");
            bool result;
            try
            {
                result = calendar.ScheduleAppointment(appointment);
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine($"TestScheduleAppointment_NoOverlap_ReturnsTrue: Failed ({e.Message})");
                return;
            }

            Console.WriteLine($"TestScheduleAppointment_NoOverlap_ReturnsTrue: {(result ? "Passed" : "Failed")}");
        }

        static void TestScheduleAppointment_WithOverlap_ReturnsFalse()
        {
            var calendar = new Calendar();
            var appointment1 = new Appointment("Meeting", DateTime.Now, 60, "Office");
            var appointment2 = new Appointment("Follow-up", DateTime.Now.AddMinutes(30), 60, "Office");

            bool result;
            try
            {
                calendar.ScheduleAppointment(appointment1);
                result = calendar.ScheduleAppointment(appointment2);
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine($"TestScheduleAppointment_WithOverlap_ReturnsFalse: Failed ({e.Message})");
                return;
            }

            Console.WriteLine($"TestScheduleAppointment_WithOverlap_ReturnsFalse: {(!result ? "Passed" : "Failed")}");
        }

        static void TestDeleteAppointment_ExistingAppointment_DeletesSuccessfully()
        {
            var calendar = new Calendar();
            var appointment = new Appointment("Meeting", DateTime.Now, 60, "Office");

            try
            {
                calendar.ScheduleAppointment(appointment);
                calendar.DeleteAppointment("Meeting");
                var appointments = calendar.GetAppointments();
                Console.WriteLine($"TestDeleteAppointment_ExistingAppointment_DeletesSuccessfully: {(appointments.Count == 0 ? "Passed" : "Failed")}");
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine($"TestDeleteAppointment_ExistingAppointment_DeletesSuccessfully: Failed ({e.Message})");
            }
        }

        static void TestGetAppointments_ReturnsSortedAppointments()
        {
            var calendar = new Calendar();
            var appointment1 = new Appointment("Meeting", DateTime.Now.AddHours(1), 60, "Office");
            var appointment2 = new Appointment("Follow-up", DateTime.Now, 30, "Office");

            List<Appointment> appointments;
            try
            {
                calendar.ScheduleAppointment(appointment1);
                calendar.ScheduleAppointment(appointment2);
                appointments = calendar.GetAppointments();
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine($"TestGetAppointments_ReturnsSortedAppointments: Failed ({e.Message})");
                return;
            }

            var isSorted = appointments[0].Name == "Follow-up" && appointments[1].Name == "Meeting";
            Console.WriteLine($"TestGetAppointments_ReturnsSortedAppointments: {(isSorted ? "Passed" : "Failed")}");
        }

        static void TestGetAppointmentsByLocation_ReturnsAppointmentsForLocation()
        {
            var calendar = new Calendar();
            var appointment1 = new Appointment("Meeting", DateTime.Now, 60, "Office");
            var appointment2 = new Appointment("Follow-up", DateTime.Now, 30, "Home");

            List<Appointment> officeAppointments;
            try
            {
                calendar.ScheduleAppointment(appointment1);
                calendar.ScheduleAppointment(appointment2);
                officeAppointments = calendar.GetAppointmentsByLocation("Office");
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine($"TestGetAppointmentsByLocation_ReturnsAppointmentsForLocation: Failed ({e.Message})");
                return;
            }

            var correctAppointment = officeAppointments.Count == 1 && officeAppointments[0].Name == "Meeting";
            Console.WriteLine($"TestGetAppointmentsByLocation_ReturnsAppointmentsForLocation: {(correctAppointment ? "Passed" : "Failed")}");
        }
    }
}