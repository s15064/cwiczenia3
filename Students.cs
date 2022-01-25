using System;

namespace cwiczenia3
{
    public class Students
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string id { get; set; }
        public string birthdate { get; set; }
        public string studies { get; set; }
        public string mode { get; set; }
        public string email { get; set; }
        public string fathersName { get; set; }
        public string mothersName { get; set; }
    
        public string CSV()
        {
            string temp =
                this.name + "," +
                this.surname + "," +
                this.studies + "," +
                this.mode + "," +
                this.id + "," +
                this.birthdate + "," +
                this.email + "," +
                this.fathersName + "," +
                this.mothersName;
            return temp;
        }
    }

}