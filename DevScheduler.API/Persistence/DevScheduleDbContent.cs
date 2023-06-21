using DevScheduler.API.Entities;

namespace DevScheduler.API.Persistence
{
    public class DevScheduleDbContent
    {
        public List<DevSchedule> DevSchedules { get; set; }

        public DevScheduleDbContent()
        {

            DevSchedules = new List<DevSchedule>();
        }
    }
}
